using System.Collections.Generic;

public static class HistoryLog
{
    public class LogEntry
    {
        public string Speaker;
        public string Content;
        public string AudioName;

        public LogEntry(string speaker, string content, string audioName)
        {
            Speaker = speaker;
            Content = content;
            AudioName = audioName;
        }
    }

    private static List<LogEntry> entries = new List<LogEntry>();

    public static void AddEntry(string speaker, string content, string audioName)
    {
        entries.Add(new LogEntry(speaker, content, audioName));
    }

    public static List<LogEntry> GetEntries()
    {
        return entries;
    }

    public static void Clear()
    {
        entries.Clear();
    }
}
