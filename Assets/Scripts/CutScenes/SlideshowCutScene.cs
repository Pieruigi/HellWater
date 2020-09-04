using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;
using HW.Interfaces;

namespace HW.Cinema
{
    public class SlideshowCutScene : CutScene, ISkippable
    {
        

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

        int currentSlideId = 0;

        Coroutine sliderCoroutine;

        // Update is called once per frame
        void Update()
        {
           
        }

        protected override void HandleOnEnter(CinemaController ctrl)
        {
            base.HandleOnEnter(ctrl);

            // Start slider coroutine
            sliderCoroutine = StartCoroutine(NextSlide());
        }


        protected override void HandleOnFadeOutOnEnterComplete(CinemaController ctrl)
        {
            base.HandleOnFadeOutOnEnterComplete(ctrl);

            // Set the first slide on fade out completed to avoid the player see the scene
            SlideProjector.Instance.ShowSlide(slides[0].sprite);
        }

        protected override void HandleOnFadeOutOnExitComplete(CinemaController ctrl)
        {
            base.HandleOnFadeOutOnExitComplete(ctrl);

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

            Exit();
        }

        
    }

}
