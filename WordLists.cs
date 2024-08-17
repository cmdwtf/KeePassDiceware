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
	/// <summary>
	/// Class <c>WordList</c> represents a wordlist/dictionary.
	/// </summary>
	public class WordList : ICloneable
	{
		/// <summary>
		/// Enum <c>CategoryEnum</c> contains possible categories of wordlists.
		/// </summary>
		public enum CategoryEnum
		{
			Standard,
			Fandom,
			Languages,
			User
		}

		/// <summary>
		/// Creates a default wordlist.
		/// </summary>
		public WordList()
		{
		}

		/// <summary>
		/// Creates enabled wordlist with <paramref name="name"/> and <paramref name="path"/>.
		/// </summary>
		/// <param name="name"> name of wordlist.</param>
		/// <param name="path"> path to wordlist</param>
		public WordList(string name, string path)
		{
			this.Name = name;
			this.Path = path;
			this.Enabled = true;
		}

		/// <summary>
		/// Creates wordlist with all setters.
		/// </summary>
		/// <param name="name"> name of wordlist.</param>
		/// <param name="path"> path to wordlist</param>
		/// <param name="enabled"> true of wordlist is enabled (default: false)</param>
		/// <param name="category"> category of wordlist (default: User)</param>
		/// <param name="embeded"> true of wordlist is embeded (default: false)</param>
		private WordList(string name, string path, bool enabled = false, CategoryEnum category = CategoryEnum.User, bool embeded = false)
		{
			this.Name = name;
			this.Path = path;
			this.Category = category;
			this.Enabled = enabled;
			this.Embeded = embeded;
		}

		/// <summary>
		/// Creates a deep copy of wordlist
		/// </summary>
		/// <returns>Deep of wordlist</returns>
		public object Clone() => MemberwiseClone();

		/// <summary>
		/// true if wordlist is enabled and should be used to generate passwords.
		/// </summary>
		public bool Enabled { get; set; } = false;

		/// <summary>
		/// Name of wordlist
		/// </summary>
		public string Name { get; set; } = "New Wordlist";

		/// <summary>
		/// Key of wordlist. Is a simplified name.
		/// </summary>
		public string Key => Name.Replace(" ", string.Empty);

		// Move to DataContractSerializer? https://stackoverflow.com/questions/802711/serializing-private-member-data
		//public string Path { get; private set; }
		/// <summary>
		/// Path to wordlist
		/// </summary>
		private string _path;

		/// <summary>
		/// Path to wordlist. For embeded wordlist it consists of "(embeded)\Filename"
		/// </summary>
		public string Path {
			get => Embeded ? "(embeded)\\" + _path : _path;
			set => _path = value;
		}


		//public CategoryEnum Category { get; private set; }
		/// <summary>
		/// Category of wordlist.
		/// </summary>
		public CategoryEnum Category { get; set; } = CategoryEnum.User;

		//public bool Embeded { get; private set; } = false;
		/// <summary>
		/// true if wordlist is embeded and compiled into plugin.
		/// </summary>
		[Browsable(false)]
		public bool Embeded { get; set; } = false;

		/// <summary>
		/// Calculates a hash of current wordlist.
		/// </summary>
		/// <returns>Hash of wordlist consisting of name and path.</returns>
		public override int GetHashCode()
		{
			if (Name == null || Path == null)
			{
				return 0;
			}
			return Name.GetHashCode() ^ Path.GetHashCode();
		}

		/// <summary>
		/// Checks if two wordlists are equal.
		/// </summary>
		/// <param name="obj"> Second wordlist to compare to.</param>
		/// <returns>true if both are wordlists and name, path and embeded match.</returns>
		public override bool Equals(object obj)
		{
			return obj is WordList other && other.Name == this.Name && other.Path == this.Path && other.Embeded == this.Embeded;
		}

		/// <summary>
		/// Default wordlist.
		/// </summary>
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
