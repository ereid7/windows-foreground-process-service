﻿using System;
using System.Diagnostics;

namespace AppTimerService.Models
{
    public class ForegroundProcessInfoEntity 
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public string ForegroundDuration { get; set; }

        public void Initialize(Process process)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            ForegroundDuration = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss\:fff");
        }
    }
}
