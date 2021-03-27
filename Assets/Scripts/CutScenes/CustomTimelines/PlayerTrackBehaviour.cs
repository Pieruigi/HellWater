using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HW.CutScene
{
    [System.Serializable]
    public class PlayerTrackBehaviour : PlayableBehaviour
    {
        [SerializeField]
        bool disabled = false;

        [SerializeField]
        bool noWeapon = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // Get the object attached to the track
            PlayerController p = playerData as PlayerController;

            p.SetDisabled(disabled);
            p.HolsterWeapon(noWeapon);
        }
    }
}