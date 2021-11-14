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
	public enum SaltType
	{
		[Description("No salt")]
		None,
		[Description("Prepended to the passphrase")]
		Prefix,
		[Description("Sprinkle into words at random")]
		Sprinkle,
		[Description("Appended to the end of the passphrase")]
		Suffix,
		[Description("Between two words at random, once")]
		BetweenOne,
		[Description("Between each word")]
		BetweenEach,
	}
}
