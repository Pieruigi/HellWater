using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HW.CutScene
{
    [System.Serializable]
    public class TransformTrackBehaviour : PlayableBehaviour
    {
        Transform target;
        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // Get the object attached to the track
            Transform t = playerData as Transform;

            // Set the object transform
            t.position = target.position;
            t.rotation = target.rotation;
            
        }
    }
}
