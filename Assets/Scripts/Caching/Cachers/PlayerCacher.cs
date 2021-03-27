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

        
            // Get the spawner
            Spawner spawner = Spawner.GetSpawner(transform);
            
            // Current spawn id
            spawner.SpawnPointId = data.SpawnPointId;
            
            
            health.Init(data.Health);

            // Load resources
            List<Item> items = new List<Item>(Resources.LoadAll<Item>(Constants.ResourceFolderEquipment));

            // Init weapons
            if (!"-".Equals(data.MeleeWeaponCode))
                playerController.EquipWeapon(items.Find(i => i.Code == data.MeleeWeaponCode));

            if (!"-".Equals(data.FireWeaponCode))
                playerController.EquipWeapon(items.Find(i => i.Code == data.FireWeaponCode));

            // Holster weapon
            playerController.HolsterWeapon();

            // Get all player fire weapons ( even the not yet available ones )
            List<FireWeapon> weapons = new List<FireWeapon>(GetComponentsInChildren<FireWeapon>());

            // Add ammonitions
            for(int i=0; i<weapons.Count; i++)
            {
                weapons[i].Init(data.LoadedAmmonitions[i]);
            }

        }

        protected override string GetCacheValue()
        {
            string fwCode = "";//playerController.FireWeapon ? playerController.FireWeapon.Item.Code : "-";
            string mwCode = "";// playerController.FireWeapon ? playerController.MeleeWeapon.Item.Code : "-";

            // Loaded ammonitions for every fire weapon ( even the not yet available ones )
            List<int> ammos = new List<int>();
            List<FireWeapon> weapons = new List<FireWeapon>(GetComponentsInChildren<FireWeapon>());
            foreach(FireWeapon weapon in weapons)
            {
                ammos.Add(weapon.NumberOfLoadedAmmo);
            }

            Spawner playerSpawner = Spawner.GetSpawner(transform);

            PlayerData data = new PlayerData(playerSpawner.SpawnPointId,
                health.CurrentHealth, mwCode, fwCode, ammos);

            return data.Format();
        }

       
    }

}
