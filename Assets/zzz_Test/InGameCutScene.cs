using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Playables;

namespace HW.Cinema
{
    public class InGameCutScene : CutScene
    {
        PlayableDirector timeline;

        protected override void Awake()
        {
            base.Awake();

            timeline = GetComponent<PlayableDirector>();
        }

        public void TestDebug()
        {
            Debug.Log("Animation started...");
        }

        protected override void HandleOnFadeOutOnEnterComplete(CinemaController ctrl)
        {
            base.HandleOnFadeOutOnEnterComplete(ctrl);

            timeline.Play();
        }


        protected override void HandleOnFadeOutOnExitComplete(CinemaController ctrl)
        {
            base.HandleOnFadeOutOnExitComplete(ctrl);

            timeline.Stop();
        }


    }

}
