using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACE.Entity
{
    public class StanceLog
    {
        public Queue<StanceLogEntry> Entries;

        public StanceLog()
        {
            Entries = new Queue<StanceLogEntry>();
        }

        public void Add(string line, string stackTrace)
        {
            Entries.Enqueue(new StanceLogEntry(line, stackTrace));

            if (Entries.Count > 5)
                Entries.Dequeue();
        }

        public void Show()
        {
            Console.WriteLine($"{ToString()}");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = Entries.Count - 1; i >= 0; i--)
            {
                var entry = Entries.ElementAt(i);
                sb.AppendLine($"{entry}\n------------------------------------------");
            }
            return sb.ToString();
        }
    }

    public class StanceLogEntry
    {
        public DateTime Timestamp;
        public string Line;
        public string StackTrace;

        public StanceLogEntry(string line, string stackTrace)
        {
            Timestamp = DateTime.UtcNow;
            Line = line;
            StackTrace = stackTrace;
        }

        public override string ToString()
        {
            var timestamp = Timestamp.ToString("yyyy-MM-dd hh:mm:ss,fff");

            return $"[{timestamp}] - {Line}\n{StackTrace}";
        }
    }
}
