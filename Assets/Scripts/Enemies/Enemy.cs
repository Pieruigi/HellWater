using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class Enemy : MonoBehaviour, ITargetable, IHitable
    {
        public void Hit(HitInfo hitInfo)
        {
            Debug.Log("Receiving HitInfo:" + hitInfo);
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
