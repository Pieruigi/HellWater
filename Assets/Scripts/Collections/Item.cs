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
        public string Code
        {
            get { return code; }
        }

        [SerializeField]
        ItemType type;
        public ItemType Type
        {
            get { return type; }
        }

        [SerializeField]
        string _name;
        public string Name
        {
            get { return _name; }
        }

        [SerializeField]
        string description;
        public string Description
        {
            get { return description; }
        }

        [SerializeField]
        Sprite icon;
        public Sprite Icon
        {
            get { return icon; }
        }

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

