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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;

namespace KeePassDiceware
{
	public class Diceware
	{

		private const string WordListFileExtension = ".txt";

		private const int DefaultSaltMinimumLength = 1;
		private const int DefaultSaltMaximumLength = 4;

		private const string AllUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string AllLower = "abcdefghijklmnopqrstuvwxyz";
		private const string AllDigits = "0123456789";

		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		private const string AllSymbols = "+-=_@#$%^&;:,.<>/~\\[](){}?!|*'\"";
		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		private const string Emojis = @"😀😃😄😁😆😅";
		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		private const string Latin1Supplement =
				"\u00A1\u00A2\u00A3\u00A4\u00A5\u00A6\u00A7" +
				"\u00A8\u00A9\u00AA\u00AB\u00AC\u00AE\u00AF" +
				"\u00B0\u00B1\u00B2\u00B3\u00B4\u00B5\u00B6\u00B7" +
				"\u00B8\u00B9\u00BA\u00BB\u00BC\u00BD\u00BE\u00BF" +
				"\u00C0\u00C1\u00C2\u00C3\u00C4\u00C5\u00C6\u00C7" +
				"\u00C8\u00C9\u00CA\u00CB\u00CC\u00CD\u00CE\u00CF" +
				"\u00D0\u00D1\u00D2\u00D3\u00D4\u00D5\u00D6\u00D7" +
				"\u00D8\u00D9\u00DA\u00DB\u00DC\u00DD\u00DE\u00DF" +
				"\u00E0\u00E1\u00E2\u00E3\u00E4\u00E5\u00E6\u00E7" +
				"\u00E8\u00E9\u00EA\u00EB\u00EC\u00ED\u00EE\u00EF" +
				"\u00F0\u00F1\u00F2\u00F3\u00F4\u00F5\u00F6\u00F7" +
				"\u00F8\u00F9\u00FA\u00FB\u00FC\u00FD\u00FE\u00FF";

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

		private static readonly Dictionary<WordLists, string[]> LoadedLists = new();

		private static readonly Dictionary<SaltSources, char[]> SaltOptionCache = new();

		private static char[] GetSaltOptions(SaltSources sources)
		{
			if (SaltOptionCache.ContainsKey(sources))
			{
				return SaltOptionCache[sources];
			}

			var sourceLists = new List<string>();

			if (sources.HasFlag(SaltSources.Uppercase))
			{
				sourceLists.Add(AllUpper);
			}
			if (sources.HasFlag(SaltSources.Lowercase))
			{
				sourceLists.Add(AllLower);
			}
			if (sources.HasFlag(SaltSources.Digits))
			{
				sourceLists.Add(AllDigits);
			}
			if (sources.HasFlag(SaltSources.Symbols))
			{
				sourceLists.Add(AllSymbols);
			}
			if (sources.HasFlag(SaltSources.Emojis))
			{
				sourceLists.Add(Emojis);
			}
			if (sources.HasFlag(SaltSources.Latin1Supplement))
			{
				sourceLists.Add(Latin1Supplement);
			}

			char[] result = string.Concat(sourceLists).ToCharArray();
			SaltOptionCache[sources] = result;

			return result;
		}

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
			ApplySalt(selectedWords, options.Salt, options.SaltCharacterSources, options.WordSeparator, random);
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
					options.SaltCharacterSources == SaltSources.None)
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
					case WordCasingType.Random:
						char[] randomized = (from c in words[scan].ToCharArray()
											 select ((random.GetRandomUInt64() & 1) == 0
												 ? char.ToUpper(c)
												 : char.ToLower(c))
											)
											.ToArray();
						words[scan] = new string(randomized);
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
					|| (random.GetRandomUInt64() & 1) == 0;

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

		private static void ApplySalt(string[] words, SaltType salt, SaltSources sources, string separator, CryptoRandomStream random)
		{
			if (salt == SaltType.None)
			{
				return;
			}

			if (salt == SaltType.Sprinkle)
			{
				for (int scan = 0; scan < words.Length; ++scan)
				{
					bool skipWord = (random.GetRandomUInt64() & 1) == 0;
					if (skipWord)
					{
						continue;
					}

					int insertAt = random.Range(0, words[scan].Length - 1);

					words[scan] = words[scan].Insert(insertAt, GenerateSalt(sources, DefaultSaltMinimumLength, DefaultSaltMaximumLength, random));
				}

				return;
			}

			if (salt == SaltType.BetweenEach)
			{
				// after each word sans the last, insert a generated salt chunk.
				for (int scan = 0; scan < words.Length - 1; ++scan)
				{
					string generatedSalt = GenerateSalt(sources, DefaultSaltMinimumLength, DefaultSaltMaximumLength, random);
					words[scan] = $"{words[scan]}{separator}{generatedSalt}";
				}

				return;
			}


			string singleSalt = GenerateSalt(sources, DefaultSaltMinimumLength, DefaultSaltMaximumLength, random);

			switch (salt)
			{
				case SaltType.Prefix:
					words[0] = $"{singleSalt}{separator}{words[0]}";
					break;
				case SaltType.Suffix:
					words[words.Length - 1] = $"{words[words.Length - 1]}{separator}{singleSalt}";
					break;
				case SaltType.BetweenOne:
					if (words.Length <= 1)
					{
						// invalid case: can't insert between a single word.
						throw new ArgumentOutOfRangeException(nameof(words.Length), "Salt cannot be inserted between a single word.");
					}
					// get a random index *between* the first and last word (e.g.: 2nd to 2nd from last)
					int targetIndex = random.Range(1, words.Length - 2);
					// choose if the salt should go before or after the word at the selected index
					bool before = (random.GetRandomUInt64() & 1) == 0;

					words[targetIndex] = before
						? $"{singleSalt}{separator}{words[targetIndex]}"
						: $"{words[targetIndex]}{separator}{singleSalt}";

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
					AdvancedStrategy.SubstitueSalt => password.Replace(excludeStr, GenerateSalt(options.SaltCharacterSources, 1, 1, random)),
					_ => throw new InvalidOperationException($"Unhandled {nameof(AdvancedStrategy)}."),
				};
			}
		}

		public static string GenerateSalt(SaltSources sources, int minChars, int maxChars, CryptoRandomStream random)
		{
			char[] saltOptions = GetSaltOptions(sources);
			int length = random.Range(minChars, maxChars);
			char[] chars = (from i in Enumerable.Range(0, length)
							select saltOptions.SelectRandom(random))
							.ToArray();
			return new string(chars);
		}

		public static IEnumerable<string> GetWordList(WordLists lists)
		{
			var selectedWordlists = new List<string[]>();

			foreach (WordLists list in lists.GetFlags())
			{
				// none isn't a real list.
				if (list == WordLists.None)
				{
					continue;
				}

				if (LoadedLists.ContainsKey(list))
				{
					selectedWordlists.Add(LoadedLists[list]);
				}
				else
				{
					string[] loadedList = ReadEmbeddedResource($"{list}{WordListFileExtension}").ToArray();
					LoadedLists.Add(list, loadedList);

					// cache it for re-use.
					selectedWordlists.Add(loadedList);
				}
			}

			return selectedWordlists.SelectMany(s => s);
		}

		// via: https://stackoverflow.com/a/3314213/944605
		public static IEnumerable<string> ReadEmbeddedResource(string resourceName, Encoding encoding = null)
		{
			encoding ??= Encoding.UTF8;

			var assembly = Assembly.GetAssembly(typeof(Diceware));
			string resourcePath = resourceName;

			// Format: "{Namespace}.{Folder}.{Filename}.{Extension}"
			if (!resourceName.StartsWith(nameof(KeePassDiceware)))
			{
				string[] manifestResourceNames = assembly.GetManifestResourceNames();
				resourcePath = manifestResourceNames.Single(str => str.EndsWith(resourceName));
			}

			using Stream stream = assembly.GetManifestResourceStream(resourcePath);
			using var reader = new StreamReader(stream, encoding);
			string line;

			while ((line = reader.ReadLine()) != null)
			{
				yield return line;
			}
		}
	}
}
