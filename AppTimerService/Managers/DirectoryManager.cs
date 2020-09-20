using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppTimerService.Managers
{
    public class DirectoryManager
    {

        protected readonly string _appTimerDataPath;

        protected string _dailyDataPath => $"{this._appTimerDataPath}\\{DateTime.Today.ToString("dd-MM-yyyy")}";

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

        protected void verifyProgramDataDirectory()
        {
            Directory.CreateDirectory(this._appTimerDataPath);
        }
        
        protected void verifyDailyDataDirectory()
        {
            Directory.CreateDirectory(this._dailyDataPath);
        }
    }
}
