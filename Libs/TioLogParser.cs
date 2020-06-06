using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace tioLogReplay.Libs
{
    public class TioLogParser
    {
        private TioConnection tio { get; set; }
        private StreamReader file { get; set; }
        private FileSystemWatcher watch { get; set; }
        
        public TioLogParser(string path, int speed, int delay, bool follow, bool pause)
        {
            string entry;
            file = new StreamReader(path);
            tio = new TioConnection();
            watch = new FileSystemWatcher();
            watch.Path = "";
            watch.Filter = "FILENAME";
            watch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
            watch.EnableRaisingEvents = true;
            watch.Changed += new FileSystemEventHandler((source, e) =>
                {
                    file.ReadToEndAsync();
                }
            );
            
            while ((entry = file.ReadLine()) != null)
            {
                var log = new LogEntry(entry);
                // Following switch could be replaced for a generic method that is able to send any command
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
