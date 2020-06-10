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
        private FileSystemWatcher watch { get; set; }

        public TioLogParser(string path, int speed, int delay, bool follow, bool pause)
        {
            Tio = new TioConnection();
            File = new StreamReader(path);

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

        public void WatchLogAsync() // Still not async
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
		 	tasks.Add(Task.Run(() => {
 	                       log = new LogEntry(entry);
                               Tio.SendCommand(log.ToString());
		           };
		       )
                    }

		    await Task.WhenAll(tasks); // don't know if it's the best way of doing it yet, needs debbuging
                }
                else
                {
                    Thread.Sleep(50); // waits 1/20 a second before trying to read of to end of stream again
                }
            }
        }

        public void WatchLogAsyncWithDelay(int delayInSeconds) // still not async
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

                        // these lines will be more useful when dealing with a big log queue
                        TimeSpan timePassed = DateTime.Now - log.Time;
                        var wait = (delayInSeconds * 1000) - (int) timePassed.TotalMilliseconds;
                        Thread.Sleep(wait);
                        Tio.SendCommand(log.ToString());
                    }
                }
                else
                {
                    Thread.Sleep(50); // waits 1/20 a second before trying to read of to end of stream again
                }
            }
        }

        public async Task CloneLogAsync()
        {
            var tasks = new List<Task>();
            LogEntry log;
            string entry;

            while ((entry = File.ReadLine()) != null)
            {
               log = new LogEntry(entry);
               tasks.Add(Task.Run(() => Tio.SendCommand(log.ToString())));
            }

            await Task.WhenAll(tasks);
        }
    }
}


