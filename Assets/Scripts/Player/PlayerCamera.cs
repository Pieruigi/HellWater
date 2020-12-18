using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    
    [ExecuteAlways]
    public class PlayerCamera : MonoBehaviour
    {
        public UnityAction OnStart;

        // Useful to apply some changes to the camera after position and rotation has been calculated.
        public UnityAction OnComplete;


        [SerializeField]
        float smoothTime = 0.5f;

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

        [Header("Move along axis section")]
        [SerializeField]
        bool moveAlongAxis = false;

        [SerializeField]
        Vector3 startPosition;

        [SerializeField]
        Vector3 moveAxis;

        [SerializeField]
        [Tooltip("Only used if LookAtPlayer is false.")]
        Vector3 lookAxis;

        [Header("Move along axis helpers")]
        [SerializeField]
        [Tooltip("Can be used to set StartPosition and MoveAxis on move along axis section.")]
        Transform moveTarget;

        [SerializeField]
        [Tooltip("Can be used to set LookAxis on move along axis section.")]
        Transform lookTarget;

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
            OnStart?.Invoke();

            Vector3 desiredPosition = transform.position, desiredForward;
            
            if (followPlayer)
            {

                desiredPosition = player.transform.position;
                desiredPosition += offsetFromPlayer;
                if (moveAlongAxis)
                {
                    if(moveTarget)
                        startPosition = moveTarget.position;
                    
                    Vector3 v = desiredPosition - startPosition;

                    if(moveTarget)
                        moveAxis = moveTarget.forward;

                    Debug.LogFormat("MoveAxis:{0}", moveAxis);
                    //Vector3 axis = Vector3.right;
                    Vector3 normal = Vector3.Cross(moveAxis, Vector3.up).normalized;
                    Debug.LogFormat("Normal:{0}", normal);

                    v = Vector3.ProjectOnPlane(v, normal);
                    
                    
                    desiredPosition = startPosition + v;

                }

                Debug.LogFormat("DesiredPos:{0}", desiredPosition);
                transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, smoothTime);
            }


            if (lookAtPlayer)
            {
                desiredForward = player.transform.position + Vector3.up*.8f - desiredPosition;
                transform.forward = Vector3.SmoothDamp(transform.forward, desiredForward, ref angVel, smoothTime);


            }

            OnComplete?.Invoke();

        }

        

     

        
    }

}
