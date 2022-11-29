
using KeePassLib.Cryptography;

namespace KeePassDiceware
{
	internal static class Extensions
	{
		public static int Range(this CryptoRandomStream random, int minInclusive, int maxInclusive)
			// #todo: avoid modulo bias
			=> (int)(random.GetRandomUInt64() % (ulong)(maxInclusive - minInclusive + 1)) + minInclusive;

		public static T SelectRandom<T>(this T[] array, CryptoRandomStream random)
		{
			int choice = random.Range(0, array.Length - 1);
			return array[choice];
		}

		public static bool CoinToss(this CryptoRandomStream random) => (random.GetRandomUInt64() & 1) == 0;
	}
}
