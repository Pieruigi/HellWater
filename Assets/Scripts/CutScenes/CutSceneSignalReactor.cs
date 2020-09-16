using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW.Cinema
{
    // This class holds basic signal reaction implementation
    public class CutSceneSignalReactor : MonoBehaviour
    {
        private float fadeSpeed = 1f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FadeOut()
        {
            CameraFader.Instance.FadeOut(fadeSpeed);
        }

        public void FadeIn()
        {
            CameraFader.Instance.FadeIn(fadeSpeed);
        }

        public void ForceBlackScreen()
        {
            CameraFader.Instance.ForceBlackScreen();
        }

        public void PlayerEnable()
        {
            PlayerController.Instance.SetDisabled(false);
        }

        public void PlayerDisable()
        {
            PlayerController.Instance.SetDisabled(true);
        }
      
        public void PlayerShow()
        {
            PlayerController.Instance.gameObject.SetActive(true);
        }

        public void PlayerHide()
        {
            PlayerController.Instance.gameObject.SetActive(false);
        }

        public void NextSlide()
        {
            //SlideProjector.Instance.ShowSlide(sprite);
        }
    }

}
