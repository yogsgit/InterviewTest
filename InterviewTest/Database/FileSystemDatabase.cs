using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace InterviewTest.Database
{
    public class FileSystemDatabase
    {
        public const string Root = @"c:\temp\VidadosDB";

        public T Get<T>(string id) where T : class
        {
            var fqFilename = GetFQFilename<T>(id);

            if (!File.Exists(fqFilename))
                return null;

            return Deserialise<T>(fqFilename);
        }

        public List<T> GetAll<T>() where T : class, IPersistable
        {
            if (!Directory.Exists(GetFolder<T>()))
                return new List<T>();

            return Directory.GetFiles(GetFolder<T>()).Select(Deserialise<T>).ToList();
        }

        public void Save<T>(T obj) where T : IPersistable
        {
            var folder = GetFolder<T>();

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (obj.Id == null)
            {
                var files = Directory.GetFiles(folder);
                   
                int id = files.Any() ? files.Max(f => int.Parse(Path.GetFileNameWithoutExtension(f))) + 1: 0;
                obj.Id = id.ToString("00000");
            }

            var serialisedObject = JsonConvert.SerializeObject(obj);

            using (var sw = new StreamWriter(GetFQFilename<T>(obj.Id)))
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                sw.Write(serialisedObject);
            }
        }

        public void DeleteAll<T>()
        {
            Directory.Delete(GetFolder<T>(), true);
        }

        private T Deserialise<T>(string fqFilename)
        {
            string serialisedObject;
            using (var sr = new StreamReader(fqFilename))
            {
                serialisedObject = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(serialisedObject);

        }

        /// <summary>
        /// [YOGESH] - Added this method to enable saving a List of types IPersistable.
        /// </summary>
        /// <typeparam name="T">Any IPersistable type</typeparam>
        /// <param name="obj">List of type IPersistable</param>
        public void SaveAll<T>(List<T> obj) where T : IPersistable
        {
            foreach (T ele in obj)
            {
                Save(ele);
            }
        }

        private static string GetFolder<T>() => Path.Combine(Root, typeof(T).Name);
        private static string GetFQFilename<T>(string id) => Path.Combine(GetFolder<T>(), id + ".json");
    }
}