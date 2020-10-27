using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public class MeleeWeapon : Weapon
    {
        public UnityAction<bool, Weapon> OnHit;
        public UnityAction<Weapon> OnStrike;

        IStriker striker;

        protected override void Awake()
        {
            striker = GetComponentInParent<IStriker>();
            base.Awake();
        }

        public bool Strike()
        {
            striker.Strike(this);

            OnStrike?.Invoke(this);

            return true;
        }
     
    }

}
