using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// Set to a new state when time elapsed.
    /// </summary>
    public class StateTimer : MonoBehaviour
    {
        [SerializeField]
        int desiredState = -1; // Negative means all states.

        [SerializeField]
        int targetState;

        [SerializeField]
        float timer;

        [SerializeField]
        FiniteStateMachine fsm;

        [SerializeField]
        bool callEvent = false;

        [SerializeField]
        bool setOthers = false;

        float currentTimer = 0;

        private void Awake()
        {
            // If the finite state machine is null the one attached to this object is given.
            if (!fsm)
                fsm = GetComponent<FiniteStateMachine>();

            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;

                if (currentTimer <= 0)
                    fsm.ForceState(targetState, callEvent, setOthers);
            }
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // Wrong state.
            if (desiredState >= 0 && fsm.CurrentStateId != desiredState)
                return;

            currentTimer = timer;
        }
    }

}
