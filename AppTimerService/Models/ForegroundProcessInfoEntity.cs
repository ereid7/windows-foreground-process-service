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
        // [XmlArray("Titles")]
        //public string[] WindowTitles;
        public string ForegroundDuration;
        public string ProcessDuration;
        public string StartTime;

        public ForegroundProcessInfoEntity(Process process)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            ForegroundDuration = "test";
            ProcessDuration = "test";
            StartTime = process.StartTime.ToString();
        }
    }
}
