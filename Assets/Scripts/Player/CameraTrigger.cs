using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        Vector3 offset;

        [SerializeField]
        float smoothTime = 0.2f;

        FollowingCamera cam;

        float smoothTimeOld;

        // Start is called before the first frame update
        void Start()
        {
            if (!target)
                target = PlayerController.Instance.transform;

            cam = FollowingCamera.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == target)
            {
                cam.Offset = offset;
                smoothTimeOld = cam.SmoothTime;
                cam.SmoothTime = smoothTime;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == target)
            {
                cam.SmoothTime = smoothTimeOld;
            }
        }
    }

}
