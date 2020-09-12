using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HW.CachingSystem
{
    public class LevelCacher : Cacher
    {
        protected override string GetCacheValue()
        {
            return SceneManager.GetActiveScene().buildIndex.ToString();
        }

        protected override void Init(string cacheValue)
        {
            // Nothing to do
        }

      
    }

}
