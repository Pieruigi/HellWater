using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class InOutInteractionTrigger : MonoBehaviour
    {

        [SerializeField]
        GameObject interactableObject;

        [SerializeField]
        int desiredState = -1; // Negative means all states.

        [SerializeField]
        FiniteStateMachine target;


        private int insideState = 1;
        private int outsideState = 0;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = interactableObject.GetComponent<FiniteStateMachine>();
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
            if(other.tag == Tags.Player)
            {
                if(fsm.CurrentStateId < 0 || fsm.CurrentStateId == desiredState)
                    target.ForceState(insideState, true, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                if (fsm.CurrentStateId < 0 || fsm.CurrentStateId == desiredState)
                    target.ForceState(outsideState, true, true);
            }
        }

      


    }

}
