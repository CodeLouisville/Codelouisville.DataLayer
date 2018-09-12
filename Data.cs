using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeLouisville.DL
{
    public class Data<T>
    {
        private readonly string _dbName;
        private readonly string _dataFolder;

        public Data(string pathToDatabaseFile)
        {
            if (string.IsNullOrWhiteSpace(pathToDatabaseFile))
                throw new ArgumentException("the pathToDatabaseFile is required", "pathToDatabaseFile");


            _dataFolder = Path.GetDirectoryName(pathToDatabaseFile);
            _dbName = Path.GetFileName(pathToDatabaseFile);
        }
        public void Save(IList<T> data)
        {
            if (data?.Count == 0)
                return;

            CreateDataFolder(_dataFolder);

            var db = Path.Combine(_dataFolder, _dbName);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            using (var wr = new StreamWriter(db, false))
            {
                wr.Write(json);
            }

        }

        public void SaveOne(T data, Func<IList<T>, int> findExistingIndex)
        {
            var rows = Get();
            if (rows?.Count > 0)
            {
                var idx = findExistingIndex(rows);
                if (idx > 0)
                    rows.RemoveAt(idx);

            }
            rows.Add(data);


            Save(rows);
        }
        public IList<T> Get()
        {
            List<T> data = new List<T>();
            CreateDataFolder(_dataFolder);
            var dbFilePath = Path.Combine(_dataFolder, _dbName);
            string db = null;
            if (File.Exists(dbFilePath))
            {
                using (var rdr = new StreamReader(dbFilePath))
                {
                    db = rdr.ReadToEnd();
                }
                data.AddRange(JsonConvert.DeserializeObject<List<T>>(db));

            }

            return data;
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
        private void CreateDataFolder(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

        }


    }
}
