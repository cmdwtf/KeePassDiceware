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

using System.ComponentModel;

namespace KeePassDiceware
{
	public enum WordCasingType
	{
		[Description("Do Not Change")]
		DoNotChange,
		Lowercase,
		Uppercase,
		[Description("Title Case")]
		TitleCase,
		[Description("Title Case First Word")]
		TitleCaseFirst,
		[Description("Title Case Random")]
		TitleCaseRandom,
		Random,
		[Description("Random Whole Words")]
		WholeWord,
	};
}
