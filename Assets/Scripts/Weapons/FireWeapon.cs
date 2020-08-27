using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public class FireWeapon : Weapon
    {
        public UnityAction OnShoot;
        public UnityAction OnReload;
        public UnityAction OnOutOfAmmo;
        public UnityAction OnReloadInterrupted;

        [SerializeField]
        int maxMagazineAmmo;
 
        int leftAmmo = 0;

        int currentMagazineAmmo = 0;

        IShooter shooter;

        protected override void Awake()
        {
            shooter = GetComponent<IShooter>();
            base.Awake();
        }

        // Start is called before the first frame update
        
        public bool Shoot()
        {
           
            // You can't shoot without ammo
            if (IsEmpty())
            {
                //// No more ammo, so we can't reload
                //if (IsOutOfAmmo())
                //    OnOutOfAmmo?.Invoke();

                return false;
            }
                

            currentMagazineAmmo--;

            shooter.Shoot(this);

            OnShoot?.Invoke();

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
