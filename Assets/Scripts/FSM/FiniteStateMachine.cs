using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{


    [System.Serializable]
    public abstract class FiniteStateMachine<TState,TAction>: MonoBehaviour
    {
        // Transition from one state to another
        [System.Serializable]
        protected class Transition
        {
            // Should be the current state
            [SerializeField]
            TState fromState;
            public TState FromState
            {
                get { return fromState; }
            }

            // The next state
            [SerializeField]
            TState toState; // The next state the fsm will reach when all conditions are true
            public TState ToState
            {
                get { return toState; }
            }
       
            // The action that calls this transition
            [SerializeField]
            TAction action;
            public TAction Action
            {
                get { return action; }
            }

            public Transition() { }

            public Transition(TState fromState, TState toState, TAction action)
            {
                this.fromState = fromState;
                this.toState = toState;
                this.action = action;
            }

            // Check whether we can go to the next state or not
            public bool CheckConditions() { return true; }
        }


        // Internal transition list ( you need to fill this by the child )
        List<Transition> transitions;

        [SerializeField]
        TState currentState; // The current state

        // Implement this to get all the transitions from children
        protected abstract List<Transition> GetTransitions();

        private void Awake()
        {
            // Init transitions field
            SetTransitions(GetTransitions());
        }

        // Look for the given action in the current state and try to move to the next state
        public void Lookup(TAction action)
        {
            // Get transition from current state given the action
            Transition transition = transitions.Find(t => t.FromState.Equals(currentState) && t.Action.Equals(action));

            // If no transition return
            if (transition == null)
                return;

            // If conditions are not satisfied return 
            if (!transition.CheckConditions())
                return;

            // Ok, lets move to the next state
            currentState = transition.ToState;
        }


        // Fill the transitions field at startup
        void SetTransitions(List<Transition> transitions)
        {
            this.transitions = new List<Transition>();
            foreach(Transition t in transitions)
            {
                
                this.transitions.Add(new Transition(t.FromState, t.ToState, t.Action));
            }
            

        }

        
    }

}
