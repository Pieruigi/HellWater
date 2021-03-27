using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraTransformTrigger : MonoBehaviour
    {
        [SerializeField]
        bool updatePosition;

        [SerializeField]
        bool updateForward;

        [SerializeField]
        Transform transformEnter;

        [SerializeField]
        Transform transformExit;


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

            
            Vector3 dir = other.transform.position - transform.position;
            if (Vector3.Dot(dir, transform.forward) < 0)
            {
                // From behind.
                if (updatePosition)
                {
                    PlayerCamera.Instance.ForcePosition(transformEnter.position);
                    //PlayerCamera.Instance.transform.position = transformEnter.position;
                    //PlayerCamera.Instance.DesiredPosition = transformEnter.position;
                }


                if (updateForward)
                {
                    //PlayerCamera.Instance.transform.forward = transformEnter.forward;
                    PlayerCamera.Instance.ForceDirection(transformEnter.forward);
                }
                    
            }
         
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;


            Vector3 dir = other.transform.position - transform.position;
            if (Vector3.Dot(dir, transform.forward) < 0)
            {
                // From behind.
                if (updatePosition)
                {
                    PlayerCamera.Instance.ForcePosition(transformExit.position);
                    //PlayerCamera.Instance.transform.position = transformExit.position;
                    //PlayerCamera.Instance.DesiredPosition = transformExit.position;
                }
                    

                if (updateForward)
                {
                    //PlayerCamera.Instance.transform.forward = transformExit.forward;
                    PlayerCamera.Instance.ForceDirection(transformExit.forward);
                }
                    
            }

        }
    }

}
