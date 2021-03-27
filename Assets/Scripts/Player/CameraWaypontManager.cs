using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraWaypontManager : MonoBehaviour
    {
        List<CameraWaypoint> waypoints;
        GameObject player;

        public static CameraWaypontManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Load all the active waypoints.
            waypoints = new List<CameraWaypoint>(GameObject.FindObjectsOfType<CameraWaypoint>());

            player = PlayerController.Instance.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Always returns w1 as the closest waypoint ( unless it doesn't exist ); w2 is returned depending on the 
        /// player position between them w1 and w2.
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        public void FindTheTwoClosestWaypoints(out CameraWaypoint w1, out CameraWaypoint w2)
        {
            w1 = null;
            w2 = null;

            float w1DistSqr = 0;
            float w2DistSqr = 0;

            // Get W1.
            foreach (CameraWaypoint waypoint in waypoints)
            {
                // Distance by the player.
                float sqrDist = (player.transform.position - waypoint.transform.position).sqrMagnitude;

                CameraWaypoint w2Candidate = null;
                
                if (w1 == null || sqrDist < w1DistSqr)
                {
                    // Store the old value ( or null ). We need it to check w2.
                    w2Candidate = w1;
                    w1 = waypoint;
                    w1DistSqr = sqrDist;
                    
                    // Is the first iteration.
                    if (w2Candidate == null)
                        continue;
                }

                if (w2Candidate == null)
                    w2Candidate = waypoint;

                // Not a good candidate because player falls off the w2-w1 vector.
                if (Vector3.Dot((player.transform.position - w1.transform.position), w2Candidate.transform.position - w1.transform.position) < 0)
                    continue;

                sqrDist = (player.transform.position - w2Candidate.transform.position).sqrMagnitude;

                if (w2 == null || w2DistSqr > sqrDist)
                {
                    w2 = w2Candidate;
                    w2DistSqr = sqrDist;
                }

                


            }


        }
    }

}
