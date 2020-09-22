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
            if ("".Equals(code.Trim()))
                return;

            CacheManager.Instance.OnCacheUpdate += HandleOnCacheUpdate;

            // Init object
            string value;
            if(CacheManager.Instance.TryGetCacheValue(code, out value))
                Init(value);
        }

        protected virtual void Start()
        {

        }

        void HandleOnCacheUpdate()
        {
            
            string cacheValue = GetCacheValue();
            if(!"".Equals(cacheValue))
                CacheManager.Instance.UpdateCacheValue(code, GetCacheValue());
        }

        protected virtual void OnDestroy()
        {
            if ("".Equals(code.Trim()))
                return;

            CacheManager.Instance.OnCacheUpdate -= HandleOnCacheUpdate;
        }
    }

}
