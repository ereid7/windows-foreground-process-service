using System;
using System.IO;

namespace AppTimerService.UnitTests.Helpers
{
    public class ForegroundProcessInfoRepositoryHelper
    {
        private readonly string _storeFileName = "ForegroundProcessInfo.xml";
        public string StorePath { get; private set; }
        public string SourcePath { get => $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}\\Resources\\{_storeFileName}"; }

        public void Initialize()
        {
            StorePath = CreateTempDirectory();
            CreateTempStore();
        }

        public void Cleanup()
        {
            try
            {
                Directory.Delete(StorePath, true);
            }
            catch (IOException iox)
            {
                // TODO handle
            }
        }

        private string CreateTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private void CreateTempStore()
        {
            var destinationPath = $"{StorePath}\\{_storeFileName}";
            try
            {
                using (FileStream fs = File.Open(SourcePath, FileMode.Open))
                using (FileStream ds = File.Create(destinationPath))
                {
                    fs.CopyTo(ds);
                }
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message);
            }
        }
    }
}
