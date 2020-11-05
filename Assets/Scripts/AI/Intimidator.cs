using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class Intimidator : MonoBehaviour, IFighter
    {
        [SerializeField]
        float attackRange = 10f;

        public bool Fight(Transform target)
        {
            // There is no fight at all.
            target.GetComponent<PlayerController>().Surrender();

            return true;
        }

        public float GetFightingRange()
        {
            return attackRange;
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
