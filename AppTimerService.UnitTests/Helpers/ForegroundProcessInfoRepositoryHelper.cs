using AppTimerService.Models;
using System;
using System.Collections.Generic;
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
                Console.WriteLine(iox.Message);
            }
        }

        public bool ListContains(List<ForegroundProcessInfoEntity> l1, ForegroundProcessInfoEntity e1)
        {
            var idx = l1.FindIndex((item) =>
            {
                return EntitiesEqual(e1, item);
            });

            return idx > -1;
        }

        public ForegroundProcessInfoEntity CreateMockEntity(int id)
        {
            var foregroundProcessInfoEntity = new ForegroundProcessInfoEntity();
            foregroundProcessInfoEntity.Id = id;
            foregroundProcessInfoEntity.ProcessName = "mockProcessName";
            foregroundProcessInfoEntity.ForegroundDuration = "00:00:77:777";

            return foregroundProcessInfoEntity;
        }

        private bool EntitiesEqual(ForegroundProcessInfoEntity e1, ForegroundProcessInfoEntity e2)
        {
            return e1.Id.Equals(e2.Id) &&
                   e1.ProcessName.Equals(e2.ProcessName) &&
                   e1.ForegroundDuration.Equals(e2.ForegroundDuration);
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
