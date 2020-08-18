using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        float damage;
        public float Damage
        {
            get { return damage; }
        }

        [SerializeField]
        float range;
        public float Range
        {
            get { return range; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
