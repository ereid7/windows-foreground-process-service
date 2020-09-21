using AppTimerService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AppTimerService.Repositories
{
    public class ForegroundProcessInfoRepository 
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

        public ForegroundProcessInfoEntity GetById(int id) {
            // TODO hanadle null
            return (from process in _items where process.Id == id select process).Single();
        }

        public void AddItem(ForegroundProcessInfoEntity entity)
        {
            _items.Add(entity);
        }

        public void UpdateItem(ForegroundProcessInfoEntity entity)
        {
            var index = _items.FindIndex(x =>
            {
                return x.Equals(entity.Id);
            });
            if (index >= 0)
            {
                _items[index] = entity;
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

        //public void Insert(ForegroundProcessInfo processInfo)
        //{
        //     _items.Add()
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
