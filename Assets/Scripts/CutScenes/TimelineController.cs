using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using HW.Interfaces;

namespace HW.Cinema
{
    public class TimelineController : MonoBehaviour, ISkippable
    {
        [SerializeField]
        bool playOnEnter = false; // True if you want to automatically play the cut scene on enter

        [SerializeField]
        float skipDelay = 3;
       
        PlayableDirector timeline;

        FiniteStateMachine fsm;

        bool running = false;

        bool readyToSkip = false; 

        private void Awake()
        {
            timeline = GetComponent<PlayableDirector>();
            fsm = GetComponent<FiniteStateMachine>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (playOnEnter)
            {
                // If play on enter is true and the cut scene is ready the play it
                if (fsm.CurrentStateId == (int)CutSceneState.Ready)
                    Play();
            }
        }

        

        public bool CanBeSkipped()
        {
            return readyToSkip;
        }

        public void Skip()
        {
            if (!readyToSkip)
                return;
        }


        void Play()
        {
            running = true;
            timeline.Play();
            StartCoroutine(CoroutinSkipDelay());
        }
        
        void Stop()
        {
            running = false;
            fsm.Lookup();
        }

        IEnumerator CoroutinSkipDelay()
        {
            yield return new WaitForSeconds(skipDelay);

            readyToSkip = true;
        }
    }

}
