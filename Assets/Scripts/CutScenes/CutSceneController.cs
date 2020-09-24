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
            if (playOnEnter && fsm.CurrentStateId == (int)CutSceneState.Ready)
                fsm.Lookup();

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
            fsm.Lookup();
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (oldState == (int)CutSceneState.Ready && fsm.CurrentStateId == (int)CutSceneState.Playing)
            {
                Play();
            }
        }

    }
}
