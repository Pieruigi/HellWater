using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.UI;
using HW.Interfaces;
using UnityEngine.Playables;
using System;

namespace HW.CutScene
{
    

    // Use this class to manage timelines and dialogs.
    // Timeline is set by looking for a playable director in the gameobject; dialog must be set up using its code.
    // You can choose to have no dialog or no timeline ( you should have at least one of these ).
    // You can also choose to use fade or to let the timeline to manage it.
    public class PlayableController : MonoBehaviour, ISkippable
    {
        /**
       * None: no action on player
       * Enabled: is enabled
       * OnlyDisabled: player is visible but you can't control it
       * Hidde: player is not visible 
       * */
        enum PlayerState { None, Enabled, OnlyDisabled, Hidden }

        [SerializeField]
        [Tooltip("Leave empty if you want no dialog at all.")]
        string dialogCode; // The dialog you want to show

        [SerializeField]
        float dialogDelay = 0;

        [SerializeField]
        bool useFade = false;

        [SerializeField]
        float fadeInDelay = 1f;

        [SerializeField]
        PlayerState onEnterPlayerState = PlayerState.None;

        [SerializeField]
        bool playOnEnter = false;

        [SerializeField]
        PlayerState onExitPlayerState = PlayerState.None;

        [SerializeField]
        bool stopOnExit = false; // You should set true on loop, otherwise you should leave false

        // You can use an external timeline ( for example if you want a npc to stay in the same position and animation )
        // or even a custom timeline ( which starts fading out )
        PlayableDirector director; // The timeline you are using... if you are using one

        FiniteStateMachine fsm;

        Dialog dialog;

        bool playing = false; // True if is playing
        int currentSpeechId = 0; 
        DateTime lastSpeech;
        bool canSkip = false;
        Animator fadeAnimator; // Animator on fade controller avoid us to fade from this script, so we disable it
        protected void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // Timeline
            director = GetComponent<PlayableDirector>();

        }

        void Start()
        {
            // Get the fade animator
            fadeAnimator = CameraFader.Instance.GetComponent<Animator>();

            // If script is provided with a dialog code then get the dialog
            if(!"".Equals(dialogCode.Trim()))
                dialog = Dialog.GetDialog(dialogCode, GameManager.Instance.Language);

            // If auto play the check state and eventually play it
            if (fsm.CurrentStateId == (int)CutSceneState.Ready && playOnEnter)
                fsm.Lookup();
        }

        // Update is called once per frame
        void Update()
        {
            // Inside loop
            if (playing && dialog)
            {
                // Wait a moment
                if((DateTime.UtcNow - lastSpeech).TotalSeconds > 0.5f)
                {
                    // Go to next speech or exit on action button pressed
                    if (PlayerController.Instance.GetActionButtonDown())
                    {
                        if(currentSpeechId < dialog.GetNumberOfSpeeches())
                            ShowNextSpeech(); // Next 
                        else
                            Exit(); // Exit ( no more speeches )
                        
                    }
                }
            }
        }

        void Play()
        {
            canSkip = false;
            StartCoroutine(CoroutinePlay());
        }


        public void Exit()
        {
            canSkip = false;

            // In case we skip 
            if (director)
            {
                if (director.time < director.duration)
                    director.time = director.duration;
            }

            StartCoroutine(CoroutineExit());

        }

      

        public void Skip()
        {
            Exit();
        }

        public bool CanBeSkipped()
        {
            return canSkip;
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (fsm.CurrentStateId == (int)CutSceneState.Ready && playOnEnter)
                fsm.Lookup();
            else
            {
                if (fsm.CurrentStateId == (int)CutSceneState.Playing && oldState == (int)CutSceneState.Ready)
                {
                    Play();
                }
               
            }
        }

        IEnumerator CoroutineExit()
        {
            // No longer playing
            playing = false;

            // Hide viewer
            DialogViewer.Instance.Hide();

            if (useFade) // Using fade
            {
                // Fade out
                yield return CameraFader.Instance.FadeOutCoroutine();

                CheckPlayerVisibility(onExitPlayerState);

                // Stop director while in black screen
                if (director)
                {
                    if(stopOnExit)
                        director.Stop();
                    else
                    {
                        if (director.time < director.duration)
                            director.time = director.duration;
                    }
                    

                }
                    
                
                // Have some delay ?
                if (fadeInDelay > 0)
                    yield return new WaitForSeconds(fadeInDelay);
                
                // Fade in
                yield return CameraFader.Instance.FadeInCoroutine();
                
                // Enable the fade animator 
                if (fadeAnimator)
                    fadeAnimator.enabled = true;
            }
            else // No fade, just stop the director
            {
                CheckPlayerVisibility(onExitPlayerState);
                if (director)
                {
                    if (stopOnExit)
                        director.Stop();
                    else
                    {
                        if (director.time < director.duration)
                            director.time = director.duration;
                    }
                }
                
                
            }

            CheckPlayerController(onExitPlayerState);

            // Set finite state machine
            fsm.Lookup();
        }

        IEnumerator CoroutinePlay()
        {
            CheckPlayerController(onEnterPlayerState);

            // When using a custom timeline we fading in and out
            if (useFade)
            {
                // Animator avoid manual fade to be executed
                if (fadeAnimator)
                    fadeAnimator.enabled = false;

                // Start fade out
                yield return CameraFader.Instance.FadeOutCoroutine();

                CheckPlayerVisibility(onEnterPlayerState); 

                // Some delay?
                if (fadeInDelay > 0)
                    yield return new WaitForSeconds(fadeInDelay);
                
                // Start director if needed
                if(director)
                    director.Play();
                
                yield return CameraFader.Instance.FadeInCoroutine();
            }
            else
            {
                CheckPlayerVisibility(onEnterPlayerState);

                if (director)
                    director.Play();
            }

            if (dialogDelay > 0)
                yield return new WaitForSeconds(dialogDelay);

            playing = true;
            canSkip = true;

            if(dialog)
                ShowNextSpeech();
                
        }

        void ShowNextSpeech()
        {
            Dialog.Speech speech = dialog.GetSpeech(currentSpeechId);
            DialogViewer.Instance.ShowSpeech(speech.Content, speech.Avatar);
            lastSpeech = DateTime.UtcNow;
            currentSpeechId++;
        }

        void CheckPlayerVisibility(PlayerState playerState)
        {

            switch (playerState)
            {
                case PlayerState.Enabled:
                    PlayerController.Instance.gameObject.SetActive(true);
                    break;

                case PlayerState.OnlyDisabled:
                    PlayerController.Instance.gameObject.SetActive(true);
                    break;
                case PlayerState.Hidden:
                    PlayerController.Instance.gameObject.SetActive(false);
                    break;
            }
        }


        void CheckPlayerController(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Enabled:
                    PlayerController.Instance.SetDisabled(false);
                    break;

                case PlayerState.OnlyDisabled:
                    PlayerController.Instance.SetDisabled(true);
                    break;
                case PlayerState.Hidden:
                    PlayerController.Instance.SetDisabled(true);
                    break;
            }
        }
    }

}
