using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW.Cinema
{
    
    
    


    public class CinemaController : MonoBehaviour
    {
        /**
     * FadeOutThenFadeIn: common cut scenes, you fade out, setup things and the fade back in
     * ForceBlackThenFadeIn: black screen is forced black screen and the you fade in ( useful to enter a new scene )
     * */
        enum CameraFadeModeOnEnter { FadeOutThenFadeIn, ForceBlackThenFadeIn }


        /**
        * None: no action on player
        * Enabled: is enabled
        * OnlyDisabled: player is visible but you can't control it
        * Hidde: player is not visible 
        * */
        enum PlayerState { None, Enabled, OnlyDisabled, Hidden }

        // Is called on fade out completed on enter ( you can for example disable player )
        public UnityAction<CinemaController> OnFadeOutOnEnterComplete;

        public UnityAction<CinemaController> OnEnter;

        // Is called on fade out completed on exit ( you can for example enable player )
        public UnityAction<CinemaController> OnFadeOutOnExitComplete;

        public UnityAction<CinemaController> OnExit;

        [Header("Common - Enter")]
        [SerializeField]
        bool forceBlackScreenOnEnter = false;
        //CameraFadeModeOnEnter cameraFadeModeOnEnter = CameraFadeModeOnEnter.FadeOutThenFadeIn;

        [SerializeField]
        float fadeSpeedOnEnter = -1; // -1 will keep default camera fade settings

        [SerializeField]
        float fadeInDelayOnEnter = 0; // How much time it will take to fade in 

        [SerializeField]
        bool exitSoon = false; // Exit without playing any cut scene for eample

        [SerializeField]
        PlayerState playerStateOnEnter = PlayerState.None;

        [Header("Common - Exit")]
        [SerializeField]
        bool keepBlackScreenOnExit = false;

        [SerializeField]
        float fadeSpeedOnExit = -1; // -1 will keep default camera fade settings

        [SerializeField]
        float fadeInDelayOnExit = 0; // How much time it will take to fade in ( or to remain black )

        [SerializeField]
        float delayOnExit = 0;

        [SerializeField]
        PlayerState playerStateOnExit = PlayerState.None;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Exit()
        {
            StartCoroutine(ExitCoroutine());
        }
   
        public void Enter()
        {
            StartCoroutine(EnterCoroutine());
        }

        IEnumerator EnterCoroutine()
        {
            // Check if player controller must be deactivated
            CheckPlayerController(playerStateOnEnter);
            

            // Force black screen the wait for delay
            if (forceBlackScreenOnEnter)
            {
                CameraFader.Instance.ForceBlackScreen();
            }
            else // Fade out
            {
                if (fadeSpeedOnEnter > 0) 
                    yield return CameraFader.Instance.FadeOutCoroutine(fadeSpeedOnEnter); // Custom fade speed
                else
                    yield return CameraFader.Instance.FadeOutCoroutine(); // Default fade speed
            }

            // Check player controller visibility
            CheckPlayerVisibility(playerStateOnEnter);

            // If exit soon then call exit and stop entering...
            if (exitSoon)
            {
                Exit();    
                yield break; // Exit
            }

            
            // Do something
            OnFadeOutOnEnterComplete?.Invoke(this);

            // Wait for delay
            if (fadeInDelayOnEnter > 0)
                yield return new WaitForSeconds(fadeInDelayOnEnter);

            // Fade in
            if (fadeSpeedOnEnter > 0)
                yield return CameraFader.Instance.FadeInCoroutine(fadeSpeedOnEnter); // Custom fade speed
            else
                yield return CameraFader.Instance.FadeInCoroutine(); // Default fade speed

            
            OnEnter?.Invoke (this);
        }

        IEnumerator ExitCoroutine()
        {
            if (!exitSoon)
            {
                // Fade out starts ever
                if (fadeSpeedOnExit > 0)
                    yield return CameraFader.Instance.FadeOutCoroutine(fadeSpeedOnExit);
                else
                    yield return CameraFader.Instance.FadeOutCoroutine();
            }
            
            // Check player visibility
            CheckPlayerVisibility(playerStateOnExit);

            // Do something
            OnFadeOutOnExitComplete?.Invoke(this);

            // Wait fo delay
            if (fadeInDelayOnExit > 0)
                yield return new WaitForSeconds(fadeInDelayOnExit);

            // Fade back in or keep black screen ?
            if (!keepBlackScreenOnExit)
            {
                // Take into account custom fade speed if needed
                if (fadeSpeedOnExit > 0)
                    yield return CameraFader.Instance.FadeInCoroutine(fadeSpeedOnExit);
                else
                    yield return CameraFader.Instance.FadeInCoroutine();
            }

            // Wait for delay before completing
            if (delayOnExit > 0)
                yield return new WaitForSeconds(delayOnExit);

            // Check for player controller
            CheckPlayerController(playerStateOnExit);

            // Cut scene completed
            OnExit?.Invoke(this);
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
