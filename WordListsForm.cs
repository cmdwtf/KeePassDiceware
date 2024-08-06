using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using KeePass.UI;

namespace KeePassDiceware
{
	public partial class WordListsForm : Form
	{
		public List<WordList> Result { get; private set; } = null;

		public WordListsForm(List<WordList> wordLists)
		{
			InitializeComponent();
			PopulateWordLists(wordLists);
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			// TODO
			Result = (List<WordList>)WordListDataGridView.DataSource;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		internal void PopulateWordLists(List<WordList> wordLists)
		{
			WordListDataGridView.DataSource = wordLists;
		}

		private void RestoreDefaultsButton_Click(object sender, EventArgs e)
		{
			PopulateWordLists(WordList.Default);
		}
	}
}
