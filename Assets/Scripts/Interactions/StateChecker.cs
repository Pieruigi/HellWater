using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    // This class is useful when you want a finite state machine to depend on other finite state machines.
    // Use: fill the fsm list and set in the correspoding state you want to check for this finite state machine 
    // conditions in order to check the other machines.
    public class StateChecker : MonoBehaviour
    {
      
        [SerializeField]
        List<FiniteStateMachine> targets;
       
        FiniteStateMachine fsm;

        private void Awake()
        {
            // Set handles to check others 
            foreach(FiniteStateMachine fsm in targets)
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
            // If true set the new state
            Check();
                
            
        }

        // Returns true if every fsm has the desired state, otherwise false
        void Check()
        {

            fsm.Lookup();

        }
    }

}
