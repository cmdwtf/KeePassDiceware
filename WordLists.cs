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
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace KeePassDiceware
{
	/// <summary>
	/// Abstract class <c>WordList</c> represents a wordlist/dictionary.
	/// </summary>
	[XmlInclude(typeof(WordListCustom))]
	[XmlInclude(typeof(WordListEmbeded))]
	public abstract class WordList : ICloneable
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
		/// Creates wordlist with all setters.
		/// </summary>
		/// <param name="name"> name of wordlist.</param>
		/// <param name="path"> path to wordlist</param>
		/// <param name="enabled"> true of wordlist is enabled (default: false)</param>
		/// <param name="category"> category of wordlist (default: User)</param>
		protected WordList(string name, string path, bool enabled, CategoryEnum category)
		{
			this.Name = name;
			this.Path = path;
			this.Enabled = enabled;
			this.Category = category;
		}

		/// <summary>
		/// Creates a deep copy of wordlist
		/// </summary>
		/// <returns>Deep of wordlist</returns>
		public object Clone() => MemberwiseClone();

		/// <summary>
		/// true if wordlist is enabled and should be used to generate passwords.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Name of wordlist
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Key of wordlist. Is a simplified name.
		/// </summary>
		public string Key => Name.Replace(" ", string.Empty);

		/// <summary>
		/// Internal storage of path to wordlist.
		/// </summary>
		private string _path;

		/// <summary>
		/// Path to wordlist
		/// </summary>
		public string Path
		{
			get => _path;
			set
			{
				// reset cache if path changes
				_words = null;
				_path = value;
			}
		}

		/// <summary>
		/// Category of wordlist.
		/// </summary>
		public CategoryEnum Category { get; set; }

		//public bool Embeded { get; private set; } = false;
		/// <summary>
		/// true if wordlist is read only.
		/// </summary>
		[Browsable(false)]
		public virtual bool ReadOnly { get; } = false;

		/// <summary>
		/// A cache of words in wordset
		/// </summary>
		[Browsable(false)]
		protected HashSet<string> _words = null;

		/// <summary>
		/// Checks that a valid wordlist is configured.
		/// </summary>
		[Browsable(false)]
		public abstract bool Valid { get; }

		/// <summary>
		/// Gets all words in wordlist
		/// </summary>
		/// <returns><c>HashSet</c> of all words in list</returns>
		public HashSet<string> Get()
		{
			if (_words == null)
			{
				CacheWordList();
			}

			return _words;
		}

		/// <summary>
		/// Caches a wordlist into <c>_words</c> property.
		/// </summary>
		protected abstract void CacheWordList();
	}

	public class WordListEmbeded : WordList
	{
		//public bool Embeded { get; private set; } = false;
		/// <summary>
		/// true if wordlist is read only.
		/// </summary>
		[Browsable(false)]
		public override bool ReadOnly { get; } = true;

		/// <summary>
		/// A virtual path for embeded resources shown to user.
		/// </summary>
		private static readonly string VirtualPath = "(embeded)\\";

		/// <summary>
		/// Checks that a valid wordlist is configured.
		/// </summary>
		[Browsable(false)]
		public override bool Valid { get => true; }

		/// <summary>
		/// Creates a embeded wordlist.
		/// </summary>
		public WordListEmbeded() : base()
		{
		}

		/// <summary>
		/// Creates wordlist with all setters.
		/// </summary>
		/// <param name="name"> name of wordlist.</param>
		/// <param name="path"> path to wordlist</param>
		/// <param name="enabled"> true of wordlist is enabled (default: false)</param>
		/// <param name="category"> category of wordlist (default: User)</param>
		public WordListEmbeded(string name, string path, bool enabled = false, CategoryEnum category = CategoryEnum.Standard) : base(name, VirtualPath + path, enabled, category)
		{
		}

		/// <summary>
		/// Caches a wordlist into <c>_words</c> property. Based on <see cref="https://stackoverflow.com/a/3314213/944605"/>
		/// </summary>
		protected override void CacheWordList()
		{
			_words = new HashSet<string>();

			var assembly = Assembly.GetAssembly(typeof(Diceware));
			string resourceName = Path;

			if (resourceName.StartsWith(VirtualPath))
			{
				resourceName = resourceName.Remove(0, VirtualPath.Length);
			}

			// Format: "{Namespace}.{Folder}.{Filename}.{Extension}"
			if (!resourceName.StartsWith(nameof(KeePassDiceware)))
			{
				string[] manifestResourceNames = assembly.GetManifestResourceNames();
				resourceName = manifestResourceNames.Single(str => str.EndsWith(resourceName));
			}

			using Stream stream = assembly.GetManifestResourceStream(resourceName);
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
			{
				while (!reader.EndOfStream)
				{
					_words.Add(reader.ReadLine());
				}
			}
		}

		/// <summary>
		/// Default embeded wordlist.
		/// </summary>
		public static List<WordList> Default
		{
			get
			{
				List<WordList> defaultList = new();

				// Standard
				defaultList.Add(new WordListEmbeded("Beale", "Beale.txt", false, CategoryEnum.Standard));
				defaultList.Add(new WordListEmbeded("Diceware (Arnold G. Reinhold's Original)", "Diceware.txt", true, CategoryEnum.Standard));
				defaultList.Add(new WordListEmbeded("EFF Large", "EffLarge.txt", true, CategoryEnum.Standard));
				defaultList.Add(new WordListEmbeded("EFF Short (v1.0)", "EffShort1point0.txt", false, CategoryEnum.Standard));
				defaultList.Add(new WordListEmbeded("EFF Short (v2.0 — More memorable, unique prefix)", "EffShort2point0.txt", false, CategoryEnum.Standard));
				defaultList.Add(new WordListEmbeded("Google — U.S. English, no swears", "Google.txt", true, CategoryEnum.Standard));

				// Fandom
				defaultList.Add(new WordListEmbeded("Game of Thrones (EFF Fandom)", "GameOfThrones.txt", false, CategoryEnum.Fandom));
				defaultList.Add(new WordListEmbeded("Harry Potter (EFF Fandom)", "HarryPotter.txt", false, CategoryEnum.Fandom));
				defaultList.Add(new WordListEmbeded("Star Trek (EFF Fandom)", "StarTrek.txt", false, CategoryEnum.Fandom));
				defaultList.Add(new WordListEmbeded("Star Wars (EFF Fandom)", "StarWars.txt", false, CategoryEnum.Fandom));

				// Languages
				defaultList.Add(new WordListEmbeded("Catalan", "Catalan.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Dutch", "Dutch.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Finnish", "Finnish.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("French", "French.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("German", "German.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Icelandic", "Icelandic.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Italian", "Italian.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Japanese", "Japanese.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Norwegian", "Norwegian.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Polish", "Polish.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Swedish", "Swedish.txt", false, CategoryEnum.Languages));
				defaultList.Add(new WordListEmbeded("Spanish", "Spanish.txt", false, CategoryEnum.Languages));

				return defaultList;
			}
		}
	}

	public class WordListCustom : WordList
	{
		/// <summary>
		/// Checks that a valid wordlist is configured.
		/// </summary>
		[Browsable(false)]
		public override bool Valid { get => File.Exists(Path); }

		/// <summary>
		/// Creates a embeded wordlist.
		/// </summary>
		public WordListCustom() : base()
		{
		}

		/// <summary>
		/// Creates wordlist with all setters.
		/// </summary>
		/// <param name="name"> name of wordlist.</param>
		/// <param name="path"> path to wordlist</param>
		/// <param name="enabled"> true of wordlist is enabled (default: false)</param>
		/// <param name="category"> category of wordlist (default: User)</param>
		public WordListCustom(string name, string path, bool enabled = true, CategoryEnum category = CategoryEnum.User) : base(name, path, enabled, category)
		{
		}

		/// <summary>
		/// Caches a wordlist into <c>_words</c> property.
		/// </summary>
		protected override void CacheWordList()
		{
			_words = new HashSet<string>();

			using (StreamReader reader = new StreamReader(Path, Encoding.UTF8))
			{
				while (!reader.EndOfStream)
				{
					_words.Add(reader.ReadLine());
				}
			}
		}
	}
}
