using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FireWeapon : Weapon
    {
        [SerializeField]
        int maxMagazineAmmo;
 
        int leftAmmo = 0;

        int currentMagazineAmmo = 0;
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Shoot()
        {
            if (IsEmpty())
                return false;

            currentMagazineAmmo--;

            return true;
        }

        public bool Reload()
        {
            if (IsFull())
                return false;

            if (IsOutOfAmmo())
                return false;

            int count = maxMagazineAmmo - currentMagazineAmmo;
            if(leftAmmo >= count)
            {
                leftAmmo -= count;
            }
            else
            {
                count = leftAmmo;
                leftAmmo = 0;
            }

            currentMagazineAmmo += count;

            return true;
        }

        public bool IsFull()
        {
            return currentMagazineAmmo > 0;
        }

        public bool IsEmpty()
        {
            return currentMagazineAmmo == 0;
        }

        public bool IsOutOfAmmo()
        {
            return leftAmmo == 0;
        }

        public void AddAmmo(int amount)
        {
            leftAmmo += amount;
        }
    }

}
