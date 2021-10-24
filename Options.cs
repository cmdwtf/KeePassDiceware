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
using System.IO;
using System.Text;
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
		public SaltSources SaltCharacterSources { get; set; } = SaltSources.All & ~(SaltSources.Emojis);
		public WordLists WordLists { get; set; } =
			WordLists.Diceware
			| WordLists.EffLarge
			| WordLists.Google;

		public Options() { }

		internal static Options Deserialize(string serialized)
		{
			var xml = new XmlSerializer(typeof(Options));
			return xml.Deserialize(new StringReader(serialized)) as Options;
		}

		internal static bool TryDeserialize(string serialized, out Options result)
		{
			result = null;

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
