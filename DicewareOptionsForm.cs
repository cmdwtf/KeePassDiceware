using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using KeePass.UI;

namespace KeePassDiceware
{
	public partial class DicewareOptionsForm : Form
	{
		private Options _options = new();
		public Options Options
		{
			get => _options;
			set
			{
				_options = value;
				PopulateInterface();
			}
		}

		private List<SaltSource> _saltSources = new();

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

			EnumDisplay<AdvancedStrategy>[] advancedStrat = EnumTools.GetDisplays<AdvancedStrategy>();
			advancedStrategyComboBox.Items.AddRange(advancedStrat);
			advancedStrategyComboBox.SelectedIndex = 0;

			EnumDisplay<SaltType>[] salts = EnumTools.GetDisplays<SaltType>();
			saltComboBox.Items.AddRange(salts);
			saltComboBox.SelectedIndex = 0;

			_saltSources.Clear();
			activeSaltSourcesLabel.Text = string.Empty;

			wordListsListView.Columns.Add("Word List");

			ListViewGroup[] wordListCategories = EnumTools.GetCategories<WordLists>()
				.Select(c => new ListViewGroup(c, c))
				.ToArray();

			wordListsListView.Groups.AddRange(wordListCategories);

			ListViewItem[] wordLists = EnumTools.GetDisplays<WordLists>()
				.Select(v =>
					new ListViewItem($"{v}", wordListCategories.First(c => c.Name == v.Category)))
				.ToArray();

			wordListsListView.Items.AddRange(wordLists);

			wordListsListView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
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
			advancedStrategyComboBox.SelectedIndex = advancedStrategyComboBox.FindStringExact(Options.AdvancedStrategy.GetDescription());
			l33tSpeakComboBox.SelectedIndex = l33tSpeakComboBox.FindStringExact(Options.L33tSpeak.GetDescription());
			saltComboBox.SelectedIndex = saltComboBox.FindStringExact(Options.Salt.GetDescription());
			_saltSources = new(Options.SaltSources);
			UpdateSaltSourcesLabel();
			UpdateListView(wordListsListView, Options.WordLists);
		}

		private void UpdateSaltSourcesLabel()
		{
			IEnumerable<string> sources = _saltSources.Where(ss => ss.Enabled)
				.Select(ss => $"{ss.Name} "
					+ (ss.MinimumAmount == ss.MaximumAmount
						? $"({ss.MinimumAmount})"
						: $"({ss.MinimumAmount}-{ss.MaximumAmount})")
					);

			activeSaltSourcesLabel.Text = string.Join(", ", sources);
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
				AdvancedStrategy = EnumTools.FromDescription<AdvancedStrategy>(advancedStrategyComboBox.SelectedItem.ToString()),
				Salt = EnumTools.FromDescription<SaltType>(saltComboBox.SelectedItem.ToString()),
				SaltSources = new(_saltSources),
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

		private void EditSaltSourcesButton_Click(object sender, EventArgs e)
		{
			SaltSourcesForm ssf = new();
			ssf.PopulateSaltSources(_saltSources);

			if (ssf.ShowDialog(this) == DialogResult.Cancel)
			{
				return;
			}

			_saltSources = ssf.Result;
			UpdateSaltSourcesLabel();
		}
	}
}
