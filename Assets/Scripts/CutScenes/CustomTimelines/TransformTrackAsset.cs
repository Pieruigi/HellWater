using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace HW.CutScene
{
    [TrackColor(1, 0, 0)]
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(TransformTrackController))]
    public class TransformTrackAsset : TrackAsset
    {

    }
}
