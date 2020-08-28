using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{
    public enum ItemType { Object, Weapon, Ammo, Document }

    public class Item : ScriptableObject
    {
        [SerializeField]
        string code;

        [SerializeField]
        ItemType type;
        public ItemType Type
        {
            get { return type; }
        }

        [SerializeField]
        string description;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

