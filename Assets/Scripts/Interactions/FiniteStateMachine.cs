﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.Events;

namespace HW
{
    
    [ExecuteAlways]
    public class FiniteStateMachine: MonoBehaviour
    {
        

        // Called on state change except when ForceState() is called with callEvent = false; int param is the old state
        public UnityAction<FiniteStateMachine> OnStateChange;

        // Called everytime a lookup fails ( normally when conditions are not satisfied ); returns the error id
        public UnityAction<FiniteStateMachine> OnFail; 

        /**
         * Transitions take care about switching from one state to onother by checking some conditions.
         * A condition is satisfied when the finite state machine to which refers is in a given state.
         * */
        [System.Serializable]
        public class Transition
        {
            // Class to manage conditions
            [System.Serializable]
            private class OtherToCheck
            {
                [SerializeField]
                FiniteStateMachine fsm;
                public FiniteStateMachine FiniteStateMachine
                {
                    get { return fsm; }
                }

                [SerializeField]
                int desiredStateId;
                public int DesiredStateId
                {
                    get { return desiredStateId; }
                }
            }

            // Can be used to choose between different transitions heading to the same state ( or just as label )
            [SerializeField]
            string tag;
            public string Tag
            {
                get { return tag; }
            }

            // The state we start from ( it should be the current state the machine is in )
            [SerializeField]
            int fromStateId;
            public int FromStateId
            {
                get { return fromStateId; }
            }

            // The new state we want the machine if this transition passes
            [SerializeField]
            int toStateId;
            public int ToStateId
            {
                get { return toStateId; }
            }

            // Conditions
            [SerializeField]
            List<OtherToCheck> othersToCheck;

            // The error that will be sent out when conditions are not satisfied
            [SerializeField]
            int errorCode = -1;
            public int ErrorCode
            {
                get { return errorCode; }
            }

            // If true the machine can change state
            public bool Checked()
            {
                foreach (OtherToCheck other in othersToCheck)
                {
                    if (other.FiniteStateMachine.currentStateId != other.DesiredStateId) 
                        return false;
                }
                return true;
            }
        }

        /**
         * Represents the core of the finite state machine.
         * */
        [System.Serializable]
        private class State
        {
            /**
             * When a finite state machine changes state it can happen that also other machines are forced in a new state;
             * the new state is forces without checking any condition, but we can still call the change state event
             * by setting callEvent true.
             * */
            [System.Serializable]
            private class OtherToSet
            {
                [SerializeField]
                FiniteStateMachine fsm;
                public FiniteStateMachine FiniteStateMachine
                {
                    get { return fsm; }
                }

                [SerializeField]
                int newStateId;
                public int NewStateId
                {
                    get { return newStateId; }
                }

                // Call the event in the machine forced to change its state
                [SerializeField]
                bool callEvent;
                public bool CallEvent
                {
                    get { return callEvent; }
                }

            }

            // Name of the state
            [SerializeField]
            string name;
            public string Name
            {
                get { return name; }
            }

            // Is the position in the array
            [ReadOnly]
            [SerializeField]
            public int id;

            // Other fsm to be set
            [SerializeField]
            List<OtherToSet> othersToSet = new List<OtherToSet>();

            [SerializeField]
            int commonStateErrorCode = -1; // A common error you can use independent on the transition
            public int CommonStateErrorCode
            {
                get { return commonStateErrorCode; }
            }

            [SerializeField]
            int commonStateSucceedCode = -1;
            public int CommonStateSucceedCode
            {
                get { return commonStateSucceedCode; }
            }

            // Sets the other fsm
            public void SetOthers()
            {
                foreach(OtherToSet other in othersToSet)
                {
                    other.FiniteStateMachine.ForceState(other.NewStateId, other.CallEvent, true);
                }
            }
        }

        int lastExitCode = -1;
        public int LastExitCode
        {
            get { return lastExitCode; }
        }

        // All the available states
        [SerializeField]
        List<State> states;

        // All the available transitions
        [SerializeField]
        List<Transition> transitions;

        // Shows the state name in the inspector
        [ReadOnly]
        [SerializeField]
        string currentStateName;

        [SerializeField]
        int currentStateId;
        public int CurrentStateId
        {
            get { return currentStateId; }
        }

        int previousStateId;
        public int PreviousStateId
        {
            get { return previousStateId; }
        }

        string disabledStateName = "Disabled";

        private void Awake()
        {
            previousStateId = currentStateId;
        }

        void Update()
        {
            // Fill the id for each state
            for(int i=0; i<states.Count; i++)
            {
                states[i].id = i;
            }

                        

            if (currentStateId < 0)
                currentStateName = disabledStateName;
            else
            {
                
                if (states != null && states.Count > 0)
                {
                    currentStateName = states[currentStateId].Name;
                }
                    
            }
                
        }

        

        public void ForceStateDisabled()
        {
            previousStateId = currentStateId;
            currentStateId = -1;
            currentStateName = disabledStateName;
        }

        
        /**
         * Try to move to the next state looking for the first checked transition in the current state.
         * */
        public bool Lookup()
        {
            // If disabled then return
            if (currentStateId < 0)
                return false;

            // Get the first checked transition
            Transition transition = transitions.Find(t => t.FromStateId == currentStateId);

            // If no transition return
            if (transition == null)
            {
                lastExitCode = states[currentStateId].CommonStateErrorCode;
                OnFail?.Invoke(this);
                return false;
            }

            if (!transition.Checked())
            {
                lastExitCode = transition.ErrorCode;
                OnFail?.Invoke(this);
                return false;
            }

            // Change state
            ChangeState(transition);

            return true;
        }

        /** 
         * Try to move to the next state by looking for a specific transition ( by tag ) in the current state.
         * Why on earht you would check for a particular transition? 
         * Say for example you have a locked door that can be opened both using the key of the picklock; then you need
         * two transitions heading to the save state but with different some different condition to check;
         * in this case you can set each transition with a given tag and the look for the right one.
         * */
        public bool Lookup(string tag)
        {
            // If disabled then return
            if (currentStateId < 0)
                return false;

            // Get the transition we are looking for
            Transition transition = transitions.Find(t=>t.FromStateId == currentStateId && tag.ToLower().Equals(t.Tag.ToLower()));

            // If no transition has been found the return
            if (transition == null)
            {
                lastExitCode = states[currentStateId].CommonStateErrorCode;
                OnFail?.Invoke(this);
                return false;
            }


            // If transition is not checked return
            if (!transition.Checked())
            {
                lastExitCode = transition.ErrorCode;
                OnFail?.Invoke(this);
                return false;
            }

            // Change state
            ChangeState(transition);

            return true;
        }

        // Forse the state machine to a new state without taking into account any condition.
        // You can call on state change event by setting the callEvent paratm true.
        // You can choose whether or not set others fsm ( if the are referencies ); in this way you can set fsm in cascade;
        // normally you should setOthers false during initialization ( reading from cache ) and set true during
        // common gameplay actions.
        public void ForceState(int stateId, bool callEvent, bool setOthers)
        {
            Debug.Log(name + " forceState("+stateId+","+callEvent+","+setOthers+")");

            previousStateId = currentStateId;
            currentStateId = stateId;

            // We don't want to send message if we are forcing this fsm
            lastExitCode = -1;

            if (callEvent)
                OnStateChange?.Invoke(this);

            if(setOthers && currentStateId >= 0 && currentStateId<states.Count)
                states[currentStateId].SetOthers();
        }

        private void ChangeState(Transition transition)
        {
            // Store old state
            previousStateId = currentStateId;

            // Switch to the new state
            currentStateId = transition.ToStateId;

            // Show the name in the inspector
            if (currentStateId < 0)
                currentStateName = disabledStateName;
            else
                currentStateName = states[currentStateId].Name;

            // Set exit code
            if (currentStateId >= 0)
                lastExitCode = states[currentStateId].CommonStateSucceedCode;
            else
                lastExitCode = -1;

            // Call event
            OnStateChange?.Invoke(this);
            
            if (currentStateId < 0)
                return;

            // Set others
            states[currentStateId].SetOthers();

            
        }
    }
}
