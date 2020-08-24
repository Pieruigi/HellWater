using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class MeleeWeapon : Weapon
    {

        IStriker striker;

        protected override void Awake()
        {
            striker = GetComponentInParent<IStriker>();
            base.Awake();
        }

        public bool Strike()
        {
            striker.Strike(this);

            return true;
        }
     
    }

}
