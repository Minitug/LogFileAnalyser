//Step 1 — Parse logs and save CSV
//Pick a single log file format
//Read line by line, extract timestamp / level / source / message
//Save the parsed entries to CSV
//Step 2 — GUI
//WinForms window
//Load CSV or parse log file
//Show table of entries
//Step 3 — Filtering / sorting / searching
//Filter by level (INFO/WARN/ERROR)
//Search text
//Sort by timestamp
//Step 4 — Plotting / summary stats
//Number of errors per hour/day
//Top modules by error count
//Maybe a timeline graph
//Step 5 — Extra features / cleanup
//Delete old CSVs
//Export filtered data
//Optional SQLite backend (persist multiple logs, fast queries)


using System.Diagnostics;

namespace LogFileAnalyser
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LogAnalyserGUI()); // Uncomment to run the GUI

            //string logPath = @"C:\Users\tug09\AppData\Roaming\discord\logs";

            //LogParser.LoadFiles(logPath);

            //LogParser.ParseFolderAndSaveCSV(logPath, "DiscordLogs");
        }
    }
}