using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class _TestSound : MonoBehaviour
{
    

    [SerializeField]
    AudioSource source1;
    [SerializeField]
    int source1ClipsElementId = -1;
    [SerializeField]
    float source1Volume = 1;

    [SerializeField]
    AudioSource source2;
    [SerializeField]
    int source2ClipsElementId = -1;
    [SerializeField]
    float source2Volume = 1;

    [SerializeField]
    AudioSource source3;
    [SerializeField]
    int source3ClipsElementId = -1;
    [SerializeField]
    float source3Volume = 1;

    [SerializeField]
    AudioSource source4;
    [SerializeField]
    int source4ClipsElementId = -1;
    [SerializeField]
    float source4Volume = 1;

    [SerializeField]
    AudioSource source5;
    [SerializeField]
    int source5ClipsElementId = -1;
    [SerializeField]
    float source5Volume = 1;

    [SerializeField]
    List<AudioClip> clips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(source1ClipsElementId >= 0)
            {
                source1.clip = clips[source1ClipsElementId];
                source1.volume = source1Volume;
                source1.Play();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (source2ClipsElementId >= 0)
            {
                source2.clip = clips[source2ClipsElementId];
                source2.volume = source2Volume;
                source2.Play();
            }
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (source3ClipsElementId >= 0)
            {
                source3.clip = clips[source3ClipsElementId];
                source3.volume = source3Volume;
                source3.Play();
            }

            
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (source4ClipsElementId >= 0)
            {
                source4.clip = clips[source4ClipsElementId];
                source4.volume = source4Volume;
                source4.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (source5ClipsElementId >= 0)
            {
                source5.clip = clips[source5ClipsElementId];
                source5.volume = source5Volume;
                source5.Play();
            }
        }
    }
}
