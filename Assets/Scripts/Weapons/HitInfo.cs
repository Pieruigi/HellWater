using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum HitPhysicalReaction { None, Stop, Push }

    public class HitInfo
    {
        #region HIT REACTION
        Vector3 position;
        public Vector3 Position
        {
            get { return position; }
        }

        Vector3 normal;
        public Vector3 Normal
        {
            get { return normal; }
        }

        HitPhysicalReaction physicalReaction;
        public HitPhysicalReaction PhysicalReaction
        {
            get { return physicalReaction; }
        }
        #endregion


        #region DAMAGE
        float damageAmount;
        public float DamageAmount
        {
            get { return damageAmount; }
        }

        bool stunnedEffect;
        public bool StunnedEffect
        {
            get { return stunnedEffect; }
        }
        #endregion


        public HitInfo(Vector3 position, Vector3 normal, HitPhysicalReaction physicalReaction, float damageAmount, bool stunnedEffect)
        {
            this.position = position;
            this.normal = normal;
            this.physicalReaction = physicalReaction;
            this.damageAmount = damageAmount;
            this.stunnedEffect = stunnedEffect;
        }
    }

}
