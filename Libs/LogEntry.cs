using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace tioLogReplay.Libs
{
    public class LogEntry
    {
        public DateTime Time { get; set; }
        public string Command { get; set; }
        public string Handle { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Data { get; set; }
        public string CurrentField { get; set; }

        public LogEntry(string entry)
        {
            if (entry.EndsWith("\\n"))
		entry = entry.Take(entry.Count() - 1);

	    string time;
            string keyInfo;
            string valueInfo;
            string key;
            string value;

            // Sets some values to their respective properties // Returns key's value info.
            // Tuple implementaion may slow tiologreplay, added for readability
            (time, this.Command, this.Handle, keyInfo, key, valueInfo, value, _) = entry.Split(',', 7);

            if(Command == "create" || Command == "open")
            {
                this.Key = key;
                this.Value = value;
            }
            else
            {
                // Sets values for the final command
                this.Key = Deserialize(keyInfo);
                this.Value = Deserialize(valueInfo);

                if (this.Key != null)
                    this.Data += $"\r\n{key}";
                if (this.Value != null)
                    this.Data += $"\r\n{value}";
            }

	    this.Time = DateTime.Parse(time);
        }

        private string Deserialize(string info)
        {
            var type = info[0]; // Takes type out info (format: "s12"; 's' for string. 'n' is null)

            if (type == 'n')
                return null;

            var sizeL = info.Skip(1); // Gets only type's numbers

            var size = int.Parse(string.Join("", sizeL)); // Turns stringfied numbers into actual numbers

            var field = GetField();

            return type switch
            {
                's' => ($"{field} string {size}"),
                'i' => ($"{field} int {size}"),
                'd' => ($"{field} double {size}"),
                _ => null
            };
        }

        private string GetField()
        {
            if (CurrentField == null)
            {
                CurrentField = "key";
                return "key";
            }
            else
            {
                CurrentField = "value";
                return "value";
            }
        }

        public override string ToString()
        {
            if (Key != null)
                return Command + ' ' + Key + ' ' + Value + Data;

            return Command + ' ' + Value + Data;
        }
    }
}
