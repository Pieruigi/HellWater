using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraFollowTrigger : MonoBehaviour
    {
        [SerializeField]
        bool startFollowingOnEnter = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            // Only works when player enters from behind.
            Vector3 dir = other.transform.position - transform.position;
            if(Vector3.Dot(dir, transform.forward) < 0)
            {
                if (startFollowingOnEnter)
                    PlayerCamera.Instance.FollowPlayer = true;
                else
                    PlayerCamera.Instance.FollowPlayer = false;
            }
           
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            // Only works when player exits from behind.
            Vector3 dir = other.transform.position - transform.position;
            if (Vector3.Dot(dir, transform.forward) < 0)
            {
                if (startFollowingOnEnter)
                    PlayerCamera.Instance.FollowPlayer = false;
                else
                    PlayerCamera.Instance.FollowPlayer = true;
            }
        }
    }

}
