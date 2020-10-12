using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HW.CutScene
{
    public class LoopSignalReceiver : MonoBehaviour
    {

        PlayableDirector director;

        double startTime;

        bool loopDisabled = false;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                loopDisabled = true;
                director.time = director.duration;
            }
        }

        public void SetStartTime()
        {
            startTime = director.time;
        }

        public void Loop()
        {
            if(!loopDisabled)
                director.time = startTime;
        }
    }

}
