using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum CameraOrientation { ToNorth, ToSouth, ToEast, ToWest }

    [ExecuteAlways]
    public class PlayerCamera : MonoBehaviour
    {
        

        // Does the camera follow the player?
        [SerializeField]
        bool followPlayer = false;

        [SerializeField]
        [Tooltip("Offset from the player")]
        Vector3 offsetFromPlayer;

        // Does the camera always look at the player?
        [SerializeField]
        bool lookAtPlayer = false;



        [SerializeField]
        float smoothTime = 0.5f;
        
        public static PlayerCamera Instance { get; private set; }

        GameObject player;

        Vector3 vel, angVel;

        

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
            player = PlayerController.Instance.gameObject;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 desiredPosition = transform.position, desiredForward;
            
            if (followPlayer)
            {
                desiredPosition = player.transform.position;
                desiredPosition += offsetFromPlayer;
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, smoothTime);
            }


            if (lookAtPlayer)
            {
                desiredForward = player.transform.position - desiredPosition;
                transform.forward = Vector3.SmoothDamp(transform.forward, desiredForward, ref angVel, smoothTime);


            }

            

        }

        

     

        
    }

}
