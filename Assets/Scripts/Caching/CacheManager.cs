using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

namespace HW.CachingSystem
{
    public class CacheManager
    {
        public UnityAction OnSave;

        private string folder = Application.persistentDataPath + "/Saves/";
        private string file = "sav.txt";

        private static CacheManager instance;
        public static CacheManager Instance
        {
            get { 
                if (instance == null)
                    instance = new CacheManager();
                return instance;
            }
        }

        // Cache
        Dictionary<string, string> cache; // Cache name and data

        private CacheManager()
        {
            // Init cache list
            cache = new Dictionary<string, string>();

            // Create the base folder if needed
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Create save file if needed
            string path = Path.Combine(folder, file);
            if (!File.Exists(path))
                using (FileStream fs = File.Create(path)) { }
                    
                

            // Try to read from disk
            ReadCache();
        }


        // Adding data to cache
        public void UpdateCacheValue(string code, string data)
        {
            if (!cache.ContainsKey(code))
                cache.Add(code, data);
            else
                cache[code] = data;
            
        }

        public bool TryGetCacheValue(string code, out string value)
        {
            value = null;
            
            // Key doesn't exist
            if (!cache.ContainsKey(code))
                return false;
            
            // Fill the out param and return true
            value = cache[code];
            return true;
                
        }

        public void Save()
        {
            OnSave?.Invoke();

            WriteCache();
        }

        public void Load()
        {
            ReadCache();
        }

        private void ReadCache()
        {
            string fileTxt = File.ReadAllText(Path.Combine(folder, file));

            using(StringReader sr = new StringReader(fileTxt))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] s = line.Split(':');
                    cache.Add(s[0], s[1]);
                }
            }
            
        }

        private void WriteCache()
        {
            using (StringWriter sw = new StringWriter())
            {
                List<string> keys = new List<string>(cache.Keys);
                foreach(string key in keys)
                {
                    sw.WriteLine(key + ":" + cache[key]);
                }

                File.WriteAllText(Path.Combine(folder, file), sw.ToString());
            }
        }
    }

}
