using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW.CachingSystem
{
    public class PlayerCacher : Cacher
    {
        Health health;

        PlayerController playerController;

        protected override void Awake()
        {
            playerController = GetComponent<PlayerController>();
            health = GetComponent<Health>();

            base.Awake();
        }

        protected override void Init(string cacheValue)
        {
            playerController.LoadedFromCache = true;

            PlayerData data = new PlayerData();
            data.Parse(cacheValue);

            //transform.position = data.Position;
            //transform.eulerAngles = data.Rotation;

            // Get the spawner
            Spawner spawner = GetComponent<Spawner>();
            
            // We only need to set the spawn point id, the spawner will take care to spawn the player
            spawner.SpawnPointId = data.SpawnPointId;
            
            health.Init(data.Health);

            // Load resources
            List<Item> items = new List<Item>(Resources.LoadAll<Item>(Constants.ResourcesFolderEquipment));

            // Init weapons
            if (!"-".Equals(data.MeleeWeaponCode))
                playerController.EquipWeapon(items.Find(i => i.Code == data.MeleeWeaponCode));

            if (!"-".Equals(data.FireWeaponCode))
                playerController.EquipWeapon(items.Find(i => i.Code == data.FireWeaponCode));

            // Holster weapon
            playerController.HolsterWeapon();
        }

        protected override string GetCacheValue()
        {
            string fwCode = playerController.FireWeapon ? playerController.FireWeapon.Item.Code : "-";
            string mwCode = playerController.FireWeapon ? playerController.MeleeWeapon.Item.Code : "-";
            
            PlayerData data = new PlayerData(GetComponent<Spawner>().SpawnPointId,
                health.CurrentHealth, mwCode, fwCode);

            return data.Format();
        }

       
    }

}
