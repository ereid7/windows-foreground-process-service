using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimerService.Models
{
    class HistoryLog
    {
        public DateTime TimeStamp { get; set; }
        public string NewProcessId { get; set; }
        public string NewProcessName { get; set; }
        public string LastProcessId { get; set; }
        public string LastProcessName { get; set; }
        public string LastProcessDuration { get; set;}
    }
}
