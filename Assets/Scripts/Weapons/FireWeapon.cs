using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;
using HW.Collections;

namespace HW
{
    // Type of bullets
    public enum AmmonitionType { GunBullet, Magnum, Shells, RifleBullet, SniperBullet }

    public enum FireWeaponHolsterId { None, Primary, Secondary }
    
    public class FireWeapon : Weapon
    {
        public static readonly float GlobalAimingRange = 14;

        public UnityAction OnShoot;
        public UnityAction OnReload;
        public UnityAction OnOutOfAmmo;
        public UnityAction OnReloadInterrupted;

        [SerializeField]
        int maxMagazineAmmo;

        [SerializeField]
        AmmonitionType ammonitionType;
        public AmmonitionType AmmonitionType
        {
            get { return ammonitionType; }
        }

        [SerializeField]
        FireWeaponHolsterId holsterId;
        public FireWeaponHolsterId HolsterId
        {
            get { return holsterId; }
        }

        int currentMagazineAmmo = 0;
        public int NumberOfLoadedAmmo
        {
            get { return currentMagazineAmmo; }
        }

        IShooter shooter;

        /**
         * Only for accuracy system.
         * How it works:
         *  0 - tooCloseRange: you get some accuracy penalty
         *  tooCloseRange - range: no penalty, maximum accuracy
         *  range - tooFarRange: you get some accuracy penalty
         *  > tooFarRange: you have no chance to hit anything
         *  */


        // Inside this range you get accuracy penalty
        [SerializeField]
        float tooCloseDistance = 0;
        public float TooCloseDistance
        {
            get { return tooCloseDistance; }
        }

        // If you 
        [SerializeField]
        float tooFarDistance;
        public float TooFarDistance
        {
            get { return tooFarDistance; }
        }

        protected override void Awake()
        {
            shooter = GetComponent<IShooter>();
            base.Awake();
        }

        
        
        public static float GetAccuracyPenalty(FireWeapon weapon, float distance)
        {
            if (weapon.tooCloseDistance > distance)
                return 0.5f;

            if (distance > weapon.Range && distance < weapon.tooFarDistance)
                return 0.5f;

            if (distance > weapon.tooFarDistance)
                return 1;

            return 0;
        }

        public void Init(int loadedAmmonitions)
        {
            currentMagazineAmmo = loadedAmmonitions;
        }

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

            int leftAmmo = Equipment.Instance.GetNumberOfAmmonitions((int)ammonitionType);
           
            if(leftAmmo < count)
                count = leftAmmo;

            Equipment.Instance.RemoveAmmonitions((int)ammonitionType, count);

            currentMagazineAmmo += count;

            //OnReload?.Invoke();

            return true;
        }

        public bool IsFull()
        {
            //return currentMagazineAmmo > 0;
            return currentMagazineAmmo == maxMagazineAmmo;
        }

        public bool IsEmpty()
        {
            return currentMagazineAmmo == 0;
        }

        public bool IsOutOfAmmo()
        {
            return Equipment.Instance.GetNumberOfAmmonitions((int)ammonitionType) == 0;
        }

        //public void AddAmmo(int amount)
        //{
        //    Equipment.Instance.AddAmmonitions(ammonition, amount);
        //    //leftAmmo += amount;
        //}
    }

}
