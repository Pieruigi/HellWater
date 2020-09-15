using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW.Cinema
{
    // This class holds basic signal reaction implementation
    public class CutSceneSignalReactor : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FadeOut(float speed)
        {
            CameraFader.Instance.FadeOut(speed);
        }

        public void FadeIn(float speed)
        {
            CameraFader.Instance.FadeIn(speed);
        }

        public void ForceBlackScreen()
        {
            CameraFader.Instance.ForceBlackScreen();
        }

        public void PlayerEnable(bool value)
        {
            PlayerController.Instance.SetDisabled(!value);
        }

      
        public void PlayerShow(bool value)
        {
            PlayerController.Instance.gameObject.SetActive(value);
        }

        public void ShowSlide(Sprite sprite)
        {
            SlideProjector.Instance.ShowSlide(sprite);
        }
    }

}
