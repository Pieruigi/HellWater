using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    public class Equipment : MonoBehaviour
    {
        [System.Serializable]
        private class AmmoData
        {
            Item ammo;
            public Item Ammo
            {
                get { return ammo; }
            }
            int count;
            public int Amount
            {
                get { return count; }
            }

            public AmmoData(Item ammo)
            {
                this.ammo = ammo;
            }

            public void IncreaseAmmo(int amount)
            {
                this.count += amount;
            }

            public void DecreaseAmmo(int amount)
            {
                count = Mathf.Max(0, count - amount);
            }
        }

        
        List<Item> weapons = new List<Item>();

        List<AmmoData> ammonitions = new List<AmmoData>();



        public static Equipment Instance { get; private set; }
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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Used by the caching system
        public void GetDataForCaching(out List<string> codes, out List<int> amounts)
        {
            codes = new List<string>();
            amounts = new List<int>();

            // Get weapons
            foreach(Item item in weapons)
            {
                codes.Add(item.Code);
                amounts.Add(1);
            }

            // Get ammonitions
            foreach(AmmoData ad in ammonitions)
            {
                codes.Add(ad.Ammo.Code);
                amounts.Add(ad.Amount);
            }
        }

        public void Add(Item item, int count = 1)
        {
            if (item.Type != ItemType.Weapon && item.Type != ItemType.Ammo)
                throw new System.Exception("Invalid type '" + item.Type + "' for " + item.name);

            switch (item.Type)
            {
                case ItemType.Weapon:
                    AddWeapon(item);
                    break;
                case ItemType.Ammo:
                    AddAmmonitions(item, count);
                    break;
            }
        }

        private void AddWeapon(Item item)
        {
            if (item.Type != ItemType.Weapon)
                throw new System.Exception("AddWeapon() can't be called with param of type " + item.Type + ".");

            weapons.Add(item);

            // The first two weapons must also be equipped
            if(weapons.Count < 3)
            {
                PlayerController.Instance.EquipWeapon(item);
                PlayerController.Instance.HolsterWeapon();
            }
                

            
        }

        private void AddAmmonitions(Item item, int amount)
        {
            if (item.Type != ItemType.Ammo)
                throw new System.Exception("AddAmmonitions() can't be called with param of type " + item.Type + ".");

            // Look for this ammo in the equipment
            AmmoData data = ammonitions.Find(ad => ad.Ammo == item);

            // If ammo doesn't exist the create new data
            if (data == null)
            {
                data = new AmmoData(item);
                ammonitions.Add(data);
            }

            // Increase data
            data.IncreaseAmmo(amount);
            
        }

        public int GetNumberOfAmmonitions(Item item)
        {
            if (item.Type != ItemType.Ammo)
                throw new System.Exception("GetNumberOfAmmonitions() can't be called with param of type " + item.Type + ".");

            AmmoData data = ammonitions.Find(ad => ad.Ammo == item);
            if (data == null)
                return 0;

            return data.Amount;
        }

        public void RemoveAmmonitions(Item item, int amount)
        {
            if (item.Type != ItemType.Ammo)
                throw new System.Exception("RemoveAmmonitions() can't be called with param of type " + item.Type + ".");

            AmmoData data = ammonitions.Find(ad => ad.Ammo == item);
            if (data == null)
                return;

            data.DecreaseAmmo(amount);

            if (data.Amount == 0)
                ammonitions.Remove(data);
        }

       
    }

}
