using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class _TestTimeline : MonoBehaviour
{
    [SerializeField]
    PlayableDirector timeline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            timeline.Play();
        }

        if (Input.GetKeyDown(KeyCode.S))
            timeline.time = 54.53f;
    }
}
