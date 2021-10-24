using System;
using System.ComponentModel;

namespace KeePassDiceware
{
	[Flags]
	public enum L33tSpeakType
	{
		None = 0x00,

		[Browsable(false)]
		Basic = 0x01,
		[Browsable(false)]
		Pro = 0x02,
		[Browsable(false)]
		SomeWords = 0x10,
		[Browsable(false)]
		AllWords = 0x20,


		[Description("Basic (Some Words)")]
		BasicSomeWords = Basic | SomeWords,
		[Description("Basic (All Words)")]
		BasicAllWords = Basic | AllWords,
		[Description("Pro (Some Words)")]
		ProSomeWords = Pro | SomeWords,
		[Description("Pro (All Words)")]
		ProAllWords = Pro | AllWords,
	};
}
