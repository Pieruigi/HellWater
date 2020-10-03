using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class TrackController : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    TrackBehaviour template;// = new TrackBehaviour();

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<TrackBehaviour>.Create(graph, template);
    }
}
