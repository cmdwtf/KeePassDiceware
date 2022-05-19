/*
	KeePassDiceware Plugin
	Copyright (C) 2021-2022 cmd <https://github.com/cmdwtf>

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


using System;
using System.Diagnostics;
using System.Windows.Forms;

using KeePass.Plugins;
using KeePass.UI;

using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace KeePassDiceware
{
	public sealed class DicewarePwGenerator : CustomPwGenerator
	{
		public override PwUuid Uuid { get; } = new PwUuid(new byte[] {
			0xBA, 0x8E, 0x96, 0xF8, 0x66, 0xAE, 0x41, 0xEB,
			0x9E, 0xE3, 0x90, 0xC9, 0x9E, 0xAD, 0x86, 0x0B, });

		public override string Name => nameof(Diceware);

		public override bool SupportsOptions => true;

		public IPluginHost Host { get; private set; }

		private Options _options;

		private static string OptionsKey { get; } = typeof(Options).FullName;

		public DicewarePwGenerator(IPluginHost host)
		{
			Host = host;

			// load (legacy) global options, if there are any.
			LoadPluginOptions(null);
		}

		public override ProtectedString Generate(PwProfile profile, CryptoRandomStream random)
		{
			Debug.Assert(profile != null);
			string uuidString = Convert.ToBase64String(Uuid.UuidBytes, Base64FormattingOptions.None);
			Debug.Assert(profile.CustomAlgorithmUuid == uuidString);

			LoadPluginOptions(profile.CustomAlgorithmOptions);

			string result = Diceware.Generate(_options, profile, random);

			return new ProtectedString(false, result);
		}

		private void LoadPluginOptions(string optionalSerializedOptions)
		{
			// try to load options based on given serialized options
			if (!string.IsNullOrWhiteSpace(optionalSerializedOptions))
			{
				if (Options.TryDeserialize(optionalSerializedOptions, out _options))
				{
					_options ??= Options.Default();
					return;
				}
			}

			// no luck with profile-specific options, load 'global' ones
			// that may have been used with a previous version of the plugin.
			string optionsString = Host.CustomConfig.GetString(OptionsKey);
			if (Options.TryDeserialize(optionsString, out _options))
			{
				_options ??= Options.Default();
				return;
			}

			// failed to deserialize 'global' options, use new defaults.
			_options = Options.Default(); 
		}

		public override string GetOptions(string strCurrentOptions)
		{
			if (Host == null)
			{
				return string.Empty;
			}

			// load the options given to us by KeePass as current
			LoadPluginOptions(strCurrentOptions);

			using var dof = new DicewareOptionsForm();
			dof.Options = _options;

			if (dof.ShowDialog(GlobalWindowManager.TopWindow) == DialogResult.OK)
			{
				_options = dof.Options;
				return dof.Options.Serialize();
			}

			// user declined to save changes -- return original value.
			return strCurrentOptions;
		}
	}
}
