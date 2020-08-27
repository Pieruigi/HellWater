using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public class FireWeapon : Weapon
    {
        public static readonly float GlobalAimingRange = 14;

        public UnityAction OnShoot;
        public UnityAction OnReload;
        public UnityAction OnOutOfAmmo;
        public UnityAction OnReloadInterrupted;

        [SerializeField]
        int maxMagazineAmmo;
 
        int leftAmmo = 0;

        int currentMagazineAmmo = 0;

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

        // Start is called before the first frame update
        
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
