using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PathFollowingCamera : MonoBehaviour
    {
        //[SerializeField]
        //bool lookAtPlayer = false;
        
        [Header("*** DEBUG ***")]
        [SerializeField]
        bool noLerp = false;

        // Waypoints.
        CameraWaypoint w1, w2;

        GameObject player;

        Vector3 velocity;
        Vector3 angularVelocity;

        float smoothTime = 0.25f;

        public static PathFollowingCamera Instance { get; private set; }
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

#if !UNITY_EDITOR
noLerp = false;
#endif
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            player = PlayerController.Instance.gameObject;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            CameraWaypontManager.Instance.FindTheTwoClosestWaypoints(out w1, out w2);
            Debug.LogFormat("w1:{0}; w2:{1}", w1, w2);

            Debug.LogFormat("w1.pos:" + w1.GetDesiredCameraPosition());
            if(w2)
                Debug.LogFormat("w2.pos:" + w2.GetDesiredCameraPosition());

            Vector3 desiredPosition;
            Vector3 desiredForward;
            if (!w2 || noLerp)
            {
                // W2 is null so the only position is given by W1.
                desiredPosition = w1.GetDesiredCameraPosition();
                desiredForward = w1.GetDesiredCameraForward();
            }
            else
            {
                // W2 is not null, so we need to lerp between W1 and W2 depending on the player position; in this
                // case we need to project the player on the W1-W2 vector, and then apply lerp computation.
                // Get the player projection.
                Vector3 w1ToW2 = w2.transform.position - w1.transform.position;
                Vector3 w1ToPlayer = player.transform.position - w1.transform.position;
                Vector3 w1Proj = Vector3.Project(w1ToPlayer, w1ToW2);

                // The interpolation factor.
                float t = w1Proj.magnitude / w1ToW2.magnitude;

                desiredPosition = Vector3.Lerp(w1.GetDesiredCameraPosition(), w2.GetDesiredCameraPosition(), t);
                desiredForward = Vector3.Lerp(w1.GetDesiredCameraForward(), w2.GetDesiredCameraForward(), t);
            }

            Debug.LogFormat("LerpedPosition:" + desiredPosition);
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.forward = Vector3.SmoothDamp(transform.forward, desiredForward, ref angularVelocity, smoothTime);

        }
    }

}
