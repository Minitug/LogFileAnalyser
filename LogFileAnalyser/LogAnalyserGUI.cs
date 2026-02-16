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
        private bool _markedAll = false;

        private string _selectedFolder;

        private HashSet<string> _checkedFiles;
        private HashSet<string> _checkedLevels;

        private List<LogEntry> _currentLogEntries;

        public LogAnalyserGUI()
        {
            InitializeComponent();
            _selectedFolder = "";
            _checkedFiles = new HashSet<string>();
            _checkedLevels = new HashSet<string>();

            _currentLogEntries = new List<LogEntry>();

            listViewParsedLines.View = View.Details;
            listViewParsedLines.Columns.Clear();
            listViewParsedLines.Columns.Add("ID", 50);
            listViewParsedLines.Columns.Add("Source File", 150);
            listViewParsedLines.Columns.Add("Timestamp", 150);
            listViewParsedLines.Columns.Add("Level", 100);
            listViewParsedLines.Columns.Add("Component", 150);
            listViewParsedLines.Columns.Add("Message", 300);
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

        private void btnParseLogs_Click(object sender, EventArgs e)
        {
            bool saveToCSV = chkBoxSaveCSV.Checked;
            List<string> selectedFiles = chkListLogFiles.CheckedItems.Cast<string>().ToList();
            string csvPrefix = txtCSVPrefix.Text.Trim();
            string path = _selectedFolder;

            try
            {
                if (selectedFiles.Count == 0)
                {
                    MessageBox.Show("Please select at least one log file to parse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _currentLogEntries = LogParser.ParseFiles(selectedFiles, path);

                populateListView();

                if (saveToCSV)
                {
                    if (string.IsNullOrWhiteSpace(csvPrefix) || csvPrefix.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    {
                        MessageBox.Show("Please enter a valid CSV prefix before saving.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    LogParser.SaveToCSV(_currentLogEntries, csvPrefix);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while parsing logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMarkAllNone_Click(object sender, EventArgs e)
        {

            try
            {
                chkListLogFiles.ItemCheck -= chkListLogFiles_ItemCheck;

                if (_markedAll)
                {
                    for (int i = 0; i < chkListLogFiles.Items.Count; i++)
                    {
                        chkListLogFiles.SetItemChecked(i, false);
                    }
                    _markedAll = false;
                    btnMarkAllNone.Text = "Mark All";
                }
                else
                {
                    for (int i = 0; i < chkListLogFiles.Items.Count; i++)
                    {
                        chkListLogFiles.SetItemChecked(i, true);
                    }
                    _markedAll = true;
                    btnMarkAllNone.Text = "Unmark All";
                }
                chkListLogFiles.ItemCheck += chkListLogFiles_ItemCheck;
            }
            catch
            {
                chkListLogFiles.ItemCheck += chkListLogFiles_ItemCheck;
            }
        }

        private void chkListLogFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int checkedCount = chkListLogFiles.CheckedItems.Count;
            string filename = chkListLogFiles.Items[e.Index].ToString();

            if (e.NewValue == CheckState.Checked)
            {
                checkedCount++;
            }
            else
            {
                checkedCount--;
            }

            if (checkedCount == chkListLogFiles.Items.Count)
            {
                _markedAll = true;
                btnMarkAllNone.Text = "Unmark All";
            }
            else
            {
                _markedAll = false;
                btnMarkAllNone.Text = "Mark All";
            }
        }
        private void chkListFilterLevel_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string level = chkListFilterLevel.Items[e.Index].ToString();
            bool willBeChecked = e.NewValue == CheckState.Checked;

            if (willBeChecked)
            {
                _checkedLevels.Add(level);
            }
            else
            {
                _checkedLevels.Remove(level);
            }
            populateListView();
        }

        private void populateListView() 
        {
            //List does not return to include all items, when removing items in the ItemCheck event, so we need to keep track of checked levels separately and apply filtering here.
            //Does not seem to work, adding a second filter either. Needs more testing to confirm if the issue is with the event or the filtering logic.

            listViewParsedLines.Items.Clear();

            List<LogEntry> filteredLogEntries = new List<LogEntry>();

            filteredLogEntries = _currentLogEntries
                .Where(entry => _checkedLevels.Count == 0 || _checkedLevels.Contains(entry.Level, StringComparer.OrdinalIgnoreCase))
                .ToList();

            listViewParsedLines.BeginUpdate();

            var items = filteredLogEntries.Select(entry =>
            {
                var lvi = new ListViewItem(entry.ID.ToString());
                lvi.SubItems.Add(entry.SourceFile);
                lvi.SubItems.Add(entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
                lvi.SubItems.Add(entry.Level);
                lvi.SubItems.Add(entry.Component);
                lvi.SubItems.Add(entry.Message);
                return lvi;
            }).ToArray();

            listViewParsedLines.Items.AddRange(items);

            listViewParsedLines.EndUpdate();

            if (chkListFilterLevel.Items.Count != 0)
            {
                _checkedLevels = new HashSet<string>(chkListFilterLevel.CheckedItems.Cast<string>());
            }

            chkListFilterLevel.Items.Clear();
            chkListFilterLevel.Items.AddRange(_currentLogEntries
                    .Select(e => e.Level)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Distinct()
                    .OrderBy(l => l)
                    .ToArray());

            if (chkListFilterLevel.Items.Count != 0)
            {
                for (int i = 0; i < chkListFilterLevel.Items.Count; i++)
                {
                    string item = chkListFilterLevel.Items[i].ToString();
                    if (_checkedLevels.Contains(item))
                        chkListFilterLevel.SetItemChecked(i, true);
                }
            }
        }
    }
}
