using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AppTimerService.Models
{
    public class ForegroundProcessInfoContext
    {
        public int Id;
        public string ProcessName;
        [XmlArray("Titles")]
        public string[] WindowTitles;
        public string ForegroundDuration;
        public string ProcessDuration;
        public string StartTime;
    }
}
