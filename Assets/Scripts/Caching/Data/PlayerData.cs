using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class PlayerData : Data
    {
        //Vector3 position;
        //public Vector3 Position
        //{
        //    get { return position; }
        //}
        //Vector3 rotation;
        //public Vector3 Rotation
        //{
        //    get { return rotation; }
        //}
        int spawnPointId;
        public int SpawnPointId
        {
            get { return spawnPointId; }
        }

        float health;
        public float Health
        {
            get { return health; }
        }
        string meleeWeaponCode;
        public string MeleeWeaponCode
        {
            get { return meleeWeaponCode; }
        }
        string fireWeaponCode;
        public string FireWeaponCode
        {
            get { return fireWeaponCode; }
        }

        
        List<int> loadedAmmonitions;
        public List<int> LoadedAmmonitions
        {
            get { return loadedAmmonitions; }
        }

        public PlayerData() { }

        public PlayerData(int spawnPointId, float health, string meleeWeaponCode, string fireWeaponCode, List<int> loadedAmmonitions)
        {
            this.spawnPointId = spawnPointId;
            this.health = health;
            this.meleeWeaponCode = meleeWeaponCode;
            this.fireWeaponCode = fireWeaponCode;
            this.loadedAmmonitions = loadedAmmonitions;
        }

        public override string Format()
        {
            string ret = string.Format("{0} {1} {2} {3}", spawnPointId, health, meleeWeaponCode, fireWeaponCode);

            // We add the ammonitions loaded for each weapon 
            for(int i=0; i<loadedAmmonitions.Count; i++)
            {
                ret += " " + loadedAmmonitions[i];
            }

            return ret;
        }

        public override void Parse(string data)
        {
            string[] s = data.Split(' ');
            spawnPointId = int.Parse(s[0]);
            health = float.Parse(s[1]);
            meleeWeaponCode = s[2];
            fireWeaponCode = s[3];
            
            // Load ammonitions loaded in each weapon
            loadedAmmonitions = new List<int>();
            for(int i=4; i<s.Length ; i++)
            {
                loadedAmmonitions.Add(int.Parse(s[i]));
            }
        }
    }

}
