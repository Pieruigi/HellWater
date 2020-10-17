using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    /**
     * This class manages all and only the objects the player can carry with him and that must be used while playing;
     * */
    public class Inventory : MonoBehaviour
    {
        // All the items 
        List<Item> items = new List<Item>();

        public static Inventory Instance { get; private set; }
        private void Awake()
        {
            if(!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Adds a new item
        public void Add(Item item)
        {
            // Item must be an object
            if (item.Type != ItemType.Object)
                return;

            // No duplicates
            if (items.Contains(item))
                return;

            // Ok add
            items.Add(item);
        }

        // Removes an existing item
        public void Remove(Item item)
        {
            items.Remove(item);
        }

        // Returns true if the item passed as param exists, otherwise returns false
        public bool Contains(Item item)
        {
            if (item.Type != ItemType.Object)
                return false;

            return items.Contains(item);
        }

        // Returns the list of item as readeble only
        public IList<Item> GetItems()
        {
            return items.AsReadOnly();
        }
    }

}
