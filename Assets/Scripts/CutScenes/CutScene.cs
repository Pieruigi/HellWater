using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW.Cinema
{
    /**
     * FadeOutThenFadeIn: common cut scenes, you fade out, setup things and the fade back in
     * ForceBlackThenFadeIn: black screen is forced black screen and the you fade in ( useful to enter a new scene )
     * */
    enum CameraFadeModeOnEnter { FadeOutThenFadeIn, ForceBlackThenFadeIn }

    ///**
    // * FadeOutThenFadeIn: common cut scenes, you fade out, reset things and the fade back in
    // * FadeOut: just fade out and remain black ( useful to exit a scene )
    // * */
    //enum CameraFadeModeOnExit { FadeOutThenFadeIn, FadeOut }

    public class CutScene : MonoBehaviour
    {
        // Is called on fade out completed on enter ( you can for example disable player )
        public UnityAction<CutScene> OnFadeOutOnEnterConpleted; 

        // Is called on fade out completed on exit ( you can for example enable player )
        public UnityAction<CutScene> OnFadeOutOnExitCompleted;

        public UnityAction<CutScene> OnCutSceneCompleted;

        [Header("Common - Enter")]
        [SerializeField]
        CameraFadeModeOnEnter cameraFadeModeOnEnter = CameraFadeModeOnEnter.FadeOutThenFadeIn;

        [SerializeField]
        float fadeSpeedOnEnter = -1; // -1 will keep default camera fade settings

        [SerializeField]
        float delayAfterFadeOutOnEnter = 0; // How much time it will take to fade in 

        [Header("Common - Exit")]
        [SerializeField]
        bool keepPlayerDisabled = false;
        
        [SerializeField]
        bool keepBlackScreenOnExit = false;

        [SerializeField]
        float fadeSpeedOnExit = -1; // -1 will keep default camera fade settings

        [SerializeField]
        float delayAfterFadeOutOnExit = 0; // How much time it will take to fade in ( or to remain black )

        [SerializeField]
        float delayOnExit = 0;

        bool running = false; // Cut scene will start running after fade in 

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Enter());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Exit()
        {
            StartCoroutine(ExitCoroutine());
        }
   
        IEnumerator Enter()
        {
         
            // Force black screen the wait for delay
            if (cameraFadeModeOnEnter == CameraFadeModeOnEnter.ForceBlackThenFadeIn)
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

            // Disable the player controller
            PlayerController.Instance.SetDisabled(true);

            // Do something
            OnFadeOutOnEnterConpleted?.Invoke(this);

            // Wait for delay
            if (delayAfterFadeOutOnEnter > 0)
                yield return new WaitForSeconds(delayAfterFadeOutOnEnter);

            // Fade in
            if (fadeSpeedOnEnter > 0)
                yield return CameraFader.Instance.FadeInCoroutine(fadeSpeedOnEnter); // Custom fade speed
            else
                yield return CameraFader.Instance.FadeInCoroutine(); // Default fade speed


            //////////////////////// TEST /////////////
            yield return new WaitForSeconds(3);
            Exit();
        }

        IEnumerator ExitCoroutine()
        {
            // Fade out starts ever
            if (fadeSpeedOnExit > 0)
                yield return CameraFader.Instance.FadeOutCoroutine(fadeSpeedOnExit);
            else
                yield return CameraFader.Instance.FadeOutCoroutine();

            // Enable the player controller
            if(!keepPlayerDisabled)
                PlayerController.Instance.SetDisabled(false);

            // Do something
            OnFadeOutOnExitCompleted?.Invoke(this);

            // Wait fo delay
            if (delayAfterFadeOutOnExit > 0)
                yield return new WaitForSeconds(delayAfterFadeOutOnExit);

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

            // Cut scene completed
            OnCutSceneCompleted?.Invoke(this);
        }

      
    }

}
