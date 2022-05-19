using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using KeePass.UI;

namespace KeePassDiceware
{
	public partial class SaltSourcesForm : Form
	{
		public List<SaltSource> Result { get; private set; } = null;
		private BindingList<SaltSource> DataSource { get; set; } = null;

		internal bool DataValid
		{
			get => okButton.Enabled;
			set => okButton.Enabled = value;
		}

		private readonly List<DataGridViewCell> _pendingErrors = new();

		public SaltSourcesForm()
		{
			InitializeComponent();
			SaltSourceDataGridView.AutoGenerateColumns = false;
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

		private void OkButton_Click(object sender, EventArgs e)
		{
			Result = DataSource.ToList();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
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

		internal void PopulateSaltSources(List<SaltSource> saltSources)
		{
			_pendingErrors.Clear();

			DataSource = new(saltSources.ConvertAll(ss => ss.Clone() as SaltSource))
			{
				AllowNew = true,
				AllowRemove = true,
				AllowEdit = true,
			};

			DataSource.AddingNew += DataSource_AddingNew;
			DataSource.ListChanged += DataSource_ListChanged;

			SaltSourceDataGridView.DataSource = DataSource;

			BindingSource bindingSource = new(DataSource, null);
			SaltSourceDataGridView.DataSource = bindingSource;

			CheckValidation();
		}

		private void DataSource_ListChanged(object sender, ListChangedEventArgs e) => CheckValidation();

		private void DataSource_AddingNew(object sender, AddingNewEventArgs e)
		{
			SaltSource created = new()
			{
				Name = "New Source"
			};

			e.NewObject = created;
		}

		private void SaltSourceDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
		{
			// End of edition on each click on column of checkbox
			if (e.ColumnIndex == SourceEnabled.Index && e.RowIndex != -1)
			{
				SaltSourceDataGridView.EndEdit();
			}
		}

		private void SaltSourceDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// End of edition on each click on column of checkbox
			if (e.ColumnIndex == SourceEnabled.Index && e.RowIndex != -1)
			{
				SaltSourceDataGridView.EndEdit();
			}
		}

		private void SaltSourceDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
			{
				return;
			}

			CheckValidation();
		}

		private void CheckValidation()
		{
			int nameErrors = ValidateSourceNames();
			DataValid = nameErrors == 0;
		}


		private int ValidateSourceNames()
		{
			int errors = 0;

			foreach (DataGridViewRow row in SaltSourceDataGridView.Rows)
			{
				if (row is null)
				{
					continue;
				}

				if (row.DataBoundItem is not SaltSource ss)
				{
					continue;
				}

				if (DataSource.Count(oss => oss.Key == ss.Key) < 2)
				{
					row.ErrorText = string.Empty;
				}
				else
				{
					row.ErrorText = "This source's name is too similar to another source. Please make it more unique.";
					errors++;
				}
			}

			return errors;
		}
	}
}
