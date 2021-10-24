using System;
using System.ComponentModel;

namespace KeePassDiceware
{
	[Flags]
	public enum SaltSources
	{
		[Browsable(false)]
		None = 0x0000_0000,
		Uppercase = 0x0000_0001,
		Lowercase = 0x0000_0002,
		Digits = 0x0000_0004,
		Symbols = 0x0000_0008,
		Emojis = 0x0000_0010,
		Latin1Supplement = 0x0000_0020,

		[Browsable(false)]
		All = Uppercase | Lowercase | Digits | Symbols | Emojis | Latin1Supplement,
	}
}
