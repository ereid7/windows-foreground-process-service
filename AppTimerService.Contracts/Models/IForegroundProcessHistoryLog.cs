using System;

namespace AppTimerService.Contracts.Models
{
    public interface IForegroundProcessHistoryLog
    {
        DateTime TimeStamp { get; set; }
        int NewProcessId { get; set; }
        string NewProcessName { get; set; }
        int LastProcessId { get; set; }
        string LastProcessName { get; set; }
        string LastProcessDuration { get; set; }
    }
}
