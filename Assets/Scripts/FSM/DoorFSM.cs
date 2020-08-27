using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum DoorState { Locked, Closed, Open }

    public enum DoorAction { Unlock, Lock, Close, Open }

    [System.Serializable]
    public class DoorFSM : FiniteStateMachine<DoorState, DoorAction>
    {
        [System.Serializable]
        protected class DoorTransition : Transition
        {
        }

        [SerializeField]
        List<DoorTransition> transitions; 

        protected override List<Transition> GetTransitions()
        {
            return new List<Transition>(transitions);
        }
    }

}
