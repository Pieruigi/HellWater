using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FollowingCamera : MonoBehaviour
    {
        [SerializeField]
        Transform target;
        public Transform Target
        {
            get { return target; }
        }

        FollowingCameraData data;

        Vector3 velocity;
        Vector3 angularVelocity;

        public static FollowingCamera Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                data = GetComponent<FollowingCameraData>();
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
            Vector3 desiredPosition = target.position + data.Offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, data.SmoothTime);
           // transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.042f);

            // Smooth rotation.
            if (data.LookAt)
            {
                // Look at the target.
                Vector3 desiredFwd = target.position - transform.position;
                transform.forward = Vector3.SmoothDamp(transform.forward, desiredFwd, ref angularVelocity, data.SmoothTime);
            }
            else
            {
                // Use lookAngles field.
                //transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, data.LookAtAngles, ref angularVelocity, data.SmoothTime);
                transform.forward = Vector3.SmoothDamp(transform.forward, data.ForwardTarget.forward, ref angularVelocity, data.SmoothTime);
            }

        }
    }

}
