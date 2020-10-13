using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    public class AmbientAudioManager : MonoBehaviour
    {
        [SerializeField]
        AudioSource source1;

        [SerializeField]
        AudioSource source2;

        //[SerializeField]
        float fadeTime = 5f; // How much time it will keep the ambient to switch between sources

        bool switching = false;
        
        AudioSource sourceIn;
        AudioSource sourceOut;

        float volumeIn;
        //float volumeOut;
        float speedIn;
        float speedOut;

        public static AmbientAudioManager Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                source1.volume = 0;
                source2.volume = 0;

                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Switching clip
            if (switching)
            {
                sourceIn.volume = Mathf.MoveTowards(sourceIn.volume, volumeIn, Time.deltaTime * speedIn);

                if (sourceOut)
                    sourceOut.volume = Mathf.MoveTowards(sourceOut.volume, 0, Time.deltaTime * speedOut);

                if (sourceIn.volume == 0 && (!sourceOut || sourceOut.volume == 0))
                {
                    switching = false;
                    if (sourceOut)
                        sourceOut.Stop();
                }
                    
            }
        }

        // Sets the clip to play 
        public void SwitchAmbientSource(AudioClip clip, float volume, float fade = 1)
        {
            fadeTime = fade;

            // If we are already playing the clipIn then do nothing...
            if (sourceIn != null && sourceIn.clip == clip)
                return;

            // ... otherwise switch clips
            switching = true;

            // We need the sourceIn to fade out
            if (sourceIn)
            {
                sourceOut = sourceIn; // Switching
            }

            // Set the new in source ( which is the other source )
            if (sourceIn == source1)
            {
                sourceIn = source2;
            }
            else
            {
                sourceIn = source1;
            }

            // Set clip 
            sourceIn.clip = clip;
            if (sourceIn.isPlaying)
                sourceIn.Stop();

            sourceIn.Play();

            volumeIn = volume;

            // Setting volume for the in source
            speedIn = (volume - sourceIn.volume) / fadeTime;

            // Setting the volume for the out source
            if (sourceOut)
                speedOut = sourceOut.volume / fadeTime;
           

            // Set switching flag
            switching = true;
        }
    }

}
