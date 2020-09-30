using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    // This class is useful when you want a finite state machine to depend on other finite state machines.
    // Use: fill the fsm list and set in the correspoding state you want to check for this finite state machine 
    // conditions in order to check the other machines.
    // Ex. fence puzzle to check it new dialog need to be activated
    public class MultiFiniteStateMachineChecker : MonoBehaviour
    {
        [SerializeField]
        List<FiniteStateMachine> others;

        FiniteStateMachine fsm;

        private void Awake()
        {
            // Set handles to catch other fsm 
            foreach(FiniteStateMachine fsm in others)
            {
                fsm.OnStateChange += HandleOnStateChange;
            }

            // Get this finite state machine
            fsm = GetComponent<FiniteStateMachine>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            Debug.Log("Check fsm:" + fsm.CurrentStateId);

            // When one of the other fsm changes we call the lookup method.
            // If all conditions of the corresponding transition are satisfied ( they should refer to the other machines )
            // then the lookup succeed.
            this.fsm.Lookup();
        }
    }

}
