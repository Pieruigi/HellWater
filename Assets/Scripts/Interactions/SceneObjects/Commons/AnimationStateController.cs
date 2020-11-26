using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// This class controls the animation depending on the state.
    /// </summary>
    public class AnimationStateController : MonoBehaviour
    {
        // Set true if you want to set on start depending on the fsm state.
        [SerializeField]
        bool setOnStart = false;

        Animator animator;

        string stateParamName = "State";

        FiniteStateMachine fsm;


        void Awake()
        {
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

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            Debug.Log("Init:" + fsm.CurrentStateId);
            animator.SetInteger(stateParamName, fsm.CurrentStateId);
        }
    }

}
