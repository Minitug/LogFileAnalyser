using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace LogFileAnalyser
{
    internal class LogParser
    {
        private string _logPath;
        private string _logFileName;

        private int _nextID = 1;

        internal LogParser(string logPath)
        {
            _logPath = logPath;
            _logFileName = Path.GetFileName(logPath);
        }

        internal List<LogEntry> ParseLogs()
        {
            List<LogEntry> entries = new List<LogEntry>();

            try
            {
                using (FileStream fs = new FileStream(
                    _logPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var entry = ParseLine(line);
                        if (entry != null)
                        {
                            entries.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading log file: {ex.Message}");
            }

            return entries;
        }

        internal LogEntry ParseLine(string line)
        {
            try
            {
                int firstClose = line.IndexOf(']');
                string timestampPart = line.Substring(1, firstClose - 1);

                int levelStart = firstClose + 2;
                int levelEnd = line.IndexOf(' ', levelStart);
                string level = line.Substring(levelStart, levelEnd - levelStart);

                int compStart = line.IndexOf('[', levelEnd) + 1;
                int compEnd = line.IndexOf(']', compStart);
                string component = line.Substring(compStart, compEnd - compStart);

                string message = line.Substring(compEnd + 3);

                return new LogEntry
                {
                    ID = _nextID++,
                    SourceFile = _logFileName,
                    Timestamp = DateTime.Parse(timestampPart),
                    Level = level,
                    Component = component,
                    Message = message
                };
            }
            catch
            {
                return null;
            }
        }

        internal static void SaveToCSV(List<LogEntry> entries, string logSource)
        {

            string fileName = $"{logSource}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("ID,Timestamp,Level,Component,Message,Sourcefile");
                    foreach (var entry in entries)
                    {
                        string line = $"{entry.ID},\"{entry.Timestamp:yyyy-MM-dd HH:mm:ss}\",\"{entry.Level}\",\"{entry.Component}\",\"{entry.Message.Replace("\"", "\"\"")}\",\"{entry.SourceFile}\"";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving to CSV: {ex.Message}");
            }
        }

        internal static void PrintEntries(List<LogEntry> entries)
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No log entries found.");
                return;
            }
            int idWidth = entries.Max(e => e.ID.ToString().Length);
            int sourceFileWidth = entries.Max(e => e.SourceFile.Length);
            int componentWidth = entries.Max(e => e.Component.Length);
            foreach (var entry in entries)
            {
                Debug.WriteLine(entry.ToString(idWidth, sourceFileWidth, componentWidth));
            }
        }

        internal static string[] LoadFiles(string directory)
        {
            string[] logFiles = Directory.GetFiles(directory, "*.log");

            return logFiles;
        }

        internal static void ParseFolderAndSaveCSV(string folderPath, string csvPrefix)
        {
            var logPaths = LoadFiles(folderPath).ToList();
            List<LogEntry> allLogs = new List<LogEntry>();
            int nextID = 1;

            foreach (var path in logPaths)
            {
                LogParser parser = new LogParser(path);
                var logs = parser.ParseLogs();

                foreach (var log in logs)
                    log.ID = nextID++;

                allLogs.AddRange(logs);
            }

            SaveToCSV(allLogs, csvPrefix);

            FilterByLevel(allLogs, new List<string> { "ERROR", "[warning]" });
        }

        internal static List<LogEntry> FilterByLevel(List<LogEntry> entries, List<string> level)
        {
            List<LogEntry> filtered = entries.Where(e => level.Contains(e.Level, StringComparer.OrdinalIgnoreCase)).ToList();
            PrintEntries(filtered);
            return filtered;
        }
    }
}
