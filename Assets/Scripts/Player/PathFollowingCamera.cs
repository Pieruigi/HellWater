using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PathFollowingCamera : MonoBehaviour
    {
        [SerializeField]
        bool lookAtPlayer = false;

        // Waypoints.
        CameraWaypoint w1, w2;
        

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CameraWaypontManager.Instance.FindTheTwoClosestWaypoints(out w1, out w2);
            Debug.LogFormat("w1:{0}; w2:{1}", w1, w2);

            Debug.LogFormat("w1.pos:" + w1.GetDesiredCameraPosition());
            if(w2)
                Debug.LogFormat("w2.pos:" + w2.GetDesiredCameraPosition());

            Vector3 desiredPosition;
            if (!w2)
            {
                // W2 is null so the only position is given by W1.
                desiredPosition = w1.GetDesiredCameraPosition();
            }
            else
            {
                // W2 is not null, so we need to lerp between W1 and W2 depending on the player position; in this
                // case we need to project the player on the W1-W2 vector, and then apply lerp computation.
                
            }


        }
    }

}
