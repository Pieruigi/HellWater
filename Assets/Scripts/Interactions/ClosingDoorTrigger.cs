using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class ClosingDoorTrigger : MonoBehaviour
    {
        
        FiniteStateMachine fsm;
      
        

        // Start is called before the first frame update
        void Start()
        {
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
                if (fsm.CurrentStateId == Constants.DoorOpenState)
                    fsm.Lookup();
            }
        }
    }

}
