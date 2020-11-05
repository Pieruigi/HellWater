using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace HW.CutScene
{
    [System.Serializable]
    public class PlayerTrackController : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        PlayerTrackBehaviour template;// = new TrackBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<PlayerTrackBehaviour>.Create(graph, template);
        }
    }

}