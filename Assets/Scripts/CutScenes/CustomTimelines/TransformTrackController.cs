using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace HW.CutScene
{
    [System.Serializable]
    public class TransformTrackController : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        TransformTrackBehaviour template;// = new TrackBehaviour();

        [SerializeField]
        string targetName;

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            // Get target from owner children.
            Transform[] tlist = owner.GetComponentsInChildren<Transform>();
            Transform target = new List<Transform>(tlist).Find(t => t.gameObject.name.ToLower().Equals(targetName.ToLower()));

            // Set target on template.
            template.SetTarget(target);

            return ScriptPlayable<TransformTrackBehaviour>.Create(graph, template);
        }
    }

}