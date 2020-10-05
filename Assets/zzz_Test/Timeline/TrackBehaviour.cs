using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TrackBehaviour : PlayableBehaviour
{
    [SerializeField]
    string text;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {

        GameObject g = playerData as GameObject;
        g.transform.position += g.transform.forward*0.03f;
    }
}
