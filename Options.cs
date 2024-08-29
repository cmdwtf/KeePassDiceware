/*
	KeePassDiceware Plugin
	Copyright (C) 2021 cmd <https://github.com/cmdwtf>

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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KeePassDiceware
{
	[Serializable]
	public class Options
	{
		public int WordCount { get; set; } = 4;
		public string WordSeparator { get; set; } = "-";
		public WordCasingType WordCasing { get; set; } = WordCasingType.TitleCase;
		public L33tSpeakType L33tSpeak { get; set; } = L33tSpeakType.None;
		public SaltType Salt { get; set; } = SaltType.None;
		public List<SaltSource> SaltSources { get; set; } = new();
		public List<WordList> WordLists { get; set; } = new();
		public AdvancedStrategy AdvancedStrategy { get; set; } = AdvancedStrategy.Drop;

		public Options() { }

		public static Options Default()
		{
			return new()
			{
				SaltSources = SaltSource.Default,
				WordLists = WordListEmbeded.Default,
			};
		}

		internal static Options Deserialize(string serialized)
		{
			using StringReader stream = new(serialized);
			using var reader = XmlReader.Create(stream);
			XmlSerializer xml = new(typeof(Options));
			xml.UnknownElement += Xml_UnknownElement;
			Options result = xml.Deserialize(reader) as Options;
			MigrateWordListsIfNeeded(serialized, ref result);
			return result;
		}

		/// <summary>
		/// Handles migration of WordList storage on versions <= 1.6.0. If a wordlists were set, it is replaced by the same configuration in the new system
		/// </summary>
		/// <param name="serialized">The serialized XML configuration</param>
		/// <param name="result">The resulting Options</param>
		private static void MigrateWordListsIfNeeded(string serialized, ref Options result)
		{
			using StringReader stream = new(serialized.Trim());
			using var reader = XmlReader.Create(stream);

			// migrate from <= v1.6.0 word lists
			reader.ReadToDescendant("WordLists");

			// They do not start as objects - it is simply a string stream
			if (reader.Read() && !reader.Value.StartsWith("<"))
			{
				// Start with a default wordlist with all disabled
				List<WordList> newWordLists = WordListEmbeded.Default;
				newWordLists.ForEach(wl => wl.Enabled = false);

				string[] oldWordListNames = reader.Value.Split(new char[] { ' ' });

				foreach (var currentWordListName in oldWordListNames)
				{
					int index = newWordLists.FindIndex(wl => ((WordListEmbeded)wl).GetBareFilename() == currentWordListName);

					// If unexpected value (e.g. nonexistent wordlist) is found -> discard all changes
					if (index == -1)
					{
						return;
					}

					newWordLists.ElementAt(index).Enabled = true;
				}

				result.WordLists = newWordLists;
			}
		}

		private static void Xml_UnknownElement(object sender, XmlElementEventArgs e)
		{
			if (e.ObjectBeingDeserialized is not Options opts)
			{
				return;
			}

			// handle legacy salt character source option
			if (e.Element.Name == "SaltCharacterSources")
			{
				string[] saltSourceNames = e.Element.InnerText.Split(new char[] { ' ' });

				// start from the defaults
				opts.SaltSources = SaltSource.Default;

				foreach (SaltSource ss in opts.SaltSources)
				{
					if (saltSourceNames.Any(ssn => string.Compare(ssn, ss.Key) == 0) == false)
					{
						ss.Disable();
					}
				}
			}

			e.ToString();
		}

		internal static bool TryDeserialize(string serialized, out Options result)
		{
			result = null;

			if (string.IsNullOrWhiteSpace(serialized))
			{
				return false;
			}

			try
			{
				result = Deserialize(serialized);
				return true;
			}
			catch
			{
				return false;
			}
		}

		internal string Serialize()
		{
			var xml = new XmlSerializer(typeof(Options));
			var sb = new StringBuilder();
			var outputStream = new StringWriter(sb);
			xml.Serialize(outputStream, this);
			return sb.ToString();
		}
	}
}
