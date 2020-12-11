using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.Events;

namespace HW
{
    
    

    public class Equipment : MonoBehaviour
    {
        public UnityAction<Weapon> WeaponAdded;
        public UnityAction<Weapon> WeaponRemoved;

        FireWeapon primaryWeapon, secondaryWeapon;
        public FireWeapon PrimaryWeapon
        {
            get { return primaryWeapon; }
        }

        public FireWeapon SecondaryWeapon
        {
            get { return secondaryWeapon; }
        }

        MeleeWeapon meleeWeapon;
        public MeleeWeapon MeleeWeapon
        {
            get { return meleeWeapon; }
        }

        //List<Item> weapons = new List<Item>();

        //AmmoData[] ammonitions = new AmmoData[5];

        int[] ammonitions = new int[5];

        int[] maximumAmmonitions = new int[] { 150, 40, 20, 100, 20 };

        //Item primaryWeapon              

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

            //// Get weapons
            //foreach(Item item in weapons)
            //{
            //    codes.Add(item.Code);
            //    amounts.Add(1);
            //}

            // Get ammonitions
            //foreach(AmmoData ad in ammonitions)
            //{
            //    codes.Add(ad.Ammo.Code);
            //    amounts.Add(ad.Amount);
            //}
        }

        //public void Add(Item item, int count = 1)
        //{
        //    if (item.Type != ItemType.Weapon && item.Type != ItemType.Ammo)
        //        throw new System.Exception("Invalid type '" + item.Type + "' for " + item.name);

        //    switch (item.Type)
        //    {
        //        case ItemType.Weapon:
        //            AddWeapon(item);
        //            break;
        //        case ItemType.Ammo:
        //            AddAmmonitions(item, count);
        //            break;
        //    }
        //}

        public void AddWeapon(Weapon weapon)
        {
            Weapon oldWeapon = null;
            if(weapon.GetType() == typeof(MeleeWeapon))
            {
                // It's melee weapon.
                RemoveMeleeWeapon();
                meleeWeapon = weapon as MeleeWeapon;
            }
            else
            {
                if(weapon.GetType() == typeof(FireWeapon))
                {
                    // We must check wheter is primary or secondary weapon.
                    FireWeapon newWeapon = weapon as FireWeapon;
                    if(newWeapon.Group == FireWeaponGroup.Primary)
                    {
                        // It's primary weapon.
                        RemovePrimaryWeapon();
                        primaryWeapon = newWeapon;
                    }
                    else
                    {
                        // It's secondary weapon.
                        RemoveSecondaryWeapon();
                        secondaryWeapon = newWeapon;
                    }
                }

                
            }

            // Let know the other that a weapon has been added.
            WeaponAdded?.Invoke(weapon);
        }

        public void RemovePrimaryWeapon()
        {
            if (primaryWeapon == null)
                return;

            Weapon tmp = primaryWeapon;
            primaryWeapon = null;

            WeaponRemoved?.Invoke(tmp);
        }

        public void RemoveSecondaryWeapon()
        {
            if (secondaryWeapon == null)
                return;

            Weapon tmp = secondaryWeapon;
            secondaryWeapon = null;

            WeaponRemoved?.Invoke(tmp);
        }

        public void RemoveMeleeWeapon()
        {
            if (meleeWeapon == null)
                return;

            Weapon tmp = meleeWeapon;
            meleeWeapon = null;

            WeaponRemoved?.Invoke(tmp);

        }

        //private void AddAmmonitions(Item item, int amount)
        //{
        //    if (item.Type != ItemType.Ammo)
        //        throw new System.Exception("AddAmmonitions() can't be called with param of type " + item.Type + ".");

        //    // Look for this ammo in the equipment
        //    AmmoData data = ammonitions.Find(ad => ad.Ammo == item);

        //    // If ammo doesn't exist the create new data
        //    if (data == null)
        //    {
        //        data = new AmmoData(item);
        //        ammonitions.Add(data);
        //    }

        //    // Increase data
        //    data.IncreaseAmmo(amount);

        //}

        /// <summary>
        /// Add some ammo.
        /// </summary>
        /// <param name="ammoType"></param>
        /// <param name="amount"></param>
        /// <returns>The number of ammo added.</returns>
        public int AddAmmonition(int ammoType, int amount)
        {
            int count = ammonitions[ammoType];
            int maxCount = maximumAmmonitions[ammoType];

            // No more room
            if (count == maxCount)
                return 0;

            int ret = amount;

            if (count + amount > maxCount)
            {
                ret = maxCount - count;
            }

            count += ret;
            ammonitions[ammoType] = count;

            return ret;
        }

        public int GetNumberOfAmmonitions(int ammoType)
        {
            return ammonitions[ammoType];
        }

        public void RemoveAmmonitions(int ammoType, int amount)
        {
            int count = ammonitions[ammoType];
            count = Mathf.Max(0, count - amount);

            ammonitions[ammoType] = count;

        }

       
    }

}
