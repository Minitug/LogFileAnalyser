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
            // LogAnalyserGUI
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowFrame;
            ClientSize = new Size(3238, 1199);
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
    }
}
