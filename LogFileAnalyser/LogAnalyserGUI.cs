using System.Numerics;

namespace LogFileAnalyser
{
    public partial class LogAnalyserGUI : Form
    {
        private bool _markedAll = false;

        private string _selectedFolder;
        private string _textSearchTerm;

        private DateTime _selectedStartDate;
        private DateTime _selectedEndDate;

        private HashSet<string> _checkedFiles;
        private HashSet<string> _checkedLevels;
        private HashSet<string> _checkedComponents;

        private List<LogEntry> _currentLogEntries;

        public LogAnalyserGUI()
        {
            InitializeComponent();
            _selectedFolder = "";
            _textSearchTerm = "";

            _checkedFiles = new HashSet<string>();
            _checkedLevels = new HashSet<string>();
            _checkedComponents = new HashSet<string>();

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
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    updatechkListLogFiles(fbd.SelectedPath);
                }
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

        private async void btnParseLogs_Click(object sender, EventArgs e)
        {
            bool saveToCSV = chkBoxSaveCSV.Checked;
            List<string> selectedFiles = chkListLogFiles.CheckedItems.Cast<string>().ToList();
            string csvPrefix = txtCSVPrefix.Text.Trim();
            string path = _selectedFolder;

            btnParseLogs.Enabled = false;
            
            try
            {
                if (selectedFiles.Count == 0)
                {
                    MessageBox.Show("Please select at least one log file to parse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int failedCount;
                (_currentLogEntries, failedCount) = await Task.Run(() =>
                    LogParser.ParseFiles(selectedFiles, path, (current, totalFiles, parsed) =>
                    {
                        ParseProgressBar.Invoke(() =>
                        {
                            ParseProgressBar.Maximum = totalFiles;
                            ParseProgressBar.Value = current;
                            lblProgressBar.Text = $"Parsing file {current} of {totalFiles}...";
                            lblEntriesParsed.Text = $"Parsed {parsed} log entries.";
                        });
                    })
                );

                lblProgressBar.Text = "Successfully parsed all files"; 
                lblEntriesFailed.Text = $"Failed to parse {failedCount} lines.";

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

            btnParseLogs.Enabled = true;
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

        private void populateListView()
        {
            listViewParsedLines.Items.Clear();

            _textSearchTerm = txtBoxTxtSearch.Text.Trim();

            if (!chkBoxFilterDate.Checked)
            {
                _selectedStartDate = DateTime.MinValue;
                _selectedEndDate = DateTime.MaxValue;
            }

            var filteredLogEntries = _currentLogEntries
            .Where(e => (_checkedLevels.Count == 0 ||
                        _checkedLevels.Contains(e.Level, StringComparer.OrdinalIgnoreCase))
                        && (e.Timestamp >= _selectedStartDate && e.Timestamp <= _selectedEndDate)
                        && (_checkedComponents.Count == 0 ||
                        _checkedComponents.Contains(e.Component, StringComparer.OrdinalIgnoreCase))
                        && (string.IsNullOrEmpty(_textSearchTerm) ||
                        e.Message.Contains(_textSearchTerm, StringComparison.OrdinalIgnoreCase)))
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

            for (int i = 0; i < listViewParsedLines.Columns.Count - 1; i++)
            {
                listViewParsedLines.Columns[i].Width = -2; // Autosize to largest content
            }

            int otherColsWidth = 0;
            for (int i = 0; i < listViewParsedLines.Columns.Count - 1; i++)
            {
                otherColsWidth += listViewParsedLines.Columns[i].Width;
            }

            int remainingWidth = listViewParsedLines.ClientSize.Width - otherColsWidth;
            listViewParsedLines.Columns[listViewParsedLines.Columns.Count - 1].Width = remainingWidth > 50 ? remainingWidth : 50;

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

            chkListFilterComponent.Items.Clear();
            chkListFilterComponent.Items.AddRange(_currentLogEntries
                    .Select(e => e.Component)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Distinct()
                    .OrderBy(l => l)
                    .ToArray());

            if (chkListFilterComponent.Items.Count != 0)
            {
                for (int i = 0; i < chkListFilterComponent.Items.Count; i++)
                {
                    string item = chkListFilterComponent.Items[i].ToString();
                    if (_checkedComponents.Contains(item))
                        chkListFilterComponent.SetItemChecked(i, true);
                }
            }

            if (filteredLogEntries.Count == 0)
            {
                lblFilterError.Text = "No log entries match the selected filters.";
            }
            else
            {
                lblFilterError.Text = "";
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            _checkedLevels = new HashSet<string>(
                chkListFilterLevel.CheckedItems.Cast<string>()
            );

            _checkedComponents = new HashSet<string>(
                chkListFilterComponent.CheckedItems.Cast<string>()
            );


            _selectedStartDate = datePickStart.Value.Date;
            _selectedEndDate = datePickEnd.Value.Date.AddDays(1).AddTicks(-1);

            if (_selectedStartDate > _selectedEndDate)
            {
                lblFilterError.Text = "Start date cannot be after end date.";
                return;
            }
            else
            {
                lblFilterError.Text = "";
            }

            populateListView();
        }
    }
}
