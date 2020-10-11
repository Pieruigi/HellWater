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
        int dialogStartIndex = 0;

        [SerializeField]
        int dialogSpeechCount = 0;

        [SerializeField]
        float dialogDelay = 0;

        [SerializeField]
        float directorDelay = 0;

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

            // Init dialog
            if (!"".Equals(dialogCode.Trim()))
            {
                currentSpeechId = dialogStartIndex;
            }
        }

        void Start()
        {
            // Get the fade animator
            fadeAnimator = CameraFader.Instance.GetComponent<Animator>();

            // If script is provided with a dialog code then get the dialog
            if(!"".Equals(dialogCode.Trim()))
                dialog = Dialog.GetDialog(dialogCode, GameManager.Instance.Language);


            // If is already in playing state then simply play it
            if (fsm.CurrentStateId == (int)CutSceneState.Playing)
            {
                Play();
            }
            else
            {
                // If auto play the check state and eventually play it
                if (fsm.CurrentStateId == (int)CutSceneState.Ready && playOnEnter)
                    fsm.Lookup();
            }
            
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
                        int maxId = dialogStartIndex + dialogSpeechCount;
                        if (dialogSpeechCount == 0)
                            maxId = dialog.GetNumberOfSpeeches() - dialogStartIndex;

                        if(currentSpeechId < maxId)
                        {
                            Debug.Log("Show next:" + currentSpeechId);
                            ShowNextSpeech(); // Next 
                        }
                        else
                        {
                            Debug.Log("Exit");
                            //Exit(); // Exit ( no more speeches )
                            fsm.Lookup();
                        }
                            
                        
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
            //if (director)
            //{
            //    if (director.time < director.duration)
            //        director.time = director.duration;
            //}

            StartCoroutine(CoroutineExit());

        }

        public void Skip()
        {
            if (director)
            {
                if (director.time < director.duration)
                    director.time = director.duration;
            }
            //fsm.Lookup();
        }

        public bool CanBeSkipped()
        {
            return canSkip;
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (oldState != fsm.CurrentStateId && fsm.CurrentStateId == (int)CutSceneState.Playing)
            {
                Play();
            }
            else
            {
                if (oldState == (int)CutSceneState.Playing && fsm.CurrentStateId == (int)CutSceneState.Played)
                    Exit();
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

            
            fsm.ForceState((int)CutSceneState.Played, false, true);
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

                // We can skip after fade out
                canSkip = true;

                CheckPlayerVisibility(onEnterPlayerState); 

                // Some delay?
                if (fadeInDelay > 0)
                    yield return new WaitForSeconds(fadeInDelay);
                
                // Start director if needed
                StartCoroutine(PlayDirector());
                
                yield return CameraFader.Instance.FadeInCoroutine();
            }
            else
            {
                // We can skip
                canSkip = true;

                CheckPlayerVisibility(onEnterPlayerState);

                StartCoroutine(PlayDirector());
            }

            if (dialogDelay > 0)
                yield return new WaitForSeconds(dialogDelay);

            playing = true;
            

            if(dialog)
                ShowNextSpeech();
                
        }

        IEnumerator PlayDirector()
        {
            if (!director)
                yield break;

            if (directorDelay > 0)
                yield return new WaitForSeconds(directorDelay);

            director.Play();
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
