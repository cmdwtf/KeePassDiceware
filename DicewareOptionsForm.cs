using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

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

		private List<WordList> _wordLists = new();
		private readonly ListViewGroup[] _wordListCategories = Enum.GetNames(typeof(WordList.CategoryEnum))
			.Select(wl => new ListViewGroup(wl, wl)).ToArray();


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
			activeSaltSourcesListView.Columns.Add("Salt sources");

			_wordLists.Clear();
			activeWordListsListView.Columns.Add("Word List");
			activeWordListsListView.Groups.AddRange(_wordListCategories);
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
			_wordLists = new(Options.WordLists);
			UpdateSaltSourcesListView();
			UpdateWordListsListView();
		}

		private void UpdateSaltSourcesListView()
		{
			string[] sources = _saltSources.Where(ss => ss.Enabled)
				.Select(ss => $"{ss.Name} "
					+ (ss.MinimumAmount == ss.MaximumAmount
						? $"({ss.MinimumAmount})"
						: $"({ss.MinimumAmount}-{ss.MaximumAmount})")
				).ToArray();

			activeSaltSourcesListView.Items.Clear();
			foreach (string rowString in sources)
			{
				ListViewItem rowItem = new ListViewItem(rowString);
				activeSaltSourcesListView.Items.Add(rowItem);
			}
			activeSaltSourcesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void UpdateWordListsListView()
		{
			ListViewItem[] lists = _wordLists.Where(wl => wl.Enabled)
				.Select(wl => new ListViewItem(wl.Name,
				_wordListCategories.First(g => g.Name == Enum.GetName(typeof(WordList.CategoryEnum), wl.Category))))
				.ToArray();

			activeWordListsListView.Items.Clear();
			activeWordListsListView.Items.AddRange(lists);

			activeWordListsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
				WordLists = new(_wordLists)
			};
		}

		private void OkButton_Click(object sender, System.EventArgs e)
		{
			Options = GetOptions();

			if (Options.WordLists.Count() == 0)
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
			UpdateSaltSourcesListView();
		}

		private void EditWordListsButton_Click(object sender, EventArgs e)
		{
			WordListsForm wlf = new(_wordLists);

			if (wlf.ShowDialog(this) == DialogResult.Cancel)
			{
				return;
			}

			_wordLists = wlf.Result;
			UpdateWordListsListView();
		}

		private void ActiveSaltSourcesListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ListView)sender).SelectedItems.Clear();
		}

		private void WordListsListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ListView)sender).SelectedItems.Clear();
		}
	}
}
