﻿using System;
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
        public string Data { get; set; }
        public string CurrentField { get; set; }
        public DateTime ParseTime { get; set; }

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
                this.Key = Deserialize(key_info);
                this.Value = Deserialize(value_info);
 
                if(this.Key != null)
                    this.Data += $"\r\n{key}"; 
                if(this.Value != null)
                    this.Data += $"\r\n{value}";
            }
            // this.ParseTime = 
        }

        private string Deserialize(string info)
        {
            var type = info[0]; // Takes type out info (format: "s12"; 's' for string. 'n' is null)
           
            if(type == 'n')
                return null;

            var sizeL = info.Skip(1); // Gets only type's numbers

            int size = int.Parse(string.Join("", sizeL)); // Turns stringfied numbers into actual numbers

            string field = GetField();

            if (type == 's')
                return ($"{field} string {size}");
            if (type == 'i')
                return ($"{field} int {size}");
            if (type == 'd')
                return ($"{field} double {size}");

            return null;
        }
        
        private string GetField()
        {
            return CurrentField == null ? "key" : "value";    
        }

        public override string ToString()
        {
            if (Key != null)
                return Command + ' ' + Key + ' ' + Value + Data;

            return Command + ' ' + Value + Data;
        }
    }
}
