using HW.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class InventoryCacher : Cacher
    {
        Inventory inventory;

        protected override void Awake()
        {
            // Get the inventory component.
            inventory = GetComponent<Inventory>();

            base.Awake();
        }

        protected override string GetCacheValue()
        {
            // Create data.
            ObjectListData old = new ObjectListData();
            
            // Add every object from the inventory.
            foreach(Item item in inventory.GetItems())
            {
                old.AddObject(item.Code, 1);
            }

            return old.Format();
            
        }

        protected override void Init(string cacheValue)
        {
            // Parse string data.
            ObjectListData old = new ObjectListData();
            old.Parse(cacheValue);

            // Get all item resources.
            List<Item> items = new List<Item>(Resources.LoadAll<Item>(Constants.ResourceFolderInventory));

            // Loop thorugh all the objects.
            foreach (ObjectListData.ObjectInfo o in old.Objects)
            {
                // Get the item from the collection.
                Item item = items.Find(i => i.Code == o.Code);
                inventory.Add(item);
            }
        }

        
    }

}
