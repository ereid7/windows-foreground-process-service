using System;

namespace AppTimerService.Models
{
    class ForegroundProcessHistoryLog 
    {
        public DateTime TimeStamp { get; set; }
        public int NewProcessId { get; set; }
        public string NewProcessName { get; set; }
        public int LastProcessId { get; set; }
        public string LastProcessName { get; set; }
        public string LastProcessDuration { get; set;}
        public ForegroundProcessHistoryLog(DateTime timeStamp,
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
