using System;
using System.Collections.Generic;
using System.Text;

namespace LogFileAnalyser
{
    public class LogParser
    {
        public List<string> ParseLog(string logContent)
        {
            List<string> logEntries = new List<string>();
            if (string.IsNullOrEmpty(logContent))
            {
                return logEntries;
            }
            string[] lines = logContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    logEntries.Add(line.Trim());
                }
            }
            return logEntries;
        }
    }
}
