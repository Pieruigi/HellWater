using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    [System.Serializable]
    public class AudioClipData
    {
        [SerializeField]
        AudioClip clip;
        public AudioClip Clip
        {
            get { return clip; }
        }

        [SerializeField]
        float volume = 1;
        public float Volume
        {
            get { return volume; }
        }
    }

}
