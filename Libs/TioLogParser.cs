using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace tioLogReplay.Libs
{
    public class TioLogParser
    {
        private TioConnection Tio { get; set; }
        private string Path { get; set; }
        public int Speed { get; }
        public int Delay { get; }
        public bool Follow { get; }
        public bool Pause { get; }

        readonly StatsLogger Logger;

        public TioLogParser(string address, string path, int speed, int delay, bool follow, bool pause)
        {
            Tio = TioConnection.Connect(address);
            Path = path;
            Speed = speed;
            Delay = delay;
            Follow = follow;
            Pause = pause;
            Logger = new StatsLogger();
        }

        public void Replay()
        {
            using StreamReader reader = (Path == "stdin")
                ? new StreamReader(Console.OpenStandardInput())
                : new StreamReader(new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            LogEntry log;
            string line;
            long lastMaxOffset = 0;
            int sendCount = 0;

            while (true)
            {
                if (Follow && Path != "stdin")
                {
                    //if the file size has not changed, idle
                    if (reader.BaseStream.Length == lastMaxOffset)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    //seek to the last max offset
                    reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);
                }

                //read out of the file until the EOF
                while ((line = reader.ReadLine()) != null)
                {
                    log = new LogEntry(line);

                    if (Delay > 0)
                        IdleByLogTime(log);

                    if (log.Key != null && !log.Key.StartsWith("__"))
                    {
                        Tio.SendCommand(log.ToFullCommand());

                        this.Logger.OnLogEntry(log);

                        sendCount += 1;
                        if (sendCount % 30000 == 0)
                        {
                            this.Logger.Log();
                            Console.WriteLine($"Sent: {line}");
                        }
                    }
                }

                if (Follow && Path != "stdin")
                {
                    Console.WriteLine("\nWaiting for file to grow...");

                    //update the last max offset
                    lastMaxOffset = reader.BaseStream.Position;
                }
            }
        }

        private void IdleByLogTime(LogEntry log)
        {
            TimeSpan timePassed = DateTime.Now - log.Time;
            var waitDecimal = Delay - Math.Floor(timePassed.TotalSeconds);
            var wait = (int)Math.Floor(waitDecimal);

            if (wait > 0)
            {
                Console.WriteLine($"\nDelay time is {Delay} seconds. It will take more {wait} seconds for the command to be sent.\n");
                Thread.Sleep(wait * 1000);
            }
        }
    }
}


