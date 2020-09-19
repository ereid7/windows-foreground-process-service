using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimerService.Models
{
    public class ForegroundHistoryProcessInfo
    {
        public int Id;
        public string ProcessName;
        public HashSet<string> WindowTitles;
        public TimeSpan Duration;
    }
}
