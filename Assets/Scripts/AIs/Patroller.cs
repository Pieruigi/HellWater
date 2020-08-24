using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace HW
{
    // Attach this class if you want an AI to patrol ( both for enemies and NPC )
    public class Patroller : MonoBehaviour, IBehaviour
    {
        [SerializeField]
        List<Transform> spots;

        [SerializeField]
        float stayInSpotMinTime = 5;

        [SerializeField]
        float stayInSpotMaxTime = 9;

        NavMeshAgent agent;

        Transform currentSpot;

        DateTime lastSpot;

        bool loop = false;
        

        void Awake()
        {
            agent = GetComponentInParent<NavMeshAgent>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!loop)
                return;

            // Destination reached
            if (agent.remainingDistance == 0)
                Debug.Log("ASSSSSSSSSSSSSSSSSSSSSSTTTTTTTTTTTTTOOOOOOOOOOPPPPPPPPPPPPP");
        }

        public void StartBehaving()
        {
            loop = true;
            currentSpot = spots[UnityEngine.Random.Range(0, spots.Count)];
            agent.SetDestination(currentSpot.position);
            //lastSpot = 
        }

        public void StopBehaving()
        {
            loop = false;
            agent.ResetPath();
        }

      
    }

}
