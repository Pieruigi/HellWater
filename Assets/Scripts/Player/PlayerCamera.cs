using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    
    [ExecuteAlways]
    public class PlayerCamera : MonoBehaviour
    {
       
        [SerializeField]
        float smoothTime = 0.5f;
        public float SmoothTime
        {
            get { return smoothTime; }
            set { smoothTime = value; }
        }

        

        [Header("Player section")]

        // Does the camera follow the player?
        [SerializeField]
        bool followPlayer = false;

        [SerializeField]
        [Tooltip("Offset from the player")]
        Vector3 offsetFromPlayer;
        public Vector3 OffsetFromPlayer
        {
            get { return offsetFromPlayer; }
            set { offsetFromPlayer = value; }
        }


        // Does the camera always look at the player?
        [SerializeField]
        bool lookAtPlayer = false;

        [SerializeField]
        [Tooltip("Only used if LookAtPlayer is false.")]
        Vector3 lookAxis;
        public Vector3 LookAxis
        {
            get { return lookAxis; }
            set { lookAxis = value; }
        }


        [Header("Move along axis section")]
        [SerializeField]
        bool moveAlongAxis = false;

        [SerializeField]
        Vector3 startPosition;
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        [SerializeField]
        Vector3 moveAxis;
        public Vector3 MoveAxis
        {
            get { return moveAxis; }
            set { moveAxis = value; }
        }

        
        [Header("Move along axis helpers")]
        [SerializeField]
        [Tooltip("Can be used to set StartPosition and MoveAxis on move along axis section.")]
        Transform moveHelper;

        [SerializeField]
        [Tooltip("Can be used to set LookAxis on move along axis section.")]
        Transform lookHelper;

        [SerializeField]
        bool keepHelpers = false;

        public static PlayerCamera Instance { get; private set; }

        GameObject player;

        Vector3 vel, angVel;

        bool computePositionDisable = false;
        public bool ComputePositionDisable
        {
            get { return computePositionDisable; }
            set { computePositionDisable = value; }
        }
        bool computeRotationDisable = false;

        Vector3 desiredPosition;
        public Vector3 DesiredPosition
        {
            get { return desiredPosition; }
            set { desiredPosition = value; }
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
               
                if (moveHelper)
                {
                    startPosition = moveHelper.position;
                    moveAxis = moveHelper.forward;
                }

                if (lookHelper)
                {
                    lookAxis = lookHelper.forward;
                }

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

            if (!keepHelpers)
            {
                Destroy(moveHelper.gameObject);
                Destroy(lookHelper.gameObject);
            }
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
         
            Vector3 desiredForward;

            // 
            // Computing desired camera position.
            //
            if(!computePositionDisable)
            {
                // Set this position as default.
                desiredPosition = transform.position;


                if (followPlayer)
                {
                    // Cmera is following player so we must calculate the new position by the offset and other things.                    
                    if (moveAlongAxis)
                    {
                        // Camera is moving along an axis, so it is not completely free.
                        // KeepHelpers is mostly used to adjust camera in realtime.
                        if (moveHelper && keepHelpers)
                        {
                            startPosition = moveHelper.position;
                            moveAxis = moveHelper.forward;
                        }

                        // Get the desired position.
                        desiredPosition = GetFollowAxisDesiredPosition(startPosition, moveAxis);
                    }
                    else
                    {
                        // Camera is following player and it is completely free.
                        // So it's new position is given by the offset from player.
                        desiredPosition = player.transform.position;
                        desiredPosition += offsetFromPlayer;
                    }

                }
            }


            //
            // We calculate the desired forward direction of the camera.
            //
            if (lookAtPlayer)
            {
                // Camera is looking at the player, so it's forward is given by the camera->player vector.
                desiredForward = player.transform.position + Vector3.up*.8f - desiredPosition;
                
            }
            else
            {
                // Camera has a fixed direction.
                // KeepHelpers is true just for debug.
                if(lookHelper && keepHelpers)
                    lookAxis = lookHelper.forward;

                // Desired forward direction.
                desiredForward = lookAxis;

            }

            // Update position and rotation.
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, smoothTime);
            transform.forward = Vector3.SmoothDamp(transform.forward, desiredForward, ref angVel, smoothTime);

         

        }
        public Vector3 GetFollowAxisDesiredPosition(Vector3 startPosition, Vector3 moveAxis)
        {
            Vector3 ret = player.transform.position;
            ret += offsetFromPlayer;


            Vector3 v = ret - startPosition;

     
   
            Vector3 normal = Vector3.Cross(moveAxis, Vector3.up).normalized;


            v = Vector3.ProjectOnPlane(v, normal);


            ret = startPosition + v;
            return ret;

        }


    }

}
