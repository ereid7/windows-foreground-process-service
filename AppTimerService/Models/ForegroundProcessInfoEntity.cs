using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace AppTimerService.Models
{
    public class ForegroundProcessInfoEntity
    {
        public int Id;
        public string ProcessName;
        public string ForegroundDuration;
        public string ProcessDuration;
        public string StartTime;
        public string EndTime;

        public void Initialize(Process process)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            ForegroundDuration = TimeSpan.FromSeconds(0).ToString();
            ProcessDuration = (DateTime.Now - process.StartTime).Duration().ToString();
            StartTime = process.StartTime.ToString();
            EndTime = "";
        }
    }
}
