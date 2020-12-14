using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HW.CutScene
{
    public class PlayableLauncher : MonoBehaviour
    {
        // The finite state machine that controls the launcher
        [SerializeField]
        FiniteStateMachine fsm;

        // The state you want the playable to start. Negative means any state.
        [SerializeField]
        int state = -1;

        // Loop means that the playable director will be activated even if the fsm loop in the
        // current state.
        [SerializeField]
        bool loopAllowed = false;

        PlayableDirector playableDirector;

        private void Awake()
        {
            // Set the handler to check the fsm state.
            fsm.OnStateChange += HandleOnStateChange;

            // Set the playable.
            playableDirector = GetComponent<PlayableDirector>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // If the fsm current state is equal to the activation state then fast forward 
            // the playable director.
            //if (fsm.CurrentStateId == state)
            //    playableDirector.time = playableDirector.duration;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // Check for the current state if needed. 
            if (state >= 0 && fsm.CurrentStateId != state)
                return;

            // Does the set up allows loop?
            if (fsm.PreviousStateId == fsm.CurrentStateId && !loopAllowed)
                return;

            // Start playable.
            playableDirector.Play();
        }
    }

}
