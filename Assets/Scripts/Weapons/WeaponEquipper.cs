using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class WeaponEquipper : MonoBehaviour
    {
        [Header("Fire Weapons")]
        // Heavy weapons can only be carried.
        [Tooltip("Only works for fire weapons.")]
        [SerializeField]
        bool heavy = false;

        [Tooltip("Only works for fire weapons.")]
        [SerializeField]
        bool primary = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void Add(Weapon weapon)
        {
           


        }

        public int Remove(object item, int count)
        {
            throw new System.NotImplementedException();
        }
    }

}
