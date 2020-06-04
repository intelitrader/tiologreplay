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
        public string Entry { get; set; }
        public string Time { get; set; }
        public string Command { get; set; }
        public string Handle { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Metadata { get; set; }
        public string CurrentField { get; set; }

        public LogEntry(string entry)
        {
            if (entry.EndsWith("\\n"))
                throw new NotImplementedException();

            string key_info;
            string value_info;
            string key;
            string value;

            // Sets some values to their respective properties // Returns key's value info.
            // Tuple implementaion may slow tiologreplay, added for readability
            (this.Time, this.Command, this.Handle, key_info, key, value_info, value, _) = entry.Split(',', 7);

            if(Command == "create" || Command == "open")
            {
                this.Key = key;
                this.Value = value; 
            }
            else
            {
                // Sets values for the final command
                this.Key = Deserialize(key_info, key); 
                this.Value = Deserialize(value_info, value);
            }
        }

        private string Deserialize(string info, string data)
        {
            var type = info[0]; // Takes type out info (format: "s12"; 's' for string. 'n' is null)
           
            if(type == 'n')
                return null;

            var sizeL = info.Skip(1); // Gets only type's numbers

            int size = int.Parse(string.Join("", sizeL)); // Turns stringfied numbers into actual numbers

            string field = GetField();

            if (type == 's')
                return ($"{field} string {size} {data}");
            if (type == 'i')
                return ($"{field} int {size} {data}");
            if (type == 'd')
                return ($"{field} double {size} {data}");

            return null;
        }
        
        private string GetField()
        {
            switch (this.CurrentField)
            {
                case "key":
                    CurrentField = "value";
                    return "value";
                case "value":
                    CurrentField = 'metadata';
                    return "metadata";
                default:
                    CurrentField = "key";
                    return "key";
            }
        }

        public override string ToString()
        {
            return Command + ' ' + (Key != null ? Key + ' ' + Value : Value);
        }
    }
}
