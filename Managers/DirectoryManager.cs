using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppTimerService.Managers
{
    public class DirectoryManager
    {

        private readonly string _appTimerDataPath;

        public DirectoryManager()
        {
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            this._appTimerDataPath = $"{programDataPath}\\AppTimer";

            this.initializeDirectoryManager();
        }

        private void initializeDirectoryManager()
        {
            this.verifyProgramDataDirectory();
            this.verifyDailyDataDirectory();
        }

        // TODO add error handling
        private void verifyProgramDataDirectory()
        {
            if (!Directory.Exists(this._appTimerDataPath))
            {
                Directory.CreateDirectory(this._appTimerDataPath);
            }
        }
        
        private void verifyDailyDataDirectory()
        {
            var dailyDataPath = $"{this._appTimerDataPath}\\{DateTime.Today.ToString("dd-MM-yyyy")}";
            if (!Directory.Exists(dailyDataPath))
            {
                Directory.CreateDirectory(dailyDataPath);
            }
        }
    }
}
