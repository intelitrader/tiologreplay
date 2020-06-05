using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace tioLogReplay.Libs
{
    class TioLogParser
    {
        public TioLogParser(string path, int speed, int delay, bool follow, bool pause)
        {
            string entry;
            StreamReader file = new StreamReader(path);
            TioConnection tio = new TioConnection();

            while ((entry = file.ReadLine()) != null)
            {
                var log = new LogEntry(entry);
                
                // This switch could be replaced for a generic method that is able to send any command
                // Again, made for readability
                
                switch (log.Command)
                {
                    case "create":
                        tio.Create(log.Key, log.Value);
                        break;
                    case "open":
                        tio.Open(log.Key, log.Value);
                        break;
                    case "push_back":
                        tio.PushBack(entry.ToString());
                        break;
                    case "push_front":
                        tio.PushFront(entry.ToString());
                        break;
                    case "set":
                        tio.Set(entry.ToString());
                        break;
                    case "insert":
                        tio.Insert(entry.ToString());
                        break;
                }
            }
        }
    }
}
