using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimerService.Models
{
    class TrackedProcessInfo
    {
        public TrackedProcessInfo(string id, string name, string duration)
        {
            this.id = id;
            this.name = name;
            this.duration = duration;
        }

        public string id;
        public string name;
        public string duration;
    }
}
