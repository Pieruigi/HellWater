using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class Striker : MonoBehaviour, IStriker
    {
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Strike(MeleeWeapon weapon)
        {
            throw new System.NotImplementedException();
        }
    }

}
