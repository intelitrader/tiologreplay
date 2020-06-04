using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using tioLogReplay.Libs;

namespace tioLogReplay
{
    class Program
    {
        static void Replay(string path, int speed, int delay, bool follow, bool pause)
        {
            TioConnection tio = new TioConnection();
            var log_file = File.ReadAllLines(path);

            foreach(var entry in log_file)
            {
                var log_line = new LogEntry(entry);
               tio.SendCommand(log_line.ToString());
            }

           /* var watch = new FileSystemWatcher();
            watch.Path = @"C:\Users\danil\Desktop\tiodb\build\server\tio\Debug\logs\";
            watch.Filter = "_20200527";
            watch.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            watch.EnableRaisingEvents = true;

            watch.Changed += new FileSystemEventHandler((source, e) => {
                        
                ); */
        }

        static void Main(string[] args)
        {
            string PATH = @"C:\Users\danil\Desktop\tiodb\build\server\tio\Debug\logs\_20200527";

            if (!File.Exists(PATH))
                throw new FileNotFoundException("Log not found");

            var options = new Options();

            options.Path = PATH; // Sets PATH as const for debbuging purposes

                Replay(options.Path,
                    options.Speed,
                    options.Delay,
                    options.Follow,
                    options.Pause);
        }
    }
}