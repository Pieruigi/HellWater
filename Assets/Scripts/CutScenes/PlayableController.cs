using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.UI;
using HW.Interfaces;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;

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

        //[SerializeField]
        //bool useFade = false;

        //[SerializeField]
        //float fadeInDelay = 1f;

        [SerializeField]
        bool playOnEnter = false;

        [SerializeField]
        PlayerState onEnterPlayerState = PlayerState.None;

        [SerializeField]
        PlayerState onExitPlayerState = PlayerState.None;

        [SerializeField]
        bool hasExitSignal = false; // You should set true on loop, otherwise you should leave false

        //[SerializeField]
        double exitTime = -1f;

        [SerializeField]
        bool keepDialogOnExit = false; // Do you want the dialog window to stay visible on exit ?

        [SerializeField]
        bool keepPlayingOnExit = false;

        [SerializeField]
        bool onSkipOrDialogCompletedFadeOut = false; // Set true if you want to fade out on exit

        [SerializeField]
        float onSkipOrDialogCompletedFadeOutSpeed = 0;

        // You can use an external timeline ( for example if you want a npc to stay in the same position and animation )
        // or even a custom timeline ( which starts fading out )
        PlayableDirector director; // The timeline you are using... if you are using one

        FiniteStateMachine fsm;

        Dialog dialog;
        bool dialogPlaying = false;

        bool playing = false; // True if is playing
        int currentSpeechId = 0; 
        DateTime lastSpeech;
        bool canSkip = false;
        Animator fadeAnimator; // Animator on fade controller avoid us to fade from this script, so we disable it

        #region LOOP
        double loopInitialTime;
        bool loopDisabled = false;
        #endregion
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

            //
            // Check exit signal
            //

            if (hasExitSignal)
            {
                // Get timeline
                TimelineAsset timeline = (TimelineAsset)director.playableAsset;

                // Get the track bound to this object which always must be the first ( index 0 is reserved )
                TrackAsset track = new List<TrackAsset>(timeline.GetRootTracks())[1];

                // Get the marker list
                List<IMarker> markers = new List<IMarker>(track.GetMarkers());
              
                bool found = false;
                for(int i=0; i<markers.Count && !found; i++)
                {
                    IMarker marker = markers[i];
                    
                    // Look for the 'Close' marker     
                    if (typeof(SignalEmitter).Equals(marker.GetType()))
                    {
                      
                        if ("close".Equals(((SignalEmitter)markers[i]).asset.name.ToLower()))
                        {
                            found = true;
                            exitTime = markers[i].time;
                        }
                    }
                }



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
            if (playing && dialogPlaying)
            {
                // Wait a moment
                if((DateTime.UtcNow - lastSpeech).TotalSeconds > 0.5f)
                {
                    // Go to next speech or exit on action button pressed
                    if (PlayerInput.GetActionButtonDown())
                    {
                        int maxId = dialogStartIndex + dialogSpeechCount;
                        if (dialogSpeechCount == 0)
                            maxId = dialog.GetNumberOfSpeeches();

                        if (currentSpeechId < maxId)
                        {
                            Debug.Log("Show next:" + currentSpeechId);
                            ShowNextSpeech(); // Next 
                        }
                        else
                        {
                            Debug.Log("Exit");
                            Skip(); // Exit ( no more speeches )
                        }
                    }
                }
            }
        }

        // Call this signal from timeline to set the current time as loop starting time
        public void InitLoop()
        {
            loopInitialTime = director.time;
        }

        // Call this function
        public void Loop()
        {
           
            if (!loopDisabled)
                director.time = loopInitialTime;
        }

        void Play()
        {
            canSkip = false;
            loopDisabled = false;
            StartCoroutine(CoroutinePlay());
        }

        // Called by signal 
        public void Exit()
        {
            if (!playing)
                return;

            canSkip = false;
            loopDisabled = true;

            // If there is no exit signal then playable should be in loop and exit is called by some other object
            //if(!hasExitSignal)
            //    director.Stop();

            StartCoroutine(CoroutineExit());
        }

        // Called on skip or when dialog is completed
        public void Skip()
        {
            StartCoroutine(CoroutineSkip());
        }

        IEnumerator CoroutineSkip()
        {
            if (!playing || !canSkip)
                yield break;

            canSkip = false;
            loopDisabled = true;

            if (!keepDialogOnExit && dialogPlaying)
            {
                dialogPlaying = false;
                DialogViewer.Instance.Hide();
            }
                

            if (onSkipOrDialogCompletedFadeOut)
            {
                CameraFader.Instance.TryDisableAnimator();
                yield return CameraFader.Instance.FadeOutCoroutine(onSkipOrDialogCompletedFadeOutSpeed);
                yield return new WaitForSeconds(0.5f);
                CameraFader.Instance.TryEnableAnimator();
            }

            if (director) // Timeline is playing
            {
                if (!keepPlayingOnExit) // We don't want the timeline to keep playing 
                {
                    if (hasExitSignal) // We have an exit signal, so we put the timeline there
                    {
                        director.time = exitTime;
                    }
                    else // No exit signal, just stop the timeline
                    {
                        director.Stop();
                    }
                }
            }

            // Exit 
            StartCoroutine(CoroutineExit());
        }

        public bool CanBeSkipped()
        {
            return canSkip;
        }

       
        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if (fsm.PreviousStateId != fsm.CurrentStateId && fsm.CurrentStateId == (int)CutSceneState.Playing)
            {
                Play();
            }
            else
            {
                if (fsm.PreviousStateId == (int)CutSceneState.Playing && fsm.CurrentStateId == (int)CutSceneState.Played)
                {
                    if (playing)
                    {
                        director.Stop();
                        Exit();
                    }
                    
                }
                    
            }
        }

        IEnumerator CoroutineExit()
        {
            if (!playing)
                yield break;

            // No longer playing
            playing = false;

            // Hide viewer
            if (!keepDialogOnExit && dialogPlaying)
            {
                dialogPlaying = false;
                DialogViewer.Instance.Hide();
            }

            // If player needs to be enable then it was disable before, so the game was busy
            if (onExitPlayerState == PlayerState.Enabled)
                GameManager.Instance.GameBusy = false;

            CheckPlayerVisibility(onExitPlayerState);
            CheckPlayerController(onExitPlayerState);

           // yield return new WaitForEndOfFrame();

            
            fsm.ForceState((int)CutSceneState.Played, true, true);
        }

        IEnumerator CoroutinePlay()
        {
            if (playing)
                yield break;
            
            // We can skip
            canSkip = true;
            playing = true;

            // If player is disabled on enter the game is busy
            if (onEnterPlayerState == PlayerState.Hidden || onEnterPlayerState == PlayerState.OnlyDisabled)
                GameManager.Instance.GameBusy = true;

            CheckPlayerVisibility(onEnterPlayerState);
            CheckPlayerController(onEnterPlayerState);

            StartCoroutine(PlayDirector());

            if (dialogDelay > 0)
                yield return new WaitForSeconds(dialogDelay);


            if (dialog)
            {
                dialogPlaying = true;
                ShowNextSpeech();
            }
                
                
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
