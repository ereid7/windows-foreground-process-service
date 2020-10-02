using AppTimerService.Contracts.Models;
using AppTimerService.Contracts.Repositories;
using AppTimerService.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AppTimerService.Repositories
{
    public class ForegroundProcessInfoRepository : IForegroundProcessInfoRepository
    {
        //https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer?view=netcore-3.1
        // https://github.com/SharpRepository/SharpRepository/blob/develop/SharpRepository.XmlRepository/XmlRepositoryBase.cs
        internal string FilePath { get; private set; }
        private readonly List<ForegroundProcessInfoEntity> _items;

        // TODO do not track duration when windows is locked
        public ForegroundProcessInfoRepository(string directoryPath) { 
        
            FilePath = $"{directoryPath}\\ForegroundProcessInfo.xml";
            _items = new List<ForegroundProcessInfoEntity>();

            if (!File.Exists(FilePath)) return;

            using (var stream = new FileStream(FilePath, FileMode.Open))
            using (StreamReader sr = new StreamReader(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ForegroundProcessInfoEntity>));
                _items = (List<ForegroundProcessInfoEntity>)serializer.Deserialize(sr);
            }
        }

        protected List<ForegroundProcessInfoEntity> Items
        {
            get
            {
                return _items;
            }
        }

        public IForegroundProcessInfoEntity GetById(int id) {
            foreach (var process in _items)
            {
                if (process.Id.Equals(id))
                {
                    return process;
                }
            }

            return null;
        }

        public void AddItem(IForegroundProcessInfoEntity entity)
        {
            _items.Add((ForegroundProcessInfoEntity)entity);
        }

        public void UpdateItem(IForegroundProcessInfoEntity entity)
        {
            var index = _items.FindIndex(x =>
            {
                return x.Equals(entity.Id);
            });
            if (index >= 0)
            {
                _items[index] = (ForegroundProcessInfoEntity)entity;
            }
        }

        public void SaveChanges()
        {
            using (StreamWriter sw = new StreamWriter(FilePath, false))
            {
                XmlSerializer serializer = new XmlSerializer(_items.GetType());
                serializer.Serialize(sw, _items);
                sw.Close();
            }
        }
    }
}
