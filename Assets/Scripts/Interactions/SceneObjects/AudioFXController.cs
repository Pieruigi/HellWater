using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    public class AudioFXController : MonoBehaviour
    {
        [SerializeField]
        int desiredState;

        [SerializeField]
        int desiredOldState;

        [SerializeField]
        float delay = 0;

        [SerializeField]
        float length = -1;

        [SerializeField]
        [Tooltip("Leave empty to play the default clip.")]
        AudioClip clip = null;

        [SerializeField]
        [Tooltip("Leave negative to play using the default volume.")]
        float volume = -1;

        AudioSource audioSource;

        // The finite state machine attached to this game object
        FiniteStateMachine fsm;

        
        private void Awake()
        {
            GetComponent<FiniteStateMachine>().OnStateChange += HandleOnStateChange;
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!audioSource.isPlaying || length <= 0)
                return;

            length -= Time.deltaTime;
            if (length < 0)
                audioSource.Stop();
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if((desiredOldState <= 0 || oldState == desiredOldState ) && ( desiredState <= 0 || fsm.CurrentStateId == desiredState))
            {
                // Try to set the clip
                if (clip != null)
                    audioSource.clip = clip;

                // Try to set the volume
                if (volume > 0)
                    audioSource.volume = volume;

                if (delay > 0)
                    audioSource.PlayDelayed(delay);
                else
                    audioSource.Play();
            }
        }
    }

}
