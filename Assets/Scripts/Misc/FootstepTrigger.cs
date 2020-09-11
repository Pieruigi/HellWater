using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FootstepTrigger : MonoBehaviour
    {
        [SerializeField]
        GroundType groundType;
        public GroundType GroundType
        {
            get { return groundType; }
        }

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
            // Footstep listener must be attached to the object in order to be processed
            FootstepListener fl = other.GetComponent<FootstepListener>();
            if (!fl)
                return;

            fl.EnterFootstepTrigger(this);
        }

        private void OnTriggerExit(Collider other)
        {
            // Footstep listener must be attached to the object in order to be processed
            FootstepListener fl = other.GetComponent<FootstepListener>();
            if (!fl)
                return;

            fl.ExitFootstepTrigger(this);
        }
    }

}
