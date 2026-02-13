# Minitug's LogFileAnalyser

A WinForms C# application that parses log files, assigns unique IDs to entries, and exports structured CSV data for analysis.

## Features (MVP)
- Parse multiple .log files from a folder
- Extract key fields: Timestamp, Level, Component, Message, Source file
- Handle multiple Discord log formats (including previously failing lines)
- Assign sequential IDs to entries
- Export all parsed entries into a single CSV
- Filter out "known fail" patterns to reduce noise in analysis

## Tech Stack
- C#
- WinForms
- .NET
- System.IO
- Regex

*Note: GUI and advanced filtering/plotting features are planned for future iterations.*

~98k entries successfully parsed in testing
