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


using KeePass.Plugins;

namespace KeePassDiceware
{
	public sealed class KeePassDicewareExt : Plugin
	{
		private IPluginHost _host = null;
		private DicewarePwGenerator _gen = null;

		public override bool Initialize(IPluginHost host)
		{
			if (host == null)
			{
				return false;
			}

			_host = host;

			_gen = new DicewarePwGenerator(_host);
			_host.PwGeneratorPool.Add(_gen);

			return true;
		}

		public override void Terminate()
		{
			if (_host != null)
			{
				_host.PwGeneratorPool.Remove(_gen.Uuid);
				_gen = null;
				_host = null;
			}
		}
	}
}
