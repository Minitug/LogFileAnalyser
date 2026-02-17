//Current goals:
//Progress & Status
//Optional but useful: simple progress bar for parsing large folders
//Text area showing number of entries, failures, etc.

//Possible future goals:
//1. Double-click row to see full message in popup
//2. Plotting / Summary Stats
//Number of errors per hour/day
//Top modules by error count
//Timeline / bar charts for log activity
//Quick stats panel: total entries, failed entries, errors, warnings
//3. Extra CSV Features
//Delete old CSVs
//Export filtered data only
//4. SQLite Backend
//Persist multiple logs
//Enable fast queries
//5. Live Monitoring / Auto-refresh
//Auto-refresh files when folder changes
//Optional “refresh” button


using System.Diagnostics;

namespace LogFileAnalyser
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LogAnalyserGUI()); 
        }
    }
}