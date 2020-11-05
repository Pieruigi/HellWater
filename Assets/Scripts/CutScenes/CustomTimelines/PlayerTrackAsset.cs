using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace HW.CutScene
{
    [TrackColor(1, 0, 0)]
    [TrackBindingType(typeof(PlayerController))]
    [TrackClipType(typeof(PlayerTrackController))]
    public class PlayerTrackAsset : TrackAsset
    {

    }
}
