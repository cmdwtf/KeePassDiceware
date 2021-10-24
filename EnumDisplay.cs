using System;

namespace KeePassDiceware
{
	internal class EnumDisplay<T> : IEquatable<T>, IEquatable<EnumDisplay<T>> where T : Enum
	{
		public T Value { get; set; }
		public string Description => Value.GetDescription();
		public string Category => Value.GetCategory();

		public EnumDisplay(T value)
		{
			Value = value;
		}

		public static implicit operator EnumDisplay<T>(T value)
		{
			var eld = new EnumDisplay<T>(value);
			return eld;
		}

		public static implicit operator T(EnumDisplay<T> other)
		{
			return other.Value;
		}

		public override string ToString() => Description;

		public bool Equals(T other) => other.Equals(Value);
		public bool Equals(EnumDisplay<T> other) => other.Value.Equals(Value);
	}

	internal static class EnumDisplay
	{
		public static EnumDisplay<T> Create<T>(T value) where T : Enum
			=> new EnumDisplay<T>(value);
	}
}
