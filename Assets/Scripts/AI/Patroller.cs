using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using HW.Interfaces;

namespace HW
{
    // Attach this class if you want an AI to patrol ( both for enemies and NPC )
    public class Patroller : MonoBehaviour, IBehaviour
    {
        [SerializeField]
        Transform spotGroup;

        [SerializeField]
        float stayInSpotMinTime = 5;

        [SerializeField]
        float stayInSpotMaxTime = 9;

        [SerializeField]
        SpeedClass speedClass = SpeedClass.VerySlow;
        //float speed = 2;

        Transform currentSpot;

        List<Transform> spots;
        DateTime lastSpot;

        bool loop = false;
        DateTime lastSpotTime;
        float stayTimer = 0;

        IMover mover;

        void Awake()
        {
            mover = GetComponentInParent<IMover>();
            mover.OnDestinationReached += HandleOnDestinationReached;

            // Fill the spot list
            spots = new List<Transform>();
            for (int i = 0; i < spotGroup.childCount; i++)
                spots.Add(spotGroup.GetChild(i));
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

            if (stayTimer >= 0)
            {
                stayTimer -= Time.deltaTime;

                if (stayTimer < 0)
                {
                    NextDestination();
                }
            }

            

        }

        public void StartBehaving()
        {
            loop = true;
            stayTimer = 0;
            mover.SetMaxSpeed(GameplayUtility.GetMovementSpeedValue(speedClass));
            NextDestination();
            
        }

        public void StopBehaving()
        {
            loop = false;
            
            //agent.ResetPath();
        }

        private void NextDestination()
        {
            // Remove the current spot from the available ones.
            List<Transform> l = spots.FindAll(s => s != currentSpot);

            currentSpot = l[UnityEngine.Random.Range(0, l.Count)];
            mover.MoveTo(currentSpot.position);
        }

        void HandleOnDestinationReached(IMover mover)
        {
            Debug.Log("Destination reached");
            stayTimer = UnityEngine.Random.Range(stayInSpotMinTime, stayInSpotMaxTime);
        }
    }

}
