using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class PlayerData : Data
    {
        Vector3 position;
        Vector3 rotation;
        float health;
        int meleeWeapon = -1;
        int fireWeapon = -1;

        public PlayerData(Vector3 position, Vector3 rotation, float health, int meleeWeapon, int fireWeapon)
        {
            this.position = position;
            this.rotation = rotation;
            this.health = health;
            this.meleeWeapon = meleeWeapon;
            this.fireWeapon = fireWeapon;
        }

        public override string Format()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", 
                position.x, position.y, position.z,
                rotation.x, rotation.y, rotation.z,
                health, meleeWeapon, fireWeapon);
        }

        public override void Parse(string data)
        {
            string[] s = data.Split(' ');
            position = new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
            rotation = new Vector3(float.Parse(s[3]), float.Parse(s[4]), float.Parse(s[5]));
            health = float.Parse(s[6]);
            meleeWeapon = int.Parse(s[7]);
            fireWeapon = int.Parse(s[8]);
        }
    }

}
