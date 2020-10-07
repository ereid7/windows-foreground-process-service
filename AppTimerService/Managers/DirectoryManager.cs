using System;
using System.IO;

namespace AppTimerService.Managers
{
    public abstract class DirectoryManager 
    {

        protected readonly string _appTimerDataPath;

        protected string _dailyDataPath => $"{this._appTimerDataPath}\\{DateTime.Today.ToString("dd-MM-yyyy")}";

        public DirectoryManager()
        {
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _appTimerDataPath = $"{programDataPath}\\AppTimer";

            InitializeDirectoryManager();
        }

        private void InitializeDirectoryManager()
        {
            VerifyProgramDataDirectory();
            VerifyDailyDataDirectory();
        }

        protected void VerifyProgramDataDirectory()
        {
            Directory.CreateDirectory(this._appTimerDataPath);
        }
        
        protected void VerifyDailyDataDirectory()
        {
            Directory.CreateDirectory(this._dailyDataPath);
        }
    }
}
