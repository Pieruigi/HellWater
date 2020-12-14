using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    // This class is useful when you want a finite state machine to depend on other finite state machines.
    // Use: fill the fsm list and set in the correspoding state you want to check for this finite state machine 
    // conditions in order to check the other machines.
    public class StateChecker : MonoBehaviour
    {
        // This is called every time a fsm is checked
        public UnityAction<StateChecker, FiniteStateMachine> OnChecked;

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

        public int TargetCount()
        {
            return targets.Count;
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // If true set the new state
            Check();

            OnChecked?.Invoke(this, fsm);
        }

        // Returns true if every fsm has the desired state, otherwise false
        void Check()
        {

            fsm.Lookup();
            
        }
    }

}
