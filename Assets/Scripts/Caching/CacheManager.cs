using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

namespace HW.CachingSystem
{
    public class CacheManager
    {
        public UnityEvent OnSave;

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
        List<Data> cache;

        private CacheManager()
        {
            // Init cache list
            cache = new List<Data>();

            // Create the base folder if needed
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }


        // Adding data to cache
        public void AddData(Data data)
        {
            cache.Add(data);
        }

        public void Save()
        {
            OnSave?.Invoke();

            //File.WriteAllText(Path.Combine(folder, file), cache.ToString());
        }
    }

}
