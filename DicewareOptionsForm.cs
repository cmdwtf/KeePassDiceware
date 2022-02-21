using System;
using System.Linq;
using System.Windows.Forms;

using KeePass.UI;

namespace KeePassDiceware
{
	public partial class DicewareOptionsForm : Form
	{
		private Options _options = new Options();
		public Options Options
		{
			get => _options;
			set
			{
				_options = value;
				PopulateInterface();
			}
		}

		public Func<Options, string> GenerateTest { get; internal set; }

		public DicewareOptionsForm()
		{
			InitializeComponent();

			InitializeEnumOptions();
		}

		private void InitializeEnumOptions()
		{
			EnumDisplay<WordCasingType>[] casings = EnumTools.GetDisplays<WordCasingType>();
			wordCasingComboBox.Items.AddRange(casings);
			wordCasingComboBox.SelectedIndex = 0;

			EnumDisplay<L33tSpeakType>[] l33tSpeak = EnumTools.GetDisplays<L33tSpeakType>();
			l33tSpeakComboBox.Items.AddRange(l33tSpeak);
			l33tSpeakComboBox.SelectedIndex = 0;

			EnumDisplay<SaltType>[] salts = EnumTools.GetDisplays<SaltType>();
			saltComboBox.Items.AddRange(salts);
			saltComboBox.SelectedIndex = 0;

			saltSourcesListView.Columns.Add("Salt Source", -1);

			ListViewItem[] saltSources = EnumTools.GetDisplays<SaltSources>()
				.Select(v => new ListViewItem($"{v}"))
				.ToArray();
			saltSourcesListView.Items.AddRange(saltSources);

			wordListsListView.Columns.Add("Word List", -1);

			ListViewGroup[] wordListCategories = EnumTools.GetCategories<WordLists>()
				.Select(c => new ListViewGroup(c, c))
				.ToArray();

			wordListsListView.Groups.AddRange(wordListCategories);

			ListViewItem[] wordLists = EnumTools.GetDisplays<WordLists>()
				.Select(v =>
					new ListViewItem($"{v}", wordListCategories.First(c => c.Name == v.Category)))
				.ToArray();

			wordListsListView.Items.AddRange(wordLists);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			GlobalWindowManager.RemoveWindow(this);
		}

		private void PopulateInterface()
		{
			wordCountNumericUpDown.Value = Options.WordCount;
			wordSeparatorPromptedTextBox.Text = Options.WordSeparator;
			wordCasingComboBox.SelectedIndex = wordCasingComboBox.FindStringExact(Options.WordCasing.GetDescription());
			l33tSpeakComboBox.SelectedIndex = l33tSpeakComboBox.FindStringExact(Options.L33tSpeak.GetDescription());
			saltComboBox.SelectedIndex = saltComboBox.FindStringExact(Options.Salt.GetDescription());
			UpdateListView(saltSourcesListView, Options.SaltCharacterSources);
			UpdateListView(wordListsListView, Options.WordLists);
		}

		private void UpdateListView<T>(ListView view, T flags) where T : Enum
		{
			foreach (ListViewItem i in view.Items)
			{
				T flag = EnumTools.FromDescription<T>(i.Text);
				i.Checked = flags.HasFlag(flag);
			}

			view.Refresh();
		}

		private Options GetOptions()
		{
			return new Options
			{
				WordCount = (int)wordCountNumericUpDown.Value,
				WordSeparator = wordSeparatorPromptedTextBox.Text,
				WordCasing = EnumTools.FromDescription<WordCasingType>(wordCasingComboBox.SelectedItem.ToString()),
				L33tSpeak = EnumTools.FromDescription<L33tSpeakType>(l33tSpeakComboBox.SelectedItem.ToString()),
				Salt = EnumTools.FromDescription<SaltType>(saltComboBox.SelectedItem.ToString()),
				SaltCharacterSources = GetListViewFlags<SaltSources>(saltSourcesListView),
				WordLists = GetListViewFlags<WordLists>(wordListsListView)
			};
		}

		private T GetListViewFlags<T>(ListView view) where T : Enum
		{
			int result = 0;
			foreach (ListViewItem i in view.Items)
			{
				T flagEnum = EnumTools.FromDescription<T>(i.Text);
				int flag = (int)Convert.ChangeType(flagEnum, typeof(int));

				if (i.Checked)
				{
					result |= flag;
				}
			}

			return (T)Enum.ToObject(typeof(T), result);
		}

		private void OkButton_Click(object sender, System.EventArgs e)
		{
			Options = GetOptions();

			if (Options.WordLists == WordLists.None)
			{
				MessageBox.Show(this, $"No word lists were selected. This will prevent the plugin from generating passwords. Please select at least one list.", "Options Validation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
				DialogResult = DialogResult.Cancel;
				return;
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButton_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void LinkLabel_LinkClicked_OpenTagAsLink(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (sender is LinkLabel ll && ll.Tag is string url)
			{
				try
				{
					System.Diagnostics.Process.Start($"{url}?utm_source={nameof(KeePassDiceware)}");
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, $"Failed to open URL: {url}\n\n{ex.Message}", "Link Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
