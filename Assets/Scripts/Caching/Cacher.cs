using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public abstract class Cacher : MonoBehaviour
    {
        [SerializeField]
        string code;
       
        //protected abstract Data GetData();

        protected abstract string GetCacheValue();

        protected abstract void Init(string cacheValue);

        protected virtual void Awake()
        {
            CacheManager.Instance.OnSave += HandleOnSave;

            // Init object
            string value;
            if(CacheManager.Instance.TryGetCacheValue(code, out value))
                Init(value);
        }

        protected virtual void Start()
        {

        }

        void HandleOnSave()
        {
            CacheManager.Instance.UpdateCacheValue(code, GetCacheValue());
        }
    }

}
