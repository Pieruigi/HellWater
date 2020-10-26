using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW.CachingSystem
{
    public class EquipmentCacher : Cacher
    {
        
        Equipment equipment;

        protected override void Awake()
        {
            equipment = GetComponent<Equipment>();
            base.Awake();
        }

        protected override string GetCacheValue()
        {
            List<string> codes;
            List<int> amounts;
            equipment.GetDataForCaching(out codes, out amounts);

            ObjectListData old = new ObjectListData();

            for(int i=0; i<codes.Count; i++)
            {
                old.AddObject(codes[i], amounts[i]);
            }

            return old.Format();
        }

        

        protected override void Init(string cacheValue)
        {
            if (cacheValue == null || "".Equals(cacheValue))
                return;

            Debug.Log("Cache loaded:" + cacheValue);
            ObjectListData old = new ObjectListData();
            old.Parse(cacheValue);

            // Get resources
            List<Item> items = new List<Item>(Resources.LoadAll<Item>(Constants.ResourceFolderEquipment));
            //Debug.Log("Resources found:" + Resources.LoadAll(ResourcesFolder).Length);
            Debug.Log("Resources found:" + items.Count);

            // Loop through cache
            foreach (ObjectListData.ObjectInfo oi in old.Objects)
            {
                Debug.Log("Look for:" + oi.Code);
                // Get resource
                Item item = items.Find(i => i.Code == oi.Code);

                equipment.Add(item, oi.Amount);
            }
        }

        
    }

}
