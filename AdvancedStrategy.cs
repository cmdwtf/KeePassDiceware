using System.ComponentModel;

namespace KeePassDiceware
{
	public enum AdvancedStrategy
	{
		[Description("Remove excluded characters")]
		Drop,
		[Description("Replace excluded characters with 'Word Separator'")]
		SubstitueWordSeparator,
		[Description("Replace excluded characters with random from salt sources")]
		SubstitueSalt,
	}
}
