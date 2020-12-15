using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FollowingCamera : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        Vector3 offset;
        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        [SerializeField]
        float smoothTime = 2f;
        public float SmoothTime
        {
            get { return smoothTime; }
            set { smoothTime = value; }
        }

        [SerializeField]
        bool lookAt = false;

        Vector3 velocity;
        Vector3 angularVelocity;

        public static FollowingCamera Instance { get; private set; }

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
            if (!target)
                target = PlayerController.Instance.transform;

            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            // Smooth position.
            Vector3 desiredPosition = target.position + offset;
            //transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.042f);

            // Smooth rotation.
            if (lookAt)
            {
                Vector3 desiredFwd = target.position - transform.position;
                transform.forward = Vector3.SmoothDamp(transform.forward, desiredFwd, ref angularVelocity, smoothTime);
            }
            

        }
    }

}
