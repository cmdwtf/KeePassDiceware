using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace KeePassDiceware
{
	internal static class EnumTools
	{
		public static string GetDescription(this Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			return fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any()
				? attributes.First().Description
				: value.ToString();
		}

		public static T FromDescription<T>(string description) where T : Enum
		{
			Type enumType = typeof(T);
			Array allValues = Enum.GetValues(enumType);

			foreach (T value in allValues)
			{
				string valueString = value.ToString();
				FieldInfo fi = enumType.GetField(valueString);
				bool match = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any()
					? attributes.First().Description == description
					: valueString == description;

				if (match)
				{
					return value;
				}
			}

			throw new ArgumentException($"{nameof(description)} must be in the given enum type. ({typeof(T).Name})");
		}

		public static string GetCategory(this Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			return fi.GetCustomAttributes(typeof(CategoryAttribute), false) is CategoryAttribute[] attributes && attributes.Any()
				? attributes.First().Category
				: string.Empty;
		}

		public static EnumDisplay<T> GetDisplay<T>(this T value) where T : Enum
			=> EnumDisplay.Create(value);

		public static IEnumerable<T> GetBrowsableValues<T>() where T : Enum
		{
			Type enumType = typeof(T);
			Array allValues = Enum.GetValues(enumType);

			foreach (T value in allValues)
			{
				FieldInfo fi = enumType.GetField(value.ToString());
				bool browsable = fi.GetCustomAttributes(typeof(BrowsableAttribute), false) is BrowsableAttribute[] attributes && attributes.Any()
					? attributes.First().Browsable
					: true;

				if (browsable)
				{
					yield return value;
				}
			}
		}

		public static IList<string> GetCategories<T>() where T : Enum
		{
			Type enumType = typeof(T);
			Array allValues = Enum.GetValues(enumType);
			var result = new List<string>();

			foreach (T value in allValues)
			{
				FieldInfo fi = enumType.GetField(value.ToString());
				string category = fi.GetCustomAttributes(typeof(CategoryAttribute), false) is CategoryAttribute[] attributes && attributes.Any()
					? attributes.First().Category
					: null;

				if (string.IsNullOrWhiteSpace(category) == false
					&& result.Contains(category) == false)
				{
					result.Add(category);
				}
			}

			return result;
		}

		public static EnumDisplay<T>[] GetDisplays<T>() where T : Enum
		{
			return GetBrowsableValues<T>()
				.Select(v => v.GetDisplay())
				.ToArray();
		}

		// via: https://stackoverflow.com/a/4171296
		public static IEnumerable<Enum> GetFlags(this Enum flagsEnum)
		{
			foreach (Enum value in Enum.GetValues(flagsEnum.GetType()))
			{
				if (flagsEnum.HasFlag(value))
				{
					yield return value;
				}
			}
		}
	}
}
