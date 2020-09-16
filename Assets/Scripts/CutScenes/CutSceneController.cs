using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW.Cinema
{
    // Main cut scene controller that manages timeline and skip process
    public class CutSceneController : MonoBehaviour, ISkippable
    {
        public UnityAction<CutSceneController> OnStart;
        public UnityAction<CutSceneController> OnStop;

        [SerializeField]
        bool playOnEnter = false; // True if you want to automatically play the cut scene on enter


        // Cut scene timeline       
        PlayableDirector timeline;

        FiniteStateMachine fsm;

        bool running = false;

        bool skipEnabled = false;

        CinemaController cinema;

        private void Awake()
        {
            // Get required components
            timeline = GetComponent<PlayableDirector>();
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            cinema = GetComponent<CinemaController>();

            // Setting handles
            cinema.OnFadeOutOnEnterComplete += HandleOnFadeOutOnEnterComplete;
            cinema.OnFadeOutOnExitComplete += HandleOnFadeOutOnExitComplete;
            cinema.OnExit += HandleOnExit;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (playOnEnter)
            {
                // If play on enter is true and the cut scene is ready the play it
                fsm.Lookup();
            }
        }

        public bool CanBeSkipped()
        {
            return skipEnabled;
        }

        public void Skip()
        {
            // Not skippable
            if (!skipEnabled)
                return;

            // Skip
            Exit();
        }


        void Enter()
        {
            if (running)
                return;

            // Cut scene is running
            running = true;

            // Manage camera and player
            cinema.Enter();
        }
        
        public void Exit()
        {
            // You can't skip anymore
            skipEnabled = false;
            
            // Stopping cut scene 
            cinema.Exit();

        }

       

        void HandleOnFadeOutOnEnterComplete(CinemaController cinema) 
        {
            // Start timeline
            timeline.Play();

            // From now on you can skip
            skipEnabled = true;

            OnStart?.Invoke(this);
        }

        void HandleOnFadeOutOnExitComplete(CinemaController cinema)
        {
            // Stop timeline
            timeline.Stop();

            OnStop?.Invoke(this);
        }

        void HandleOnExit(CinemaController cinema)
        {

            running = false;
            
            // Switch to played state
            fsm.Lookup();
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if(oldState == (int)CutSceneState.Ready && fsm.CurrentStateId == (int)CutSceneState.Playing)
            {
                Enter();
            }
        }
    }

}
