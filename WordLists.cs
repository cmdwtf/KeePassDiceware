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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KeePassDiceware
{
	public class WordList : ICloneable
	{
		public enum CategoryEnum
		{
			Standard,
			Fandom,
			Languages,
			User
		}

		public WordList()
		{
			Enabled = false;
		}

		private WordList(string name, string path, bool enabled = false, CategoryEnum category = CategoryEnum.User, bool embeded = false)
		{
			this.Name = name;
			this.Path = path;
			this.Category = category;
			this.Enabled = enabled;
			this.Embeded = embeded;
		}

		public object Clone() => MemberwiseClone();

		public bool Enabled { get; set; }

		public string Name { get; set; }

		// Move to DataContractSerializer? https://stackoverflow.com/questions/802711/serializing-private-member-data
		//public string Path { get; private set; }
		public string Path { get; set; }

		//public CategoryEnum Category { get; private set; }
		public CategoryEnum Category { get; set; }

		//public bool Embeded { get; private set; } = false;
		[Browsable(false)]
		public bool Embeded { get; set; } = false;

		public override int GetHashCode()
		{
			if (Name == null || Path == null)
			{
				return 0;
			}
			return Name.GetHashCode() ^ Path.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is WordList other && other.Name == this.Name && other.Path == this.Path && other.Embeded == this.Embeded;
		}

	public static List<WordList> Default
		{
			get
			{
				List<WordList> defaultList = new();

				// Standard
				defaultList.Add(new WordList("Beale", "Beale.txt", false, CategoryEnum.Standard, true));
				defaultList.Add(new WordList("Diceware (Arnold G. Reinhold's Original)", "Diceware.txt", true, CategoryEnum.Standard, true));
				defaultList.Add(new WordList("EFF Large", "EffLarge.txt", true, CategoryEnum.Standard, true));
				defaultList.Add(new WordList("EFF Short (v1.0)", "EffShort1point0.txt", false, CategoryEnum.Standard, true));
				defaultList.Add(new WordList("EFF Short (v2.0 — More memorable, unique prefix)", "EffShort2point0.txt", false, CategoryEnum.Standard, true));
				defaultList.Add(new WordList("Google — U.S. English, no swears", "Google.txt", true, CategoryEnum.Standard, true));

				// Fandom
				defaultList.Add(new WordList("Game of Thrones (EFF Fandom)", "GameOfThrones.txt", false, CategoryEnum.Fandom, true));
				defaultList.Add(new WordList("Harry Potter (EFF Fandom)", "HarryPotter.txt", false, CategoryEnum.Fandom, true));
				defaultList.Add(new WordList("Star Trek (EFF Fandom)", "StarTrek.txt", false, CategoryEnum.Fandom, true));
				defaultList.Add(new WordList("Star Wars (EFF Fandom)", "StarWars.txt", false, CategoryEnum.Fandom, true));

				// Languages
				defaultList.Add(new WordList("Catalan", "Catalan.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Dutch", "Dutch.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Finnish", "Finnish.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("French", "French.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("German", "German.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Icelandic", "Icelandic.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Italian", "Italian.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Japanese", "Japanese.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Norwegian", "Norwegian.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Polish", "Polish.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Swedish", "Swedish.txt", false, CategoryEnum.Languages, true));
				defaultList.Add(new WordList("Spanish", "Spanish.txt", false, CategoryEnum.Languages, true));

				return defaultList;
			}
		}
	}

}
