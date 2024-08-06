
using System;

using KeePassLib.Cryptography;

namespace KeePassDiceware
{
	internal static class Extensions
	{
		/// <summary>
		/// Generates a pseudorandom value in range [0, <paramref name="maxInclusive"/>] (both ends are inclusive).
		/// </summary>
		/// <param name="maxInclusive"> maximal value (inclusive) of random number.</param>
		/// <returns>
		/// Pseudorandom value from [0, <paramref name="maxInclusive"/>] range.
		/// </returns>
		public static ulong AtMost(this CryptoRandomStream random, ulong maxInclusive)
		{
			const ulong RANDOM_MAX = ulong.MaxValue; // GetRandomUInt64 max return value
			if (maxInclusive == RANDOM_MAX)
			{
				return random.GetRandomUInt64();
			}

			ulong modulus = maxInclusive + 1;
			ulong ceil = RANDOM_MAX - (RANDOM_MAX % modulus);

			ulong generated;
			do
			{
				generated = random.GetRandomUInt64();
			} while (generated >= ceil);

			return generated % modulus;
		}

		/// <summary>
		/// Generates a pseudorandom value in range [0, <paramref name="maxInclusive"/>] (both ends are inclusive).
		/// </summary>
		/// <param name="maxInclusive"> maximal value (inclusive) of random number.</param>
		/// <returns>
		/// Pseudorandom value from [0, <paramref name="maxInclusive"/>] range.
		/// </returns>
		public static int AtMost(this CryptoRandomStream random, int maxInclusive)
		{
			if (maxInclusive < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxInclusive), $"Must be positive");
			}

			ulong val = random.AtMost(Convert.ToUInt64(maxInclusive));

			return checked((int)val); // maxInclusive <= int.MaxValue -> wont throw
		}

		/// <summary>
		/// Generates a pseudorandom value in range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>] (both ends are inclusive).
		/// </summary>
		/// <param name="minInclusive"> minimal value (inclusive) of random number.</param>
		/// <param name="maxInclusive"> maximal value (inclusive) of random number.</param>
		/// <returns>
		/// Pseudorandom value from [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>] range.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when the <paramref name="minInclusive"/> is larger than <paramref name="maxInclusive"/>.
		/// </exception>
		public static int Range(this CryptoRandomStream random, int minInclusive, int maxInclusive)
		{
			if (minInclusive > maxInclusive)
			{
				throw new ArgumentOutOfRangeException(nameof(minInclusive), $"Must be smaller or equal to {nameof(maxInclusive)}");
			}

			int randomValue = AtMost(random, maxInclusive - minInclusive);
			
			return randomValue + minInclusive;
		}

		/// <summary>
		/// Selects a pseudorandom element from <paramref name="array"/>.
		/// </summary>
		/// <param name="array"> array from which to select the random element.</param>
		/// <returns>
		/// A pseudorandom element from <paramref name="array"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Thrown when <paramref name="array"/> is empty.
		/// </exception>
		public static T SelectRandom<T>(this T[] array, CryptoRandomStream random)
		{
			if (array.Length <= 0)
			{
				throw new ArgumentException(nameof(array), $"Unable to select a random member of empty object.");
			}

			int choice = random.AtMost(array.Length - 1);

			return array[choice];
		}

		/// <summary>
		/// Return TRUE with 50% probability. Simulates a perfect coin toss.
		/// </summary>
		/// <returns>
		/// TRUE / FALSE with equal probabilities.
		/// </returns>
		public static bool CoinToss(this CryptoRandomStream random) => (random.GetRandomBytes(1)[0] & 1) == 0;
	}
}
