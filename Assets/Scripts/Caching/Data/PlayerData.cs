using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class PlayerData : Data
    {
        Vector3 position;
        public Vector3 Position
        {
            get { return position; }
        }
        Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
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
        public PlayerData() { }

        public PlayerData(Vector3 position, Vector3 rotation, float health, string meleeWeaponCode, string fireWeaponCode)
        {
            this.position = position;
            this.rotation = rotation;
            this.health = health;
            this.meleeWeaponCode = meleeWeaponCode;
            this.fireWeaponCode = fireWeaponCode;
        }

        public override string Format()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", 
                position.x, position.y, position.z,
                rotation.x, rotation.y, rotation.z,
                health, meleeWeaponCode, fireWeaponCode);
        }

        public override void Parse(string data)
        {
            string[] s = data.Split(' ');
            position = new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
            rotation = new Vector3(float.Parse(s[3]), float.Parse(s[4]), float.Parse(s[5]));
            health = float.Parse(s[6]);
            meleeWeaponCode = s[7];
            fireWeaponCode = s[8];
        }
    }

}
