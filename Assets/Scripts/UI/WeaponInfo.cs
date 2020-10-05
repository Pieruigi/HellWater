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
        TMPro.TMP_Text ammoCount;

        Weapon currentWeapon = null;
        string ammoTextFormat = "{0}/{1}";

        private void Awake()
        {
            ResetAll();
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController.Instance.OnSetCurrentWeapon += HandleOnWeaponSet;
            PlayerController.Instance.OnResetCurrentWeapon += HandleOnWeaponReset;
        }

        // Update is called once per frame
        void Update()
        {

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

                ammoCount.enabled = true;
                CountAmmo();
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
            ammoCount.text = string.Format(ammoTextFormat, (currentWeapon as FireWeapon).NumberOfLoadedAmmo, totalAmmo);
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
            ammoCount.enabled = false;
        }
    }

}
