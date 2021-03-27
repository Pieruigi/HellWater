using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.Events;

namespace HW
{
    
    

    public class Equipment : MonoBehaviour
    {
        public UnityAction<Weapon> OnWeaponAdded;
        public UnityAction<Weapon> OnWeaponRemoved;
        

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

       
        public bool IsPrimary(FireWeapon fireWeapon)
        {
            return primaryWeapon == fireWeapon;
        }
        
        public void AddMeleeWeapon(MeleeWeapon weapon)
        {
            RemoveMeleeWeapon();
            meleeWeapon = weapon as MeleeWeapon;

            OnWeaponAdded?.Invoke(weapon);
        }

        public void AddFireWeapon(FireWeapon weapon)
        {
            if(weapon.HolsterId == FireWeaponHolsterId.Primary)
            {
                RemovePrimaryFireWeapon();
                primaryWeapon = weapon;
            }
            else
            {
                if(weapon.HolsterId == FireWeaponHolsterId.Secondary)
                {
                    RemoveSecondaryFireWeapon();
                    secondaryWeapon = weapon;
                }
            }    

            OnWeaponAdded?.Invoke(weapon);
        }


        void RemovePrimaryFireWeapon()
        {
            if (primaryWeapon == null)
                return;

            FireWeapon tmp = primaryWeapon;
            primaryWeapon = null;

            // Let know the others that a weapon has been removed.
            OnWeaponRemoved?.Invoke(tmp);
        }

        void RemoveSecondaryFireWeapon()
        {
            if (secondaryWeapon == null)
                return;

            FireWeapon tmp = secondaryWeapon;
            secondaryWeapon = null;

            // Let know the others that a weapon has been removed.
            OnWeaponRemoved?.Invoke(tmp);
        }

        void RemoveMeleeWeapon()
        {
            if (meleeWeapon == null)
                return;

            MeleeWeapon tmp = meleeWeapon;
            meleeWeapon = null;

            // Let know the others that a weapon has been removed.
            OnWeaponRemoved?.Invoke(tmp);

        }



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

            // Check the maximum amount.
            int ret = Mathf.Min(amount, maxCount-count);

            // Update ammo.
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
