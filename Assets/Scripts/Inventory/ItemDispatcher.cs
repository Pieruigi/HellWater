using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    public class ItemDispatcher
    {
        private static ItemDispatcher instance;

        public static ItemDispatcher Instance 
        {
            get { 
                if (instance == null) 
                    instance = new ItemDispatcher();
                return instance;
            }  
        }

        //public static void Add(Item item, int amount = 1)
        //{
        //    switch (item.Type)
        //    {
        //        case ItemType.Weapon:
        //        case ItemType.Ammo:

        //            Equipment.Instance.Add(item, amount);
        //            break;

        //        case ItemType.Object:
        //            Inventory.Instance.Add(item);
        //            break;
        //    }
        //}
    }

}
