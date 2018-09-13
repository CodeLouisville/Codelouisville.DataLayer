using System;
using System.Collections.Generic;
using System.IO;
using CodeLouisville.DL.Interfaces;
using Newtonsoft.Json;

namespace CodeLouisville.DL.Models
{
    public abstract class BaseData<T> : IData<T>
    {

        private readonly string _dbName;
        
        private readonly string _dataFolder;

        protected BaseData(string pathToDatabaseFile)
        {
            if (string.IsNullOrWhiteSpace(pathToDatabaseFile))
                throw new ArgumentException("the pathToDatabaseFile is required", "pathToDatabaseFile");


            _dataFolder = Path.GetDirectoryName(pathToDatabaseFile);
            _dbName = Path.GetFileName(pathToDatabaseFile);
        }

        public T GetByQuery(Func<IList<T>, T> query)
        {
            T data = default(T);

            var rows = Get();
            if (rows?.Count > 0)
            {
                data = query(rows);
            }

            return data;
        }

        public abstract IList<T> Get();

        public abstract void Save(IList<T> collection);
        public void SaveOne(T obj, Func<IList<T>, int> query)
        {
            var rows = Get();
            if (rows?.Count > 0)
            {
                var idx = query(rows);
                if (idx > 0)
                    rows.RemoveAt(idx);

            }
            rows.Add(obj);


            Save(rows);
        }

        private void CreateDataFolder(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

        }

        protected void SaveToDisk(string serializedData)
        {
            CreateDataFolder(_dataFolder);

            var db = Path.Combine(_dataFolder, _dbName);

            using (var wr = new StreamWriter(db, false))
            {
                wr.Write(serializedData);
            }
        }
        
        protected string GetFromDisk()
        {
            string data = null;
            CreateDataFolder(_dataFolder);
            var dbFilePath = Path.Combine(_dataFolder, _dbName);
            if (File.Exists(dbFilePath))
            {
                using (var rdr = new StreamReader(dbFilePath))
                {
                    data = rdr.ReadToEnd();
                }
            }
            return data;
        }
    }
}