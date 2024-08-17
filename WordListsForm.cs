using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using KeePass.UI;

namespace KeePassDiceware
{
	/// <summary>
	/// Class <c>WordListsForms</c> represents a form that allows user to add, modify and remove dictionaries.
	/// </summary>
	public partial class WordListsForm : Form
	{
		/// <value>Property <c>Result</c> represents the state of list after closing of the form.</value>
		/// <remarks>
		/// Contains modifications even if the form was closed with <c>DialogResult.Cancel</c> status.
		/// In such cases the <c>Result</c> property should be considered invalid.
		/// </remarks>
		public List<WordList> Result { get; private set; } = null;

		/// <value>Property <c>_wordListsBindingList</c> is a binding list on top of <c>Result</c> used inside <c>WordListDataGridView</c>.</value>
		private BindingList<WordList> _wordListsBindingList;

		/// <summary>
		/// Creates a form with a initial wordlists contianed inside <paramref name="wordLists"/>.
		/// </summary>
		/// <param name="wordLists"> initial list of configured WordLists.</param>
		public WordListsForm(List<WordList> wordLists)
		{
			InitializeComponent();
			PopulateWordLists(wordLists);
		}

		/// <summary>
		/// Load handeler. Registers window inside Keepass <c>GlobalWindowManager</c>.
		/// </summary>
		/// <param name="EventArgs"> OnLoad event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			GlobalWindowManager.AddWindow(this);
		}

		/// <summary>
		/// Unload handeler. Deregisters window inside Keepass <c>GlobalWindowManager</c>.
		/// </summary>
		/// <param name="EventArgs"> OnClosed event arguments</param>
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			GlobalWindowManager.RemoveWindow(this);
		}

		/// <summary>
		/// Handles <c>OkButton</c> button press. Closes form with result <c>DialogResult.OK</c>.
		/// </summary>
		/// <param name="sender"> sender of click event</param>
		/// <param name="e"> eventargs of click event</param>
		private void OkButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Handles <c>CancelButton</c> button press. Closes form with result <c>DialogResult.Cancel</c>.
		/// </summary>
		/// <param name="sender"> sender of click event</param>
		/// <param name="e"> eventargs of click event</param>
		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>
		/// Populates <c>WordListDataGridView</c> with WordLists inside <paramref name="wordLists"/> parameter.
		/// </summary>
		/// <param name="wordLists"> word lists to render</param>
		internal void PopulateWordLists(List<WordList> wordLists)
		{
			Result = wordLists.ConvertAll(wl => (WordList)wl.Clone());

			_wordListsBindingList = new(Result);

			WordListDataGridView.DataSource = _wordListsBindingList;

			foreach (DataGridViewRow row in WordListDataGridView.Rows)
			{
				if (row.DataBoundItem is WordList wl)
				{
					if (wl.Embeded)
					{
						// Disable modifications of embeded list names.
						row.Cells["wordListName"].ReadOnly = true;
					}
				}

			}

		}

		/// <summary>
		/// Populates <c>WordListDataGridView</c> with default WordLists values.
		/// </summary>
		/// <param name="sender"> sender of click event</param>
		/// <param name="e"> eventargs of click event</param>
		private void RestoreDefaultsButton_Click(object sender, EventArgs e)
		{
			PopulateWordLists(WordList.Default);
		}

		/// <summary>
		/// Validates modified row in <c>WordListDataGridView</c>.
		/// </summary>
		/// <param name="sender"> sender of cell event</param>
		/// <param name="e"> eventargs of cell event</param>
		private void WordListDataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			DataGridViewRow row = WordListDataGridView.Rows[e.RowIndex];

			if (row.DataBoundItem is WordList wl)
			{
				row.ErrorText = string.Empty;
				e.Cancel = false;

				// Validate name
				if (Result.Count(cwl => cwl.Key == wl.Key) >= 2)
				{
					row.ErrorText += "This wordlists's name is too similar to another wordlists. Please make it more unique.";
					e.Cancel = true;
				}

				// Validate file
				if (wl.Path == null || wl.Path == "")
				{
					row.ErrorText += "This wordlists does not have a path to file set. Please configure the file location.";
					e.Cancel = true;
				}
				else if (!wl.Embeded && !File.Exists(wl.Path))
				{
					row.ErrorText += "This wordlists does not exist. Please set a correct path.";
					e.Cancel = true;
				}
				//else if (File.ReadLines(wl.Path).Count() < 50)
				//{
				//	row.ErrorText += "This wordlists contains less than 50 words. Please use a larger wordlist.";
				//	e.Cancel = true;
				//}
			}
		}

		/// <summary>
		/// Finishes validation of row in <c>WordListDataGridView</c>.
		/// </summary>
		/// <param name="sender"> sender of cell event</param>
		/// <param name="e"> eventargs of cell event</param>
		private void WordListDataGridView_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow row = WordListDataGridView.Rows[e.RowIndex];
			row.ErrorText = string.Empty;
		}

		/// <summary>
		/// Opens a File dialog to let user select a new wordlist.
		/// </summary>
		/// <returns>
		/// Path to the new wordlist. null if dialog was canceled.
		/// </returns>
		private string GetNewWordListPath()
		{
			var dialog = new OpenFileDialog();
			dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			dialog.Title = "Please select a new wordlist";
			dialog.CheckFileExists = true;
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				return dialog.FileName;
			}

			return null;
		}

		/// <summary>
		/// Checks if <paramref name="path"/> is already inside configured wordlists.
		/// If so a Error message box is opened.
		/// </summary>
		/// <param name="path"> path to be checked for duplicities.</param>
		/// <returns>
		/// true if <paramref name="path"/>] is already in configured wordlists. false otherwise.
		/// </returns>
		private bool IsDuplicateFile(string path)
		{
			if (Result.Count(cwl => cwl.Path == path) >= 1)
			{
				string existingName = Result.Find(cwl => cwl.Path == path).Name;
				MessageBox.Show(this, $"Same file is already configured as wordlist \"{existingName}\".",
	$"Duplicate wordlists", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Iterates on top of <paramref name="name"/> until a unique name is found.
		/// Every iteration it increasese a number behind name.
		/// </summary>
		/// <param name="name"> name to be made unique</param>
		/// <returns>
		/// "<paramref name="name"/>" if not duplicate. "<paramref name="name"/> (x)" where x is a positive int that is not duplicate.
		/// </returns>
		private string GetNonconflictingName(string name)
		{
			WordList wl = new WordList(name, "");

			if (Result.Count(cwl => cwl.Key == wl.Key) < 1)
			{
				return name;
			}

			uint counter = 0;
			do
			{
				counter++;

				wl.Name = name + $" ({counter})";
			} while (Result.Count(cwl => cwl.Key == wl.Key) >= 1);

			return wl.Name;
		}

		/// <summary>
		/// Adds a new WordLists value. Asks a user for a wordlist location.
		/// </summary>
		/// <param name="sender"> sender of click event</param>
		/// <param name="e"> eventargs of click event</param>
		private void AddNewButton_Click(object sender, EventArgs e)
		{
			string path = GetNewWordListPath();

			if (path == null || IsDuplicateFile(path))
			{
				return;
			}

			var fileName = System.IO.Path.GetFileNameWithoutExtension(path);

			WordList newList = new WordList(GetNonconflictingName(fileName), path);

			_wordListsBindingList.Add(newList);
		}

		/// <summary>
		/// Asks a user for a new wordlist location after double clicking on path.
		/// </summary>
		/// <param name="sender"> sender of click event</param>
		/// <param name="e"> eventargs of click event</param>
		private void WordListDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if ( e.RowIndex >= 0
				&& e.ColumnIndex >= 0
				&& WordListDataGridView.Columns[e.ColumnIndex].Name == "wordListPath"
				&& WordListDataGridView.Rows[e.RowIndex].DataBoundItem is WordList wl
				&& !wl.Embeded)
			{
				string newPath = GetNewWordListPath();
				if (newPath != null && !IsDuplicateFile(newPath))
				{
					wl.Path = newPath;
				}
			}
		}
	}
}
