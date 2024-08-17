namespace KeePassDiceware
{
	partial class WordListsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.WordListDataGridView = new System.Windows.Forms.DataGridView();
            this.RestoreDefaultsButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.AddNewButton = new System.Windows.Forms.Button();
            this.wordListEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.wordListName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wordListPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wordListCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wordListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.WordListDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wordListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // WordListDataGridView
            // 
            this.WordListDataGridView.AllowUserToAddRows = false;
            this.WordListDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WordListDataGridView.AutoGenerateColumns = false;
            this.WordListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.WordListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WordListDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wordListEnabled,
            this.wordListName,
            this.wordListPath,
            this.wordListCategory});
            this.WordListDataGridView.DataSource = this.wordListBindingSource;
            this.WordListDataGridView.Location = new System.Drawing.Point(18, 18);
            this.WordListDataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.WordListDataGridView.Name = "WordListDataGridView";
            this.WordListDataGridView.RowHeadersWidth = 62;
            this.WordListDataGridView.RowTemplate.Height = 28;
            this.WordListDataGridView.Size = new System.Drawing.Size(956, 417);
            this.WordListDataGridView.TabIndex = 0;
            this.WordListDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.WordListDataGridView_CellDoubleClick);
            this.WordListDataGridView.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.WordListDataGridView_RowValidated);
            this.WordListDataGridView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.WordListDataGridView_RowValidating);
            // 
            // RestoreDefaultsButton
            // 
            this.RestoreDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RestoreDefaultsButton.CausesValidation = false;
            this.RestoreDefaultsButton.Location = new System.Drawing.Point(18, 444);
            this.RestoreDefaultsButton.Margin = new System.Windows.Forms.Padding(4);
            this.RestoreDefaultsButton.Name = "RestoreDefaultsButton";
            this.RestoreDefaultsButton.Size = new System.Drawing.Size(212, 34);
            this.RestoreDefaultsButton.TabIndex = 1;
            this.RestoreDefaultsButton.Text = "Restore &Defaults";
            this.RestoreDefaultsButton.UseVisualStyleBackColor = true;
            this.RestoreDefaultsButton.Click += new System.EventHandler(this.RestoreDefaultsButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(740, 444);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 34);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(861, 444);
            this.okButton.Margin = new System.Windows.Forms.Padding(4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(112, 34);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // AddNewButton
            // 
            this.AddNewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddNewButton.CausesValidation = true;
            this.AddNewButton.Location = new System.Drawing.Point(238, 444);
            this.AddNewButton.Margin = new System.Windows.Forms.Padding(4);
            this.AddNewButton.Name = "AddNewButton";
            this.AddNewButton.Size = new System.Drawing.Size(112, 34);
            this.AddNewButton.TabIndex = 5;
            this.AddNewButton.Text = "&Add";
            this.AddNewButton.UseVisualStyleBackColor = true;
            this.AddNewButton.Click += new System.EventHandler(this.AddNewButton_Click);
            // 
            // wordListEnabled
            // 
            this.wordListEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.wordListEnabled.DataPropertyName = "Enabled";
            this.wordListEnabled.FillWeight = 1F;
            this.wordListEnabled.HeaderText = "Enabled";
            this.wordListEnabled.MinimumWidth = 40;
            this.wordListEnabled.Name = "wordListEnabled";
            this.wordListEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.wordListEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.wordListEnabled.Width = 75;
            // 
            // wordListName
            // 
            this.wordListName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.wordListName.DataPropertyName = "Name";
            this.wordListName.FillWeight = 80F;
            this.wordListName.HeaderText = "Name";
            this.wordListName.MinimumWidth = 20;
            this.wordListName.Name = "wordListName";
            this.wordListName.Width = 250;
            // 
            // wordListPath
            // 
            this.wordListPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.wordListPath.DataPropertyName = "Path";
            this.wordListPath.FillWeight = 200F;
            this.wordListPath.HeaderText = "Path";
            this.wordListPath.MinimumWidth = 10;
            this.wordListPath.Name = "wordListPath";
            this.wordListPath.ReadOnly = true;
            // 
            // wordListCategory
            // 
            this.wordListCategory.DataPropertyName = "Category";
            this.wordListCategory.FillWeight = 50F;
            this.wordListCategory.HeaderText = "Category";
            this.wordListCategory.MinimumWidth = 10;
            this.wordListCategory.Name = "wordListCategory";
            this.wordListCategory.ReadOnly = true;
            // 
            // wordListBindingSource
            // 
            this.wordListBindingSource.DataSource = typeof(KeePassDiceware.WordList);
            // 
            // WordListsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(992, 496);
            this.Controls.Add(this.AddNewButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.RestoreDefaultsButton);
            this.Controls.Add(this.WordListDataGridView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "WordListsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Word Lists";
            ((System.ComponentModel.ISupportInitialize)(this.WordListDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wordListBindingSource)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView WordListDataGridView;
		private System.Windows.Forms.Button RestoreDefaultsButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.BindingSource wordListBindingSource;
		private System.Windows.Forms.DataGridViewCheckBoxColumn wordListEnabled;
		private System.Windows.Forms.DataGridViewTextBoxColumn wordListName;
		private System.Windows.Forms.DataGridViewTextBoxColumn wordListPath;
		private System.Windows.Forms.DataGridViewTextBoxColumn wordListCategory;
		private System.Windows.Forms.Button AddNewButton;
	}
}
