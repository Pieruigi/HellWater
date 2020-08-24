using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.AI;

namespace HW
{
    public class Enemy : MonoBehaviour, ITargetable, IHitable
    {
        [SerializeField]
        MonoBehaviour idleBehaviour;

        // The target this enemy is looking for ( can be both the player and any NPC )
        Transform target;

        bool engaged = false;

        Vector3 initialPosition;
        NavMeshAgent agent;

        

        void Awake()
        {
            initialPosition = transform.position;
            agent = GetComponent<NavMeshAgent>();
        }


        // Start is called before the first frame update
        void Start()
        {
            if (idleBehaviour != null)
                idleBehaviour.GetComponent<IBehaviour>().StartBehaving();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #region INTERFACES IMPLEMENTATION
        public void Hit(HitInfo hitInfo)
        {
            Debug.Log("Receiving HitInfo:" + hitInfo);

            if (!engaged)
                Engage();
        }
        #endregion


        void Engage()
        {
            engaged = true;
            Debug.Log("Engaged");
        }

        void Idle()
        {

        }
    }

}
