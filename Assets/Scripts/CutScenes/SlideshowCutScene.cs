using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW.Cinema
{
    public class SlideshowCutScene : MonoBehaviour
    {
        enum StateId { Executed = 0, ToBeExecuted = 1 }

        [System.Serializable]
        class Slide
        {
            [SerializeField]
            public Sprite sprite;

            [SerializeField]
            public float length;
        }

        [SerializeField]
        List<Slide> slides;

        [SerializeField]
        bool executeOnEnter = false; // True if you want this cs to execute after the scene has been loaded

        FiniteStateMachine fsm;

        CinemaController cinemaController;

        HoldActionController actionController;

        int currentSlideId = 0;

        Coroutine sliderCoroutine;

        private void Awake()
        {
            // Get the finite state machine
            fsm = GetComponent<FiniteStateMachine>();

            // Get cinema controller and set handles
            cinemaController = GetComponent<CinemaController>();
            cinemaController.OnFadeOutOnEnterComplete += HandleOnFadeOutOnEnterComplete;
            cinemaController.OnFadeOutOnExitComplete += HandleOnFadeOutOnExitComplete;
            cinemaController.OnEnter += HandleOnEnter;
            cinemaController.OnExit += HandleOnExit;
        }

        // Start is called before the first frame update
        void Start()
        {
            // If execute is set to auto and cs has not been executed then execute it
            if(executeOnEnter && fsm.CurrentStateId == (int)StateId.ToBeExecuted)
            {
                cinemaController.Enter();
            }
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        void HandleOnEnter(CinemaController ctrl)
        {
            // Start slider coroutine
            sliderCoroutine = StartCoroutine(NextSlide());
        }

        void HandleOnExit(CinemaController ctrl)
        {

            // Update finit state machine 
            fsm.Lookup();
        }

        void HandleOnFadeOutOnEnterComplete(CinemaController ctrl)
        {
            // Set the first slide on fade out completed to avoid the player see the scene
            SlideProjector.Instance.ShowSlide(slides[0].sprite);
        }

        void HandleOnFadeOutOnExitComplete(CinemaController ctrl)
        {
            SlideProjector.Instance.Hide();
        }

        IEnumerator NextSlide()
        {
            // Wait for slide length
            yield return new WaitForSeconds(slides[0].length);

            // Loop starting from the second slide
            for (int i=1; i<slides.Count; i++)
            {
                // Get the next slide
                Slide slide = slides[i];

                // Set new slide
                SlideProjector.Instance.ShowSlide(slide.sprite);

                // Wait for the next slide
                yield return new WaitForSeconds(slide.length);
            }

            // Completed
            cinemaController.Exit();
        }

        
    }

}
