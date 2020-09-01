using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW.CachingSystem
{
    public class PlayerCacher : Cacher
    {
        PlayerController playerController;
        Health health;

        protected override void Awake()
        {
            playerController = GetComponent<PlayerController>();
            health = GetComponent<Health>();

            base.Awake();
        }

        protected override void Init(string cacheValue)
        {
            PlayerData data = new PlayerData();
            data.Parse(cacheValue);

            transform.position = data.Position;
            transform.eulerAngles = data.Rotation;
            health.Init(data.Health);

            // Load resources
            List<Item> items = new List<Item>(Resources.LoadAll<Item>(Constants.ResourcesFolderEquipment));

            if (!"-".Equals(data.MeleeWeaponCode))
                playerController.EquipWeapon(items.Find(i => i.Code == data.MeleeWeaponCode));

            if (!"-".Equals(data.FireWeaponCode))
                playerController.EquipWeapon(items.Find(i => i.Code == data.FireWeaponCode));
        }

        protected override string GetCacheValue()
        {
            string fwCode = playerController.FireWeapon ? playerController.FireWeapon.Item.Code : "-";
            string mwCode = playerController.FireWeapon ? playerController.MeleeWeapon.Item.Code : "-";

            PlayerData data = new PlayerData(transform.position, transform.eulerAngles,
                health.CurrentHealth, mwCode, fwCode);

            return data.Format();
        }

       
    }

}
