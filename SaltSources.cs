using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace KeePassDiceware
{
	public class SaltSource : ICloneable
	{
		private const int DefaultSaltMinimumLength = 1;
		private const int DefaultSaltMaximumLength = 1;

		internal const string AllUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		internal const string AllLower = "abcdefghijklmnopqrstuvwxyz";
		internal const string AllDigits = "0123456789";

		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		internal const string AllSymbols = "+-=_@#$%^&;:,.<>/~\\[](){}?!|*'\"";
		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		internal const string Emojis = @"😀😃😄😁😆😅";
		// Copyright (C) 2014-2021 Mark McGuill. All rights reserved.
		internal const string Latin1Supplement =
				"\u00A1\u00A2\u00A3\u00A4\u00A5\u00A6\u00A7" +
				"\u00A8\u00A9\u00AA\u00AB\u00AC\u00AE\u00AF" +
				"\u00B0\u00B1\u00B2\u00B3\u00B4\u00B5\u00B6\u00B7" +
				"\u00B8\u00B9\u00BA\u00BB\u00BC\u00BD\u00BE\u00BF" +
				"\u00C0\u00C1\u00C2\u00C3\u00C4\u00C5\u00C6\u00C7" +
				"\u00C8\u00C9\u00CA\u00CB\u00CC\u00CD\u00CE\u00CF" +
				"\u00D0\u00D1\u00D2\u00D3\u00D4\u00D5\u00D6\u00D7" +
				"\u00D8\u00D9\u00DA\u00DB\u00DC\u00DD\u00DE\u00DF" +
				"\u00E0\u00E1\u00E2\u00E3\u00E4\u00E5\u00E6\u00E7" +
				"\u00E8\u00E9\u00EA\u00EB\u00EC\u00ED\u00EE\u00EF" +
				"\u00F0\u00F1\u00F2\u00F3\u00F4\u00F5\u00F6\u00F7" +
				"\u00F8\u00F9\u00FA\u00FB\u00FC\u00FD\u00FE\u00FF";

		private static bool? _runtimeCanHandleEmoji = null;
		internal static bool RuntimeCanHandleEmoji
		{
			get
			{
				if (_runtimeCanHandleEmoji.HasValue)
				{
					return _runtimeCanHandleEmoji.Value;
				}

				_runtimeCanHandleEmoji = TestRuntimeRenderSupport(Emojis);

				return _runtimeCanHandleEmoji.Value;
			}
		}

		private static readonly StringFormat RuntimeEmojiTestStringFormat = new()
		{
			Alignment = StringAlignment.Near, // typically provided from the listviewitem's column alignment
			LineAlignment = StringAlignment.Center,
			FormatFlags = StringFormatFlags.NoWrap,
			Trimming = StringTrimming.EllipsisCharacter
		};

		public string Name { get; set; }
		[Browsable(false)]
		public string Key => Name.Replace(" ", string.Empty);
		public string Pool { get; set; }
		public int MinimumAmount { get; set; }
		public int MaximumAmount { get; set; }
		public bool Enabled
		{
			get => (MinimumAmount > 0 || MaximumAmount > 0) && MaximumAmount >= MinimumAmount;
			set
			{
				if (Enabled == value)
				{
					return;
				}

				if (value)
				{
					MinimumAmount = DefaultSaltMinimumLength;
					MaximumAmount = DefaultSaltMaximumLength;
				}
				else
				{
					MinimumAmount = MaximumAmount = 0;
				}
			}
		}

		internal bool? _canRender = null;
		public bool CanRender
		{
			get
			{
				if (_canRender.HasValue)
				{
					return _canRender.Value;
				}

				_canRender = TestRuntimeRenderSupport(Pool);

				return _canRender.Value;
			}
		}

		public SaltSource(string name, string pool, bool enabled = true)
		{
			Name = name;
			Pool = pool;

			Enabled = enabled;
		}

		public SaltSource()
		{
			Enabled = true;
		}

		public void Enable() => Enabled = true;

		public void Disable() => Enabled = false;

		public char[] GetCharacterPool() => Pool.ToCharArray();

		public object Clone() => MemberwiseClone();

		/// <summary>
		/// A default set of salt sources.
		/// </summary>
		public static List<SaltSource> DefaultSources
		{
			get
			{
				List<SaltSource> sources =
				[
					new SaltSource("Uppercase", AllUpper),
					new SaltSource("Lowercase", AllLower),
					new SaltSource("Digits", AllDigits),
					new SaltSource("Symbols", AllSymbols),
					new SaltSource("Emoji", Emojis, enabled: false),
					new SaltSource("Latin 1 Supplement", Latin1Supplement),
				];

				return sources;
			}
		}

		/// <summary>
		/// As <see cref="DefaultSources"/>, but without the "Emoji" source.
		/// </summary>
		public static List<SaltSource> DefaultSourcesWithoutEmoji => DefaultSources.FindAll(s => s.Name != "Emoji");

		/// <summary>
		/// Attempts to render the given string with the graphics API.
		/// </summary>
		/// <param name="testString">The string to attempt to render</param>
		/// <returns>True if the render was successful, otherwise false</returns>
		internal static bool TestRuntimeRenderSupport(string testString)
		{
			try
			{
				// attempt to render the given string to a temporary bitmap.
				using Bitmap test = new(1, 1);
				using var g = Graphics.FromImage(test);
				g.DrawString(testString, SystemFonts.DefaultFont, Brushes.Black, 0, 0, RuntimeEmojiTestStringFormat);

				// call fill rectangle as this is where the exception was thrown in the example from:
				// https://github.com/mono/mono/issues/18079
				g.FillRectangle(Brushes.Black, 0, 0, 1, 1);
				return true;
			}
			catch
			{
				// if drawing fails for any reason, assume this runtime can't handle characters in that string
				return false;
			}
		}
	}
}
