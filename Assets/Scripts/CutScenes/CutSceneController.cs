using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW.CutScene
{
    public class CutSceneController : MonoBehaviour, ISkippable
    {
        [SerializeField]
        bool playOnEnter = false; // True if you want to automatically play the cut scene on enter

        [SerializeField]
        float exitTime;

        // Cut scene timeline       
        PlayableDirector timeline;

        FiniteStateMachine fsm;

        bool skipEnabled = false;

        void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            timeline = GetComponent<PlayableDirector>();
        }

        void Start()
        {
            // If is in ready state and play on enter is flagged then the timeline starts
            if (playOnEnter && fsm.CurrentStateId == (int)CutSceneState.Ready)
                fsm.Lookup();

            // If is already in playing state then simply play it
            if (fsm.CurrentStateId == (int)CutSceneState.Playing)
                Play();

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
            timeline.time = exitTime;
        }

        void Play()
        {
            timeline.Play();

            skipEnabled = true;
        }

        public void Exit()
        {
            skipEnabled = false;
            fsm.Lookup();
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (oldState != fsm.CurrentStateId && fsm.CurrentStateId == (int)CutSceneState.Playing)
            {
                Play();
            }

        }

    }
}
