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
        static void Main(string[] args)
        {
            const string path = @"C:\Users\danil\Desktop\tiodb\build\server\tio\Debug\logs\_20200527";

            if (!File.Exists(path))
                throw new FileNotFoundException("Log not found");

            var options = new Options();

            options.Path = path; // Sets PATH as const for debbuging purposes

            var tioLogParser = new TioLogParser(options.Path,
                options.Speed,
                options.Delay,
                options.Follow,
                options.Pause);

            tioLogParser.CloneLog(); // Clones existent log

            if (options.Delay == 0)
               tioLogParser.WatchLog(); // Watches log
            else
              tioLogParser.WatchLogWithDelay();
        }

    }
}