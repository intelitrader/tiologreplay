using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tioLogReplay.Libs
{
    public class TioLogParser
    {
        private TioConnection Tio { get; set; }
        private StreamReader File { get; set; }
        public int Speed { get; }
        public int Delay { get; }
        public bool Follow { get; }
        public bool Pause { get; }

        // private FileSystemWatcher watch { get; set; }

        public TioLogParser(string path, int speed, int delay, bool follow, bool pause)
        {
            Tio = new TioConnection();
            File = new StreamReader(path);
            Speed = speed;
            Delay = delay;
            Follow = follow;
            Pause = pause;

            // Watches log updates and replays every change

            //watch = new FileSystemWatcher();
            //watch.Path = @"C:\Users\danil\Desktop\tiodb\build\server\tio\Debug\logs\";
            //watch.Filter = "_20200527";
            //watch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
            //watch.EnableRaisingEvents = true;
            //watch.Changed += new FileSystemEventHandler((source, e) =>
            //    {
            //        // Read to end of stream and send command
            //    }
            //);
        }

        public void WatchLog()
        {
            List<Task> tasks = new List<Task>();
            LogEntry log;
            string entry;

            while (true)
            {
                if (File.EndOfStream)
                {
                    while ((entry = File.ReadLine()) != null)
                    {
                        log = new LogEntry(entry);
                        Tio.SendCommand(log.ToFullCummand());
                    }
                }
                else
                {
                    Thread.Sleep(50); // waits 1/20 a second before trying to read the end of the stream again
                }
            }
        }

        public void WatchLogWithDelay() // still not async
        {
            LogEntry log;
            string entry;
            while (true)
            {
                if (File.EndOfStream)
                {
                    while ((entry = File.ReadLine()) != null)
                    {
                        log = new LogEntry(entry);

                        TimeSpan timePassed = DateTime.Now - log.Time;
                        var wait = (Delay * 1000) - (int)timePassed.TotalMilliseconds;

                        Thread.Sleep(wait);

                        Tio.SendCommand(log.ToFullCummand());
                    }
                }
                else
                {
                    Thread.Sleep(50); // waits 1/20 a second before trying to read of to end of stream again
                }
            }
        }

        public void CloneLog()
        {
            var tasks = new List<Task>();
            LogEntry log;
            string entry;

            while ((entry = File.ReadLine()) != null)
            {
                log = new LogEntry(entry);
                Tio.SendCommand(log.ToFullCummand());
            }
        }
    }
}


