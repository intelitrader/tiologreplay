using System;
using System.Collections.Generic;
using System.Text;

namespace tioLogReplay.Libs
{
    class StatsLogger
    {
        private int ContainerCount = 0;
        private int MessageCount = 0;
        private int LastLogMessageCount = 0;
        private int TotalData = 0;
        private int TotalChanges = 0;
        private DateTime LastLog { get; set; }

        public StatsLogger()
        {
            this.LastLog = DateTime.Now;
        }

        public void OnLogEntry(LogEntry log)
        {
            this.MessageCount += 1;
            this.TotalData += log.ToFullCommand().Length;

            if (log.Command == "create")
            {
                this.ContainerCount += 1;
            }
            else
            {
                this.TotalChanges += 1;
            }
        }


        public void Log()
        {
            var delta = (DateTime.Now - this.LastLog).TotalSeconds;
            var msgCount = this.MessageCount - this.LastLogMessageCount;
            var persec = msgCount / (delta > 0 ? delta : 1);
            var totalKb = this.TotalData / 1024;

            Console.WriteLine($"{MessageCount} msgs, {ContainerCount} containers, {TotalChanges} changes, {totalKb}kb so far, {persec:0.##} msgs/s");

            this.LastLog = DateTime.Now;
            this.LastLogMessageCount = this.MessageCount;
        }
    }
}
