using System.Windows.Forms;

namespace KeePassDiceware
{
	public partial class DicewareOptionsForm
	{
		private void InitializeComponent()
		{
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.wordSeparatorPromptedTextBox = new KeePass.UI.PromptedTextBox();
            this.wordCasingComboBox = new System.Windows.Forms.ComboBox();
            this.l33tSpeakComboBox = new System.Windows.Forms.ComboBox();
            this.saltComboBox = new System.Windows.Forms.ComboBox();
            this.wordListsListView = new System.Windows.Forms.ListView();
            this.wordCountLabel = new System.Windows.Forms.Label();
            this.wordSeparatorLabel = new System.Windows.Forms.Label();
            this.wordCasingsLabel = new System.Windows.Forms.Label();
            this.l33tSpeakLabel = new System.Windows.Forms.Label();
            this.saltsLabel = new System.Windows.Forms.Label();
            this.saltSourcesLabel = new System.Windows.Forms.Label();
            this.wordListsLabel = new System.Windows.Forms.Label();
            this.wordCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.saltSourcesListView = new System.Windows.Forms.ListView();
            this.ExplanationGroupBox = new System.Windows.Forms.GroupBox();
            this.xkcdLinkLabel = new System.Windows.Forms.LinkLabel();
            this.dicewareLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.wordCountNumericUpDown)).BeginInit();
            this.OptionsGroupBox.SuspendLayout();
            this.ExplanationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(406, 674);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(325, 674);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // wordSeparatorPromptedTextBox
            // 
            this.wordSeparatorPromptedTextBox.Location = new System.Drawing.Point(148, 40);
            this.wordSeparatorPromptedTextBox.Name = "wordSeparatorPromptedTextBox";
            this.wordSeparatorPromptedTextBox.PromptText = "String to be placed between words";
            this.wordSeparatorPromptedTextBox.Size = new System.Drawing.Size(300, 20);
            this.wordSeparatorPromptedTextBox.TabIndex = 3;
            // 
            // wordCasingComboBox
            // 
            this.wordCasingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wordCasingComboBox.FormattingEnabled = true;
            this.wordCasingComboBox.Location = new System.Drawing.Point(148, 66);
            this.wordCasingComboBox.Name = "wordCasingComboBox";
            this.wordCasingComboBox.Size = new System.Drawing.Size(300, 21);
            this.wordCasingComboBox.TabIndex = 5;
            // 
            // l33tSpeakComboBox
            // 
            this.l33tSpeakComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.l33tSpeakComboBox.FormattingEnabled = true;
            this.l33tSpeakComboBox.Location = new System.Drawing.Point(148, 93);
            this.l33tSpeakComboBox.Name = "l33tSpeakComboBox";
            this.l33tSpeakComboBox.Size = new System.Drawing.Size(300, 21);
            this.l33tSpeakComboBox.TabIndex = 5;
            // 
            // saltComboBox
            // 
            this.saltComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.saltComboBox.FormattingEnabled = true;
            this.saltComboBox.Location = new System.Drawing.Point(148, 120);
            this.saltComboBox.Name = "saltComboBox";
            this.saltComboBox.Size = new System.Drawing.Size(300, 21);
            this.saltComboBox.TabIndex = 5;
            // 
            // wordListsListView
            // 
            this.wordListsListView.CheckBoxes = true;
            this.wordListsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.wordListsListView.HideSelection = false;
            this.wordListsListView.Location = new System.Drawing.Point(148, 247);
            this.wordListsListView.Name = "wordListsListView";
            this.wordListsListView.Size = new System.Drawing.Size(300, 341);
            this.wordListsListView.TabIndex = 6;
            this.wordListsListView.UseCompatibleStateImageBehavior = false;
            this.wordListsListView.View = System.Windows.Forms.View.Details;
            // 
            // wordCountLabel
            // 
            this.wordCountLabel.AutoSize = true;
            this.wordCountLabel.Location = new System.Drawing.Point(14, 16);
            this.wordCountLabel.Name = "wordCountLabel";
            this.wordCountLabel.Size = new System.Drawing.Size(66, 13);
            this.wordCountLabel.TabIndex = 7;
            this.wordCountLabel.Text = "Word count:";
            // 
            // wordSeparatorLabel
            // 
            this.wordSeparatorLabel.AutoSize = true;
            this.wordSeparatorLabel.Location = new System.Drawing.Point(14, 43);
            this.wordSeparatorLabel.Name = "wordSeparatorLabel";
            this.wordSeparatorLabel.Size = new System.Drawing.Size(83, 13);
            this.wordSeparatorLabel.TabIndex = 7;
            this.wordSeparatorLabel.Text = "Word separator:";
            // 
            // wordCasingsLabel
            // 
            this.wordCasingsLabel.AutoSize = true;
            this.wordCasingsLabel.Location = new System.Drawing.Point(14, 69);
            this.wordCasingsLabel.Name = "wordCasingsLabel";
            this.wordCasingsLabel.Size = new System.Drawing.Size(75, 13);
            this.wordCasingsLabel.TabIndex = 7;
            this.wordCasingsLabel.Text = "Word casings:";
            // 
            // l33tSpeakLabel
            // 
            this.l33tSpeakLabel.AutoSize = true;
            this.l33tSpeakLabel.Location = new System.Drawing.Point(14, 96);
            this.l33tSpeakLabel.Name = "l33tSpeakLabel";
            this.l33tSpeakLabel.Size = new System.Drawing.Size(65, 13);
            this.l33tSpeakLabel.TabIndex = 7;
            this.l33tSpeakLabel.Text = "L33t Speak:";
            // 
            // saltsLabel
            // 
            this.saltsLabel.AutoSize = true;
            this.saltsLabel.Location = new System.Drawing.Point(14, 123);
            this.saltsLabel.Name = "saltsLabel";
            this.saltsLabel.Size = new System.Drawing.Size(28, 13);
            this.saltsLabel.TabIndex = 7;
            this.saltsLabel.Text = "Salt:";
            // 
            // saltSourcesLabel
            // 
            this.saltSourcesLabel.AutoSize = true;
            this.saltSourcesLabel.Location = new System.Drawing.Point(14, 150);
            this.saltSourcesLabel.Name = "saltSourcesLabel";
            this.saltSourcesLabel.Size = new System.Drawing.Size(116, 13);
            this.saltSourcesLabel.TabIndex = 7;
            this.saltSourcesLabel.Text = "Salt character sources:";
            // 
            // wordListsLabel
            // 
            this.wordListsLabel.AutoSize = true;
            this.wordListsLabel.Location = new System.Drawing.Point(14, 250);
            this.wordListsLabel.Name = "wordListsLabel";
            this.wordListsLabel.Size = new System.Drawing.Size(56, 13);
            this.wordListsLabel.TabIndex = 7;
            this.wordListsLabel.Text = "Word lists:";
            // 
            // wordCountNumericUpDown
            // 
            this.wordCountNumericUpDown.Location = new System.Drawing.Point(148, 14);
            this.wordCountNumericUpDown.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.wordCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.wordCountNumericUpDown.Name = "wordCountNumericUpDown";
            this.wordCountNumericUpDown.Size = new System.Drawing.Size(300, 20);
            this.wordCountNumericUpDown.TabIndex = 8;
            this.wordCountNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.saltSourcesListView);
            this.OptionsGroupBox.Controls.Add(this.wordCountLabel);
            this.OptionsGroupBox.Controls.Add(this.wordSeparatorPromptedTextBox);
            this.OptionsGroupBox.Controls.Add(this.wordCountNumericUpDown);
            this.OptionsGroupBox.Controls.Add(this.wordListsLabel);
            this.OptionsGroupBox.Controls.Add(this.wordCasingComboBox);
            this.OptionsGroupBox.Controls.Add(this.saltSourcesLabel);
            this.OptionsGroupBox.Controls.Add(this.l33tSpeakComboBox);
            this.OptionsGroupBox.Controls.Add(this.saltsLabel);
            this.OptionsGroupBox.Controls.Add(this.saltComboBox);
            this.OptionsGroupBox.Controls.Add(this.l33tSpeakLabel);
            this.OptionsGroupBox.Controls.Add(this.wordListsListView);
            this.OptionsGroupBox.Controls.Add(this.wordCasingsLabel);
            this.OptionsGroupBox.Controls.Add(this.wordSeparatorLabel);
            this.OptionsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(467, 598);
            this.OptionsGroupBox.TabIndex = 9;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Options";
            // 
            // saltSourcesListView
            // 
            this.saltSourcesListView.CheckBoxes = true;
            this.saltSourcesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.saltSourcesListView.HideSelection = false;
            this.saltSourcesListView.Location = new System.Drawing.Point(148, 150);
            this.saltSourcesListView.Name = "saltSourcesListView";
            this.saltSourcesListView.Size = new System.Drawing.Size(300, 91);
            this.saltSourcesListView.TabIndex = 9;
            this.saltSourcesListView.UseCompatibleStateImageBehavior = false;
            this.saltSourcesListView.View = System.Windows.Forms.View.Details;
            // 
            // ExplanationGroupBox
            // 
            this.ExplanationGroupBox.Controls.Add(this.xkcdLinkLabel);
            this.ExplanationGroupBox.Controls.Add(this.dicewareLinkLabel);
            this.ExplanationGroupBox.Location = new System.Drawing.Point(12, 616);
            this.ExplanationGroupBox.Name = "ExplanationGroupBox";
            this.ExplanationGroupBox.Size = new System.Drawing.Size(467, 50);
            this.ExplanationGroupBox.TabIndex = 9;
            this.ExplanationGroupBox.TabStop = false;
            this.ExplanationGroupBox.Text = "Explanation";
            // 
            // xkcdLinkLabel
            // 
            this.xkcdLinkLabel.AutoSize = true;
            this.xkcdLinkLabel.Location = new System.Drawing.Point(14, 29);
            this.xkcdLinkLabel.Name = "xkcdLinkLabel";
            this.xkcdLinkLabel.Size = new System.Drawing.Size(42, 13);
            this.xkcdLinkLabel.TabIndex = 0;
            this.xkcdLinkLabel.TabStop = true;
            this.xkcdLinkLabel.Tag = "https://xkcd.com/936/";
            this.xkcdLinkLabel.Text = "XKCD?";
            this.xkcdLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked_OpenTagAsLink);
            // 
            // dicewareLinkLabel
            // 
            this.dicewareLinkLabel.AutoSize = true;
            this.dicewareLinkLabel.Location = new System.Drawing.Point(14, 16);
            this.dicewareLinkLabel.Name = "dicewareLinkLabel";
            this.dicewareLinkLabel.Size = new System.Drawing.Size(153, 13);
            this.dicewareLinkLabel.TabIndex = 0;
            this.dicewareLinkLabel.TabStop = true;
            this.dicewareLinkLabel.Tag = "https://theworld.com/~reinhold/diceware.html";
            this.dicewareLinkLabel.Text = "What is the Diceware method?";
			this.dicewareLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked_OpenTagAsLink);
			// 
			// DicewareOptionsForm
			// 
			this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(493, 709);
            this.Controls.Add(this.ExplanationGroupBox);
            this.Controls.Add(this.OptionsGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DicewareOptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Diceware Password Generation Options";
            ((System.ComponentModel.ISupportInitialize)(this.wordCountNumericUpDown)).EndInit();
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            this.ExplanationGroupBox.ResumeLayout(false);
            this.ExplanationGroupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		private Button okButton;
		private Button cancelButton;
		private KeePass.UI.PromptedTextBox wordSeparatorPromptedTextBox;
		private ComboBox wordCasingComboBox;
		private ComboBox l33tSpeakComboBox;
		private ComboBox saltComboBox;
		private ListView wordListsListView;
		private Label wordCountLabel;
		private Label wordSeparatorLabel;
		private Label wordCasingsLabel;
		private Label l33tSpeakLabel;
		private Label saltsLabel;
		private Label saltSourcesLabel;
		private Label wordListsLabel;
		private NumericUpDown wordCountNumericUpDown;
		private GroupBox OptionsGroupBox;
		private GroupBox ExplanationGroupBox;
		private LinkLabel xkcdLinkLabel;
		private LinkLabel dicewareLinkLabel;
		private ListView saltSourcesListView;
	}
}
