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

        #endregion


        public HitInfo(Vector3 position, Vector3 normal, HitPhysicalReaction physicalReaction, float damageAmount)
        {
            this.position = position;
            this.normal = normal;
            this.physicalReaction = physicalReaction;
            this.damageAmount = damageAmount;
        }


        public override string ToString()
        {
            return string.Format("[HitInfo position:{0}, normal:{1}, physicalReaction:{2}, damageAmount:{3}]",
                position, normal, physicalReaction, damageAmount);
        }
    }

}
