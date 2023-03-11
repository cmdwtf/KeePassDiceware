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
using System.ComponentModel;

namespace KeePassDiceware
{
	[Flags]
	public enum WordLists
	{
		[Browsable(false)]
		None = 0x0000_0000,

		// standard
		[Category("Standard")]
		Beale = 0x0000_0001,
		[Category("Standard"), Description("Diceware (Arnold G. Reinhold's Original)")]
		Diceware = 0x0000_0002,
		[Category("Standard"), Description("EFF Large")]
		EffLarge = 0x0000_0004,
		[Category("Standard"), Description("EFF Short (v1.0)")]
		EffShort1point0 = 0x0000_0008,
		[Category("Standard"), Description("EFF Short (v2.0 — More memorable, unique prefix)")]
		EffShort2point0 = 0x0000_0010,
		[Category("Standard"), Description("Google — U.S. English, no swears")]
		Google = 0x0000_0020,

		// Fandom
		[Category("Fandom"), Description("Game of Thrones (EFF Fandom)")]
		GameOfThrones = 0x0000_0100,
		[Category("Fandom"), Description("Harry Potter (EFF Fandom)")]
		HarryPotter = 0x0000_0200,
		[Category("Fandom"), Description("Star Trek (EFF Fandom)")]
		StarTrek = 0x0000_0400,
		[Category("Fandom"), Description("Star Wars (EFF Fandom)")]
		StarWars = 0x0000_0800,

		// Languages
		[Category("Languages")]
		Catalan = 0x0001_0000,
		[Category("Languages")]
		Dutch = 0x0002_0000,
		[Category("Languages")]
		Finnish = 0x0004_0000,
		[Category("Languages")]
		French = 0x0008_0000,
		[Category("Languages")]
		German = 0x0010_0000,
		[Category("Languages")]
		Icelandic = 0x0020_0000,
		[Category("Languages")]
		Italian = 0x0040_0000,
		[Category("Languages")]
		Japanese = 0x0080_0000,
		[Category("Languages")]
		Norwegian = 0x0100_0000,
		[Category("Languages")]
		Polish = 0x0200_0000,
		[Category("Languages")]
		Swedish = 0x0400_0000,
		[Category("Languages")]
		Spanish = 0x0800_0000,
	}
}
