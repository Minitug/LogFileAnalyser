using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LogFileAnalyser
{
    internal class LogParser
    {
        private string _logPath;
        private string _logFileName;

        private static int _nextID = 1;

        private static readonly string[] _timestampFormats =
        {
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MMM-dd HH:mm:ss.fff",
            "yyyy-MMM-dd HH:mm:ss.fff zzz",
            "yyyy-MMM-dd HH:mm:ss.fff K"
        };



        private static readonly Regex _regexFormat1 =
        new Regex(
        @"^\[(?<timestamp>.*?)\]\[\s*\d+\s*:\s*\d+\s*\]\[(?<level>[A-Za-z]+)\s*\]\s+(?<message>.*)$",
        RegexOptions.Compiled
        );

        private static readonly Regex _regexFormat2 =
        new Regex(
        @"^\[(?<timestamp>[^\]]+)\]\[\s*\d+:\s*\d+\]\[(?<level>[A-Za-z]+)\s*\]\s+.*?\s+(?<message>%c\[.*\]|Overlay2:.*)$",
        RegexOptions.Compiled
        );

        private static readonly Regex _regexFormat3 =
        new Regex(
        @"^\[(?<timestamp>.*?)\]\s+\[(?<level>[A-Za-z]+)\]\s*(\[(?<component>.*?)\])?\s*(?<message>.*)$",
        RegexOptions.Compiled
        );

        private static readonly Regex _regexFormat4 =
        new Regex(
        @"^\[(?<timestamp>.*?)\]\s+\[?(?<level>[A-Za-z]+)\]?\s+(?<message>.+)$",
        RegexOptions.Compiled
        );

        private static readonly Regex _regexFormat5 =
        new Regex(
        @"^\[(?<timestamp>.*?)\]\s+(?<level>[A-Za-z]+)\s*(\[(?<component>.*?)\])?:\s+(?<message>.*)$",
        RegexOptions.Compiled
        );


        private static List<LogEntry> _failedEntries = new List<LogEntry>();

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
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

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
            Regex[] regexes = new[] { _regexFormat1, _regexFormat2, _regexFormat3, _regexFormat4, _regexFormat5 };
            Regex matchRegex = _regexFormat1;
            Match match = null;

            foreach (var regex in regexes)
            {
                match = regex.Match(line);
                matchRegex = regex;
                if (match.Success)
                {
                    break;
                }
            }

            if (match == null || !match.Success)
            {
                _failedEntries.Add(new LogEntry
                {
                    ID = 0,
                    SourceFile = _logFileName,
                    Timestamp = DateTime.Now,
                    Level = "PARSE_ERROR",
                    Component = "",
                    Message = line
                });
                return null;
            }

            string component = match.Groups["component"].Success
                      ? match.Groups["component"].Value
                      : "";

            string level = match.Groups["level"].Value.ToUpperInvariant();

            if (level == "WARNING")
            {
                level = "WARN";
            }


            string timestampText = match.Groups["timestamp"].Value;
            DateTime timestampValue;

            if (!DateTime.TryParseExact(
                    match.Groups["timestamp"].Value,
                    _timestampFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces,
                    out timestampValue))
            {
                if (!DateTime.TryParse(timestampText, out timestampValue))
                {
                    _failedEntries.Add(new LogEntry
                    {
                        ID = 0,
                        SourceFile = _logFileName,
                        Timestamp = DateTime.Now,
                        Level = "PARSE_ERROR",
                        Component = "",
                        Message = $"Failed timestamp parse: {line}, matchRegex: {matchRegex}"
                    });
                    return null;
                }
            }

            

            return new LogEntry
            {
                ID = _nextID++,
                SourceFile = _logFileName,
                Timestamp = timestampValue,
                Level = level,
                Component = component,
                Message = match.Groups["message"].Value
            };
        }

        internal static void SaveToCSV(List<LogEntry> entries, string logSource)
        {
            string knownFailPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "KnownFailPatterns.txt");
            var knownFails = LoadKnownFails(knownFailPath);

            var groupedFailed = _failedEntries
                .GroupBy(e => e.Message)
                .Where(g => !knownFails.Any(f => g.Key.Contains(f)))
                .Select(g => new { Message = g.Key, Count = g.Count(), g.First().SourceFile })
                .OrderByDescending(g => g.Count)
                .ToList();

            string fileName = $"{logSource}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("ID,Timestamp,Level,Component,Message,Sourcefile");
                foreach (var entry in entries)
                {
                    sb.AppendLine($"{entry.ID},\"{entry.Timestamp:yyyy-MM-dd HH:mm:ss}\",\"{entry.Level}\",\"{entry.Component}\",\"{entry.Message.Replace("\"", "\"\"")}\",\"{entry.SourceFile}\"");
                }
                File.WriteAllText(fileName, sb.ToString());

                var sbFailed = new StringBuilder();
                sbFailed.AppendLine(("Message,Occurences,Sourcefile"));
                foreach (var entry in groupedFailed)
                {
                    sbFailed.AppendLine($"\"{entry.Message.Replace("\"", "\"\"")}\",\"{entry.Count}\",\"{entry.SourceFile}\"");
                }
                File.WriteAllText($"failed_lines_{fileName}", sbFailed.ToString());

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
            //PrintEntries(filtered);
            return filtered;
        }

        internal static List<String> LoadKnownFails(string path)
        {
            return File.ReadAllLines(path)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
        }

        internal static List<LogEntry> ParseFiles(List<string> selectedFiles, string folderPath)
        {
            _nextID = 1;
            List<LogEntry> logEntries = new List<LogEntry>();
            List<string> fullPaths = selectedFiles.Select(file => Path.Combine(folderPath, file)).ToList();
            foreach (var file in fullPaths)
            {
                LogParser parser = new LogParser(file);
                var logs = parser.ParseLogs();

                foreach (var log in logs)
                    log.ID = _nextID++;


                logEntries.AddRange(logs);
            }
            


            return logEntries;
        }

        internal static void SaveToCSV2(List<LogEntry> entries, string csvPrefix)
        {

        }
    }
}
