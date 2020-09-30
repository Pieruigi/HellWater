using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    public class ItemDispatcher : MonoBehaviour
    {
        public static ItemDispatcher Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Dispatch(Item item, int count = 1)
        {
            switch (item.Type)
            {
                case ItemType.Object:
                    Inventory.Instance.Add(item);
                    break;
                case ItemType.Ammo:
                case ItemType.Weapon:
                    Equipment.Instance.Add(item, count);
                    break;
                case ItemType.Document:
                    //Library.Instance.Add(item);
                    break;
            }

            

        }
    }

}
