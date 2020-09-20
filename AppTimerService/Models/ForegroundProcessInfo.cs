using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimerService.Models
{
    class ForegroundProcessInfo
    {
        public int Id;
        public string ProcessName;
        public HashSet<string> WindowTitles;
        public TimeSpan ForegroundDuration;
        public TimeSpan ProcessDuration;
        public DateTime StartTime;
    }
}
