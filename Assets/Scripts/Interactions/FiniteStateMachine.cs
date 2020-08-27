using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FiniteStateMachine : MonoBehaviour
    {
        // This class represent a state if the fsm
        [System.Serializable]
        class State
        {
            [SerializeField]
            string name; // The state name
            public string Name
            {
                get { return name; }
            }

            //[SerializeField]
            //List<Transition> transitions; // A list of transitions to other states
            //public IList<Transition> GetTransitions()
            //{
            //    return transitions.AsReadOnly();
            //}
            
        }

        // Transition is responsible for switching from one state to another
        [System.Serializable]
        class Transition
        {
            [SerializeField]
            string currentState;
            public string CurrentState
            {
                get { return nextState; }
            }

            [SerializeField]
            string nextState; // The next state the fsm will reach when all conditions are true
            public string NextState
            {
                get { return nextState; }
            }

            [SerializeField]
            string action;
            public string Action
            {
                get { return action; }
            }

            public bool CheckConditions() { return true; }
        }


        [SerializeField]
        List<Transition> transitions;

        string currentState;

        private void Awake()
        {
            //CheckForErrors();
                
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TryActivate(string action)
        {
            // Get the action from the current state

        }

        public void Lookup()
        {
            // Get all the transition starting from the current state
            List<Transition> availables = transitions.FindAll(t => t.CurrentState.ToLower().Equals(currentState.ToLower()));

            // No transitions
            if (availables.Count == 0)
                return;

            // Get the first one which satisfies its conditions
            Transition transition = null;
            for (int i = 0; i < availables.Count && transition == null; i++)
            {
                // If conditions are satisfied then we found it
                if (availables[i].CheckConditions())
                    transition = availables[i];
            }

            // If no transition then return
            if (transition == null)
                return;

            // Switch state
            currentState = transition.NextState;


        }

        //public void Lookup()
        //{
        //    // Get the first transition of the current state of whom conditions are satisfied
        //    Transition transition = null;
        //    //State currentState = states[currentStateId];
        //    for(int i=0; i< currentState.GetTransitions().Count && transition==null; i++)
        //    {
        //        // If conditions are satisfied then we found it
        //        if (currentState.GetTransitions()[i].CheckConditions())
        //            transition = currentState.GetTransitions()[i];
        //    }

        //    // If no transition has been found then return
        //    if (transition == null)
        //        return;

        //    // Switch state
        //    //currentStateId = states.IndexOf(states[transition.NextStateId]);
        //    currentState = states.Find(s => s.Name == transition.NextStateName);


        //}


        //void CheckForErrors()
        //{
        //    // Look for all states in each transition
        //    foreach(State state in states)
        //    {
        //        foreach(Transition transition in state.GetTransitions())
        //        {
        //            if (states.Find(s => s.Name == transition.NextStateName) == null)
        //                throw new System.Exception("State '" + transition.NextStateName + "' not found.");
        //        }
        //    }

        //}
        
    }

}
