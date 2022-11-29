using System.Windows.Forms;

namespace KeePassDiceware
{
	public partial class SaltSourcesForm
	{
		private void InitializeComponent()
		{
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.SaltSourceDataGridView = new System.Windows.Forms.DataGridView();
			this.SourceEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.SourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SourceMinimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SourceMaximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SourceCharacterSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RestoreDefaultsButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.SaltSourceDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(493, 296);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(574, 296);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// SaltSourceDataGridView
			// 
			this.SaltSourceDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SaltSourceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.SaltSourceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SourceEnabled,
            this.SourceName,
            this.SourceMinimum,
            this.SourceMaximum,
            this.SourceCharacterSet});
			this.SaltSourceDataGridView.Location = new System.Drawing.Point(12, 12);
			this.SaltSourceDataGridView.Name = "SaltSourceDataGridView";
			this.SaltSourceDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.SaltSourceDataGridView.Size = new System.Drawing.Size(637, 278);
			this.SaltSourceDataGridView.TabIndex = 3;
			this.SaltSourceDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SaltSourceDataGridView_CellDoubleClick);
			this.SaltSourceDataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SaltSourceDataGridView_CellMouseUp);
			this.SaltSourceDataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.SaltSourceDataGridView_CellValidated);
			// 
			// SourceEnabled
			// 
			this.SourceEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.SourceEnabled.DataPropertyName = "Enabled";
			this.SourceEnabled.HeaderText = "Enabled";
			this.SourceEnabled.Name = "SourceEnabled";
			this.SourceEnabled.Width = 52;
			// 
			// SourceName
			// 
			this.SourceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.SourceName.DataPropertyName = "Name";
			this.SourceName.FillWeight = 50F;
			this.SourceName.HeaderText = "Name";
			this.SourceName.Name = "SourceName";
			// 
			// SourceMinimum
			// 
			this.SourceMinimum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.SourceMinimum.DataPropertyName = "MinimumAmount";
			this.SourceMinimum.FillWeight = 1F;
			this.SourceMinimum.HeaderText = "Minimum";
			this.SourceMinimum.Name = "SourceMinimum";
			this.SourceMinimum.Width = 73;
			// 
			// SourceMaximum
			// 
			this.SourceMaximum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.SourceMaximum.DataPropertyName = "MaximumAmount";
			this.SourceMaximum.FillWeight = 1F;
			this.SourceMaximum.HeaderText = "Maximum";
			this.SourceMaximum.Name = "SourceMaximum";
			this.SourceMaximum.Width = 76;
			// 
			// SourceCharacterSet
			// 
			this.SourceCharacterSet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.SourceCharacterSet.DataPropertyName = "Pool";
			this.SourceCharacterSet.HeaderText = "Characters in Source";
			this.SourceCharacterSet.Name = "SourceCharacterSet";
			// 
			// RestoreDefaultsButton
			// 
			this.RestoreDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RestoreDefaultsButton.Location = new System.Drawing.Point(12, 296);
			this.RestoreDefaultsButton.Name = "RestoreDefaultsButton";
			this.RestoreDefaultsButton.Size = new System.Drawing.Size(141, 23);
			this.RestoreDefaultsButton.TabIndex = 4;
			this.RestoreDefaultsButton.Text = "Restore &Defaults";
			this.RestoreDefaultsButton.UseVisualStyleBackColor = true;
			this.RestoreDefaultsButton.Click += new System.EventHandler(this.RestoreDefaultsButton_Click);
			// 
			// SaltSourcesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(661, 331);
			this.Controls.Add(this.RestoreDefaultsButton);
			this.Controls.Add(this.SaltSourceDataGridView);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SaltSourcesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Salt Sources";
			((System.ComponentModel.ISupportInitialize)(this.SaltSourceDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		private Button cancelButton;
		private Button okButton;
		private DataGridView SaltSourceDataGridView;
		private DataGridViewCheckBoxColumn SourceEnabled;
		private DataGridViewTextBoxColumn SourceName;
		private DataGridViewTextBoxColumn SourceMinimum;
		private DataGridViewTextBoxColumn SourceMaximum;
		private DataGridViewTextBoxColumn SourceCharacterSet;
		private Button RestoreDefaultsButton;
	}
}
