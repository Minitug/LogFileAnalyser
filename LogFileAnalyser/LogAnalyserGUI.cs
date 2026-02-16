//TMP GUI PLAN
//1.Top: File Selection Panel
    //Folder Picker – Browse… button + textbox showing selected folder.
    //File List – list all .log files in the folder with checkboxes to include/exclude files.
    //Refresh Button – reload files in folder.
//2. Parsing Options Panel
    //Regex Formats to Apply – checkboxes for Format1…Format5.
    //Date Range – optional From / To pickers.
    //Log Levels – checkboxes: ERROR, WARNING, INFO, DEBUG, etc.
    //Skip Known Fails – checkbox to ignore patterns from KnownFailPatterns.txt.
    //Preview Only – checkbox to just parse without saving CSV.
//3. Filter & Search Panel
    //Level Filter – dropdown or multi-select list.
    //Component Filter – dropdown of components from parsed logs.
    //Text Search – simple text box (optional regex toggle).
    //Apply Filter Button – updates preview/grid.
//4. Parsed Log Preview
    //Grid/Table showing:
    //ID | Timestamp | Level | Component | Message | Source File
    //Failed Entries highlighted in red or with an icon.
    //Sortable Columns by timestamp, level, component, source file.
    //Optional: double-click row to see full message in popup.
//5. CSV Output / Actions
    //CSV Prefix / File Name textbox.
    //Buttons:
    //Parse Folder
    //Save CSV
    //Parse & Save CSV
    //Filter by Level
    //Progress Bar for large folders.
    //Status / Log Output – simple text area to show number of entries, failures, etc.
//6. Extras / Charts (Optional)
    //Simple bar chart: number of entries per level.
    //Timeline chart: errors over time.
    //Quick stats panel: Total entries, Failed entries, Errors, Warnings.


namespace LogFileAnalyser
{
    public partial class LogAnalyserGUI : Form
    {
        private string _selectedFolder;
        private HashSet<string> _checkedFiles;

        public LogAnalyserGUI()
        {
            InitializeComponent();
            _selectedFolder = "";
            _checkedFiles = new HashSet<string>();
        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            //_selectedFolder = txtFolderSelection.Text;
            using (var fbd = new FolderBrowserDialog())
            {
                updatechkListLogFiles(fbd.SelectedPath);
            }
        }

        private void txtFolderSelection_TextChanged(object sender, EventArgs e)
        {
            updatechkListLogFiles(txtFolderSelection.Text);
        }

        private void updatechkListLogFiles(string newFolderPath)
        {
            if (!Directory.Exists(newFolderPath))
            {
                lblFolderSelectError.Text = "Folder does not exist";
                return;
            }
            _checkedFiles = new HashSet<string>(chkListLogFiles.CheckedItems.Cast<string>());

            txtFolderSelection.TextChanged -= txtFolderSelection_TextChanged;
            txtFolderSelection.Text = newFolderPath;
            txtFolderSelection.TextChanged += txtFolderSelection_TextChanged;


            var logFiles = Directory.GetFiles(newFolderPath, "*.log");
            
            if (logFiles.Length == 0)
            {
                lblFolderSelectError.Text = "No .log files found in the selected folder";
                return;
            }
            chkListLogFiles.Items.Clear();
            foreach (var file in logFiles)
            {
                var filename = Path.GetFileName(file);
                int index = chkListLogFiles.Items.Add(filename);
                if (_checkedFiles.Contains(filename))
                    chkListLogFiles.SetItemChecked(index, true);
            }
            lblFolderSelectError.Text = "";
            _selectedFolder = newFolderPath;
        }
    }
}
