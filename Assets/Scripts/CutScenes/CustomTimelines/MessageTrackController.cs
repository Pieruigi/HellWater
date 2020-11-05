using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace HW.CutScene
{
    [System.Serializable]
    public class MessageTrackController : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        MessageTrackBehaviour template;// = new TrackBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<MessageTrackBehaviour>.Create(graph, template);
        }
    }

}
