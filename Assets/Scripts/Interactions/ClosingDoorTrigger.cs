using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class ClosingDoorTrigger : MonoBehaviour
    {
        
        DoorController ctrl;
        FiniteStateMachine fsm;
     
        // Start is called before the first frame update
        void Start()
        {
            ctrl = GetComponentInParent<DoorController>();
            fsm = GetComponentInParent<FiniteStateMachine>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == Tags.Player)
            {
                if (ctrl.Opened)
                    fsm.Lookup();
            }
        }
    }

}
