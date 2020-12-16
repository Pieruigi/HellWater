using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraWaypoint : MonoBehaviour
    {
        // If true the position is computed by adding the target local position to the player, otherwise the 
        // the target world position is taken into account.
        [SerializeField]
        bool offsetFromPlayer = false;

        [SerializeField]
        Transform target;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector3 GetDesiredCameraPosition()
        {
            
            return offsetFromPlayer ? PlayerController.Instance.transform.position + target.localPosition : (target ? target.position : transform.position);

        }

        public Vector3 GetDesiredCameraForward()
        {
            return target ? target.forward : transform.forward;
        }

    }

}
