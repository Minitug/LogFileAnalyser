namespace LogFileAnalyser
{
    partial class LogAnalyserGUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnFolderSelect = new Button();
            txtFolderSelection = new TextBox();
            lblDescFolderSelect = new Label();
            chkListLogFiles = new CheckedListBox();
            lblFolderSelectError = new Label();
            chkBoxSaveCSV = new CheckBox();
            btnParseLogs = new Button();
            txtCSVPrefix = new TextBox();
            lblPrefix = new Label();
            listViewParsedLines = new ListView();
            btnMarkAllNone = new Button();
            SuspendLayout();
            // 
            // btnFolderSelect
            // 
            btnFolderSelect.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFolderSelect.Location = new Point(42, 68);
            btnFolderSelect.Name = "btnFolderSelect";
            btnFolderSelect.Size = new Size(179, 29);
            btnFolderSelect.TabIndex = 0;
            btnFolderSelect.Text = "Select Folder";
            btnFolderSelect.UseVisualStyleBackColor = true;
            btnFolderSelect.Click += btnFolderSelect_Click;
            // 
            // txtFolderSelection
            // 
            txtFolderSelection.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFolderSelection.Location = new Point(42, 102);
            txtFolderSelection.Name = "txtFolderSelection";
            txtFolderSelection.Size = new Size(642, 31);
            txtFolderSelection.TabIndex = 1;
            txtFolderSelection.Text = "C:\\Users\\tug09\\AppData\\Roaming\\discord\\logs";
            txtFolderSelection.TextChanged += txtFolderSelection_TextChanged;
            // 
            // lblDescFolderSelect
            // 
            lblDescFolderSelect.AutoSize = true;
            lblDescFolderSelect.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescFolderSelect.ForeColor = Color.Snow;
            lblDescFolderSelect.Location = new Point(42, 42);
            lblDescFolderSelect.Name = "lblDescFolderSelect";
            lblDescFolderSelect.Size = new Size(538, 23);
            lblDescFolderSelect.TabIndex = 2;
            lblDescFolderSelect.Text = "Please select the folder, you want the logs from";
            // 
            // chkListLogFiles
            // 
            chkListLogFiles.FormattingEnabled = true;
            chkListLogFiles.Location = new Point(42, 173);
            chkListLogFiles.Name = "chkListLogFiles";
            chkListLogFiles.Size = new Size(638, 312);
            chkListLogFiles.TabIndex = 3;
            chkListLogFiles.ItemCheck += chkListLogFiles_ItemCheck;
            // 
            // lblFolderSelectError
            // 
            lblFolderSelectError.AutoSize = true;
            lblFolderSelectError.ForeColor = Color.Red;
            lblFolderSelectError.Location = new Point(42, 136);
            lblFolderSelectError.Name = "lblFolderSelectError";
            lblFolderSelectError.Size = new Size(0, 23);
            lblFolderSelectError.TabIndex = 4;
            // 
            // chkBoxSaveCSV
            // 
            chkBoxSaveCSV.AutoSize = true;
            chkBoxSaveCSV.ForeColor = Color.Snow;
            chkBoxSaveCSV.Location = new Point(718, 41);
            chkBoxSaveCSV.Name = "chkBoxSaveCSV";
            chkBoxSaveCSV.Size = new Size(443, 27);
            chkBoxSaveCSV.TabIndex = 6;
            chkBoxSaveCSV.Text = "Do you want to save the parse to CSV?";
            chkBoxSaveCSV.UseVisualStyleBackColor = true;
            // 
            // btnParseLogs
            // 
            btnParseLogs.Location = new Point(718, 65);
            btnParseLogs.Name = "btnParseLogs";
            btnParseLogs.Size = new Size(142, 34);
            btnParseLogs.TabIndex = 7;
            btnParseLogs.Text = "Parse logs";
            btnParseLogs.UseVisualStyleBackColor = true;
            btnParseLogs.Click += btnParseLogs_Click;
            // 
            // txtCSVPrefix
            // 
            txtCSVPrefix.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCSVPrefix.Location = new Point(718, 136);
            txtCSVPrefix.Name = "txtCSVPrefix";
            txtCSVPrefix.Size = new Size(642, 31);
            txtCSVPrefix.TabIndex = 8;
            txtCSVPrefix.Text = "DiscordLogs";
            // 
            // lblPrefix
            // 
            lblPrefix.AutoSize = true;
            lblPrefix.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPrefix.ForeColor = Color.Snow;
            lblPrefix.Location = new Point(718, 105);
            lblPrefix.Name = "lblPrefix";
            lblPrefix.Size = new Size(1055, 23);
            lblPrefix.TabIndex = 9;
            lblPrefix.Text = "Please write the prefix, you want for your csv files. Only applicable if checkbox above checked";
            // 
            // listViewParsedLines
            // 
            listViewParsedLines.Location = new Point(718, 173);
            listViewParsedLines.Name = "listViewParsedLines";
            listViewParsedLines.Size = new Size(1055, 312);
            listViewParsedLines.TabIndex = 10;
            listViewParsedLines.UseCompatibleStateImageBehavior = false;
            // 
            // btnMarkAllNone
            // 
            btnMarkAllNone.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMarkAllNone.Location = new Point(240, 68);
            btnMarkAllNone.Name = "btnMarkAllNone";
            btnMarkAllNone.Size = new Size(139, 29);
            btnMarkAllNone.TabIndex = 11;
            btnMarkAllNone.Text = "Mark all";
            btnMarkAllNone.UseVisualStyleBackColor = true;
            btnMarkAllNone.Click += btnMarkAllNone_Click;
            // 
            // LogAnalyserGUI
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowFrame;
            ClientSize = new Size(3238, 1199);
            Controls.Add(btnMarkAllNone);
            Controls.Add(listViewParsedLines);
            Controls.Add(lblPrefix);
            Controls.Add(txtCSVPrefix);
            Controls.Add(btnParseLogs);
            Controls.Add(chkBoxSaveCSV);
            Controls.Add(lblFolderSelectError);
            Controls.Add(chkListLogFiles);
            Controls.Add(lblDescFolderSelect);
            Controls.Add(txtFolderSelection);
            Controls.Add(btnFolderSelect);
            Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "LogAnalyserGUI";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnFolderSelect;
        private TextBox txtFolderSelection;
        private Label lblDescFolderSelect;
        private CheckedListBox chkListLogFiles;
        private Label lblFolderSelectError;
        private CheckBox chkBoxSaveCSV;
        private Button btnParseLogs;
        private TextBox txtCSVPrefix;
        private Label lblPrefix;
        private ListView listViewParsedLines;
        private Button btnMarkAllNone;
    }
}
