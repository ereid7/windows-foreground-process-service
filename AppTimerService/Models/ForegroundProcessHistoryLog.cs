using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimerService.Models
{
    class ForegroundHistoryLog
    {
        public DateTime TimeStamp { get; set; }
        public int NewProcessId { get; set; }
        public string NewProcessName { get; set; }
        public int LastProcessId { get; set; }
        public string LastProcessName { get; set; }
        public string LastProcessDuration { get; set;}
        public ForegroundHistoryLog(DateTime timeStamp,
                                    int newProcessId,
                                    string newProcessName,
                                    int lastProcessId,
                                    string lastProcessName,
                                    string lastProcessDuration)
        {
            TimeStamp = timeStamp;
            NewProcessId = newProcessId;
            NewProcessName = newProcessName;
            LastProcessId = lastProcessId;
            LastProcessName = lastProcessName;
            LastProcessDuration = lastProcessDuration;
        }
    }
}
