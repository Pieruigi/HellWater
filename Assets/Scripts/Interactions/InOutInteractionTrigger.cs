using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class InOutInteractionTrigger : MonoBehaviour
    {

        Transform target; // The player or some other gameplay object.

        //[SerializeField]
        FiniteStateMachine fsm;


        private int insideState = 1;
        private int outsideState = 0;

        bool inside = false;
        
        bool skip = false;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();

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
            // We skip after the object has been enabled in order to let it check the trigger.
            if(skip)
            {
                skip = false;
                return;
            }

            // Check whether the trigger state match the fsm.
            bool force = false;
            if ((inside && fsm.CurrentStateId != insideState) || (!inside && fsm.CurrentStateId != outsideState))
                force = true;

            // No match, force fsm.
            if(force)
                fsm.ForceState(inside ? insideState : outsideState, true, true);

        }

        private void OnEnable()
        {
            // We set a frame to let the physics check whether the target is inside or not.
            skip = true;
        }

        private void OnDisable()
        {
            // Set disable state which is -1.
            fsm.ForceStateDisabled();
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == target)
                inside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform == target)
                inside = false;
        }

        


    }

}
