using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using HW;

public class _TestAmbientAudio : MonoBehaviour
{
    [SerializeField]
    AudioClip clip1;

    [SerializeField]
    float volume1 = 1;

    [SerializeField]
    AudioClip clip2;

    [SerializeField]
    float volume2 = 1;

    [SerializeField]
    float fadeTime = 1;

    AudioClip current;

    // Start is called before the first frame update
    void Start()
    {
        current = clip1;
        AmbientAudioManager.Instance.SwitchAmbientSource(clip1, volume1, fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(current == clip1)
            {
                AmbientAudioManager.Instance.SwitchAmbientSource(clip2, volume2, fadeTime);
                current = clip2;
            }
            else
            {
                AmbientAudioManager.Instance.SwitchAmbientSource(clip1, volume1, fadeTime);
                current = clip1;
            }
        }
    }
}
