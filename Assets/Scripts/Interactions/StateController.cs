using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// By attaching this class to an object depending on a finite machine state you can control
    /// the object using an animator.
    /// The simplest way is to set all the states you need in the animator and then call them
    /// from any state.
    /// </summary>
    public class StateController : MonoBehaviour
    {
        // Set true if you want to set on start depending on the fsm state, otherwise live false.
        [SerializeField]
        bool setOnStart = false;

        [SerializeField]
        [Tooltip("The finite state machine this object depends on; if null it will try to set the" +
            "field from this object")]
        FiniteStateMachine fsm;

        Animator animator;

        string stateParamName = "State";


        void Awake()
        {
            if(!fsm)
                fsm = GetComponent<FiniteStateMachine>();

            fsm.OnStateChange += HandleOnStateChange;
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if(setOnStart)
                animator.SetInteger(stateParamName, fsm.CurrentStateId);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            Debug.Log("Init:" + fsm.CurrentStateId);
            animator.SetInteger(stateParamName, fsm.CurrentStateId);
        }
    }

}
