using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HW.Collections;


namespace HW.UI
{
    public class WeaponInfo : MonoBehaviour
    {
        [SerializeField]
        Image weaponImage;

        [SerializeField]
        TMPro.TMP_Text ammoFireWeaponCount;

        [SerializeField]
        TMPro.TMP_Text ammoMeleeWeaponInfinite;

        Weapon currentWeapon = null;
        string ammoFireWeaponTextFormat = "{0}/{1}";
        //string infiniteChar = "∞";

        private void Awake()
        {
            ResetAll();
        }

       

        public void Init()
        {
            PlayerController.Instance.OnSetCurrentWeapon += HandleOnWeaponSet;
            PlayerController.Instance.OnResetCurrentWeapon += HandleOnWeaponReset;
        }


        void HandleOnWeaponSet(Weapon weapon)
        {
            // Already set
            if (weapon == currentWeapon)
                return;

            // ???
            if (weapon == null)
            {
                ResetAll();
                return;
            }

            ResetAll();

            // Set weapon
            currentWeapon = weapon;
            weaponImage.enabled = true;
            weaponImage.sprite = currentWeapon.Item.Icon;

            // Is fire weapon?
            if(weapon.GetType() == typeof(FireWeapon))
            {
                (currentWeapon as FireWeapon).OnShoot += HandleOnShoot;
                (currentWeapon as FireWeapon).OnReload += HandleOnReload;

                ammoFireWeaponCount.enabled = true;
                CountAmmo();
            }
            else
            {
                ammoMeleeWeaponInfinite.enabled = true;
            }
           
        }

        void HandleOnWeaponReset()
        {
            ResetAll();
        }

        void HandleOnShoot()
        {
            CountAmmo();
        }

        void HandleOnReload()
        {
            CountAmmo();
        }

        void CountAmmo()
        {
            int totalAmmo = Equipment.Instance.GetNumberOfAmmonitions((currentWeapon as FireWeapon).Ammonition);
            ammoFireWeaponCount.text = string.Format(ammoFireWeaponTextFormat, (currentWeapon as FireWeapon).NumberOfLoadedAmmo, totalAmmo);
        }

       void ResetAll()
        {
            if (currentWeapon)
            {
                if(currentWeapon.GetType().Equals(typeof(FireWeapon)))
                {
                    (currentWeapon as FireWeapon).OnShoot -= HandleOnShoot;
                    (currentWeapon as FireWeapon).OnReload -= HandleOnReload;
                }
            }

            currentWeapon = null;
            weaponImage.enabled = false;
            ammoFireWeaponCount.enabled = false;
            ammoMeleeWeaponInfinite.enabled = false;
        }
    }

}
