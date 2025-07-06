/*
	KeePassDiceware Plugin
	Copyright (C) 2021 cmd <https://github.com/cmdwtf>
	Portions Copyright (C) 2014-2021 Mark McGuill. All rights reserved. (Marked with comments.)

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Affero General Public License as published
	by the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Affero General Public License for more details.

	You should have received a copy of the GNU Affero General Public License
	along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;

namespace KeePassDiceware
{
	public class Diceware
	{
		private static readonly char[] LookalikeCharacters = "I|1lO0".ToCharArray();

		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		private static readonly Dictionary<char, string> L33tMap = new()
		{
			{ 'A', "4" },
			{ 'B', "|3" },
			{ 'C', "(" },
			{ 'D', "|)" },
			{ 'E', "3" },
			{ 'F', "|=" },
			{ 'G', "6" },
			{ 'H', "|-|" },
			{ 'I', "|" },
			{ 'J', "9" },
			{ 'K', "|<" },
			{ 'L', "1" },
			{ 'M', "|v|" },
			{ 'N', "|/|" },
			{ 'O', "0" },
			{ 'P', "|*" },
			{ 'Q', "0," },
			{ 'R', "|2" },
			{ 'S', "5" },
			{ 'T', "7" },
			{ 'U', "|_|" },
			{ 'V', "|/" },
			{ 'W', "|/|/" },
			{ 'X', "><" },
			{ 'Y', "`/" },
			{ 'Z', "2" },
		};

		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		private static readonly Dictionary<char, string> L3ssl33tMap = new()
		{
			{ 'A', "4" },
			{ 'E', "3" },
			{ 'G', "6" },
			{ 'I', "|" },
			{ 'J', "9" },
			{ 'L', "1" },
			{ 'O', "0" },
			{ 'S', "5" },
			{ 'T', "7" },
			{ 'Z', "2" },
		};

		public static string Generate(Options options, PwProfile profile, CryptoRandomStream random)
		{
			Debug.Assert(options != null);
			Debug.Assert(profile != null);

			VerifyOptions(options, profile);

			// get wordlists to choose words from
			string[] wordlist = GetWordList(options.WordLists).ToArray();

			if (!wordlist.Any())
			{
				throw new InvalidOperationException("No word lists were selected, cannot generate password.");
			}

			// select the requested number of words
			string[] selectedWords = (from i in Enumerable.Range(0, options.WordCount)
									  select wordlist.SelectRandom(random))
									 .ToArray();

			// mutate the words as requested
			ApplyWordCasing(selectedWords, options.WordCasing, random);
			ApplyL33tSpeak(selectedWords, options.L33tSpeak, random);
			ApplySalt(selectedWords, options.Salt, options.SaltSources, options.WordSeparator, random);
			ApplyAdvancedSettings(selectedWords, options, profile, random);

			// join the mutated words by the selected separator
			string joined = string.Join(options.WordSeparator, selectedWords);

			// and return the result!
			return joined;
		}

		private static void VerifyOptions(Options options, PwProfile profile)
		{
			if (profile.NoRepeatingCharacters)
			{
				throw new NotSupportedException("The 'each character must occur at most once' advanced setting is unsupported for this generator.");
			}

			if (profile.ExcludeLookAlike || profile.ExcludeCharacters.Length > 0)
			{
				if (options.AdvancedStrategy == AdvancedStrategy.SubstitueWordSeparator &&
					options.WordSeparator.Length == 0)
				{
					throw new InvalidOperationException("The excluded character strategy selected was to replace excluded characters with the word separator, but no word separator was specified.");
				}

				if (options.AdvancedStrategy == AdvancedStrategy.SubstitueSalt &&
					options.SaltSources.All(s => !s.Enabled))
				{
					throw new InvalidOperationException("The excluded character strategy selected was to replace excluded characters with salt, but no salt sources were selected.");
				}
			}
		}

		private static void ApplyWordCasing(string[] words, WordCasingType wordCasing, CryptoRandomStream random)
		{
			if (wordCasing == WordCasingType.DoNotChange)
			{
				return;
			}

			for (int scan = 0; scan < words.Length; ++scan)
			{
				switch (wordCasing)
				{
					case WordCasingType.Lowercase:
						words[scan] = words[scan].ToLowerInvariant();
						break;
					case WordCasingType.Uppercase:
						words[scan] = words[scan].ToUpperInvariant();
						break;
					case WordCasingType.TitleCase:
						string first = words[scan][0].ToString().ToUpperInvariant();
						words[scan] = $"{first}{words[scan].Substring(1)}";
						break;
					case WordCasingType.TitleCaseFirst:
						if (scan == 0)
						{
							string titleFirst = words[scan][0].ToString().ToUpperInvariant();
							words[scan] = $"{titleFirst}{words[scan].Substring(1)}";
						}
						break;
					case WordCasingType.TitleCaseRandom:
						if (random.CoinToss())
						{
							string firstLetter = words[scan][0].ToString().ToUpperInvariant();
							words[scan] = $"{firstLetter}{words[scan].Substring(1)}";
						}
						break;
					case WordCasingType.Random:
						char[] randomized = (from c in words[scan].ToCharArray()
											 select (random.CoinToss()
												 ? char.ToUpper(c)
												 : char.ToLower(c))
											)
											.ToArray();
						words[scan] = new string(randomized);
						break;
					case WordCasingType.WholeWord:
						if (random.CoinToss())
						{
							words[scan] = words[scan].ToUpperInvariant();
						}
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(wordCasing));
				}
			}
		}

		private static void ApplyL33tSpeak(string[] words, L33tSpeakType l33tSpeak, CryptoRandomStream random)
		{
			if (l33tSpeak == L33tSpeakType.None)
			{
				return;
			}

			for (int scan = 0; scan < words.Length; ++scan)
			{
				bool mutateWord = l33tSpeak.HasFlag(L33tSpeakType.AllWords)
					|| random.CoinToss();

				if (mutateWord == false)
				{
					continue;
				}

				words[scan] = l33tSpeak.HasFlag(L33tSpeakType.Basic)
					? ApplyCharRemap(words[scan], L3ssl33tMap)
					: ApplyCharRemap(words[scan], L33tMap);
			}
		}

		private static string ApplyCharRemap(string target, Dictionary<char, string> map)
		{
			var sb = new StringBuilder();

			foreach (char c in target)
			{
				if (map.ContainsKey(c))
				{
					sb.Append(map[c]);
				}
				else
				{
					sb.Append(c);
				}
			}

			return sb.ToString();
		}

		private static void ApplySalt(string[] words, SaltType salt, IEnumerable<SaltSource> sources, string separator, CryptoRandomStream random)
		{
			if (salt == SaltType.None)
			{
				return;
			}

			if (salt == SaltType.Sprinkle)
			{
				for (int scan = 0; scan < words.Length; ++scan)
				{
					if (random.CoinToss()) // skip word
					{
						continue;
					}

					int insertAt = random.AtMost(words[scan].Length);

					words[scan] = words[scan].Insert(insertAt, GenerateSalt(sources, random));
				}

				return;
			}

			if (salt == SaltType.BetweenEach)
			{
				// after each word sans the last, insert a generated salt chunk.
				for (int scan = 0; scan < words.Length - 1; ++scan)
				{
					string generatedSalt = GenerateSalt(sources, random);
					words[scan] = $"{words[scan]}{separator}{generatedSalt}";
				}

				return;
			}


			string singleSalt = GenerateSalt(sources, random);

			switch (salt)
			{
				case SaltType.Prefix:
					words[0] = $"{singleSalt}{separator}{words[0]}";
					break;
				case SaltType.Suffix:
					words[words.Length - 1] = $"{words[words.Length - 1]}{singleSalt}";
					break;
				case SaltType.SuffixAsWord:
					words[words.Length - 1] = $"{words[words.Length - 1]}{separator}{singleSalt}";
					break;
				case SaltType.BetweenOne:
					if (words.Length <= 1)
					{
						// invalid case: can't insert between a single word.
						throw new ArgumentOutOfRangeException(nameof(words.Length), "Salt cannot be inserted between a single word.");
					}
					// get a random index word after which to place salt
					int targetIndex = random.AtMost(words.Length - 2);

					words[targetIndex] = $"{words[targetIndex]}{separator}{singleSalt}";
					break;
				case SaltType.SuffixToRandomWord:
					int suffixToIndex = random.AtMost(words.Length - 1);
					words[suffixToIndex] = $"{words[suffixToIndex]}{singleSalt}";
					break;
				case SaltType.PrefixToRandomWord:
					int prefixToIndex = random.AtMost(words.Length - 1);
					words[prefixToIndex] = $"{singleSalt}{words[prefixToIndex]}";
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(salt));
			}
		}

		private static void ApplyAdvancedSettings(string[] words, Options options, PwProfile profile, CryptoRandomStream random)
		{
			if (profile.ExcludeLookAlike == false &&
				profile.ExcludeCharacters.Length == 0)
			{
				return;
			}

			for (int scan = 0; scan < words.Length; ++scan)
			{
				if (profile.ExcludeLookAlike)
				{
					foreach (char lookalike in LookalikeCharacters)
					{
						words[scan] = AdvancedSettingChararacterReplace(words[scan], lookalike, options, random);
					}
				}

				foreach (char excluded in profile.ExcludeCharacters)
				{
					words[scan] = AdvancedSettingChararacterReplace(words[scan], excluded, options, random);
				}
			}

			string AdvancedSettingChararacterReplace(string password, char exclude, Options options, CryptoRandomStream random)
			{
				string excludeStr = exclude.ToString();

				return options.AdvancedStrategy switch
				{
					AdvancedStrategy.Drop => password.Replace(excludeStr, string.Empty),
					AdvancedStrategy.SubstitueWordSeparator => password.Replace(excludeStr, options.WordSeparator),
					AdvancedStrategy.SubstitueSalt => password.Replace(excludeStr, GenerateSalt(options.SaltSources, random)),
					_ => throw new InvalidOperationException($"Unhandled {nameof(AdvancedStrategy)}."),
				};
			}
		}

		public static string GenerateSalt(IEnumerable<SaltSource> sources, CryptoRandomStream random)
		{
			StringBuilder result = null;

			foreach (SaltSource source in sources)
			{
				if (!source.Enabled)
				{
					continue;
				}

				char[] saltOptions = source.GetCharacterPool();

				int length = random.Range(source.MinimumAmount, source.MaximumAmount);
				char[] chars = (from i in Enumerable.Range(0, length)
								select saltOptions.SelectRandom(random))
								.ToArray();

				result ??= new StringBuilder();
				result.Append(chars);
			}

			return result?.ToString().Shuffle(random) ?? string.Empty;
		}

		public static IEnumerable<string> GetWordList(List<WordList> lists)
		{
			HashSet<string> joinedWordList = new HashSet<string>();

			foreach (WordList list in lists.Where(wl => wl.Enabled))
			{
				joinedWordList.UnionWith(list.Get());
			}

			return joinedWordList;
		}
	}
}
