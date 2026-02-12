using System;
using System.Text;
using System.Collections.Generic;

namespace LogFileAnalyser
{
    internal class LogEntry
    {
        internal int ID { get; set; }
        internal string SourceFile { get; set; }
        internal DateTime Timestamp { get; set; }
        internal string Level { get; set; }
        internal string Component { get; set; }
        internal string Message { get; set; }

        public string ToString(int idWidth, int sourceFileWidth, int componentWidth)
        {
            return string.Format(
                "{0," + idWidth + "} | {1," + sourceFileWidth + "} | {2:yyyy-MM-dd HH:mm:ss} | {3,-8} | {4,-" + componentWidth + "} | {5}",
                ID,
                SourceFile,
                Timestamp,
                Level,
                Component,
                Message
            );
        }
    }
}
