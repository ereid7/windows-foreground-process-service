using AppTimerService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AppTimerService.Repositories
{
    public class ForegroundProcessInfoRepository 
    {
        //https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer?view=netcore-3.1
        // https://github.com/SharpRepository/SharpRepository/blob/develop/SharpRepository.XmlRepository/XmlRepositoryBase.cs
        internal string FilePath { get; private set; }
        private readonly List<ForegroundProcessInfoContext> processInfoList;

        // TODO do not track duration when windows is locked
        public ForegroundProcessInfoRepository(string directoryPath) { 
        
            FilePath = $"{directoryPath}\\ForegroundProcessInfo.xml";

            if (!File.Exists(FilePath)) return;

            using (var stream = new FileStream(FilePath, FileMode.Open))
            using (StreamReader sr = new StreamReader(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ForegroundProcessInfoContext>));
                processInfoList = (List<ForegroundProcessInfoContext>)serializer.Deserialize(sr);
            }
        }

        public void Update()
        {
            using (StreamWriter sw = new StreamWriter(FilePath, false))
            {
                XmlSerializer serializer = new XmlSerializer(processInfoList.GetType());
                serializer.Serialize(sw, processInfoList);
                sw.Close();
            }
        }

        //public void Insert(ForegroundProcessInfo processInfo)
        //{
        //     processInfoList.Add()
        //}

        //public void Insert(ForegroundProcessInfo processInfo)
        //{

        //}

        //public Configuration GetById(object id)
        //{
        //    return (from c in configurations where c.Id == id select c).Single();
        //}
    }
}
