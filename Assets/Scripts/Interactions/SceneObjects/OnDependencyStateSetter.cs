using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    // This class is useful when you want a finite state machine to depend on other finite state machines.
    // Use: fill the fsm list and set in the correspoding state you want to check for this finite state machine 
    // conditions in order to check the other machines.
    public class OnDependencyStateSetter : MonoBehaviour
    {
        [System.Serializable]
        private class FsmData
        {
            [SerializeField]
            public FiniteStateMachine fsm;

            [SerializeField]
            public int desiredState;
        }

        [SerializeField]
        List<FsmData> others;

        [SerializeField]
        int requiredState = -1; // The state you must stay in order to perform checking

        [SerializeField]
        int newState;

        [SerializeField]
        bool callOnStateChangeEvent;

        FiniteStateMachine fsm;

        private void Awake()
        {
            // Set handles to check others 
            foreach(FsmData data in others)
            {
                data.fsm.OnStateChange += HandleOnStateChange;
            }

            // Get this finite state machine
            fsm = GetComponent<FiniteStateMachine>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Check on start.
            // We are not afraid to override cached state because checking only work in a given state.
            Check();
                
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            // If true set the new state
            Check();
                
            
        }

        // Returns true if every fsm has the desired state, otherwise false
        void Check()
        {
            // This only works in a specific state
            if (fsm.CurrentStateId != requiredState)
                return;

            foreach (FsmData data in others)
                if (data.fsm.CurrentStateId != data.desiredState)
                    return;

            // All checked, set new state
            fsm.ForceState(newState, callOnStateChangeEvent, true);

        }
    }

}
