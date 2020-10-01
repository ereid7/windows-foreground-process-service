using System;
using System.Diagnostics;

namespace AppTimerService.Models
{
    public class ForegroundProcessInfoEntity
    {
        public int Id;
        public string ProcessName;
        public string ForegroundDuration;

        public void Initialize(Process process)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            ForegroundDuration = TimeSpan.FromSeconds(0).ToString();
        }
    }
}
