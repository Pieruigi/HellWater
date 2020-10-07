using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        WeaponInfo weaponInfo;

        private void Awake()
        {
            
            panel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController.Instance.OnSetCurrentWeapon += HandleOnSetCurrentWeapon;
            PlayerController.Instance.OnResetCurrentWeapon += HandleOnResetCurrentWeapon;

            weaponInfo.Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnSetCurrentWeapon(Weapon weapon)
        {
            panel.SetActive(true);
        }

        void HandleOnResetCurrentWeapon()
        {
            panel.SetActive(false);
        }
    }

}
