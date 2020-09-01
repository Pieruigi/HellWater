using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        Item item;
        public Item Item
        {
            get { return item; }
        }

        [SerializeField]
        float damageAmount; // The amount of damage delivered
        public float DamageAmount
        {
            get { return damageAmount; }
        }

        [SerializeField]
        bool stunnedEffect; // Does the enemy remain stunned ?
        public bool StunnedEffect
        {
            get { return stunnedEffect; }
        }

        [SerializeField]
        HitPhysicalReaction hitPhysicalReaction; // Does the enemy stop moving or get pushed ?
        public HitPhysicalReaction HitPhysicalReaction
        {
            get { return hitPhysicalReaction; }
        }

        [SerializeField]
        float range;
        public float Range
        {
            get { return range; }
        }

        [SerializeField]
        int animationId = 0; // 1:bat; 2:gun; 3:shotgun; 4:combat rifle; 5:rifle
        public int AnimationId
        {
            get { return animationId; }
        }

        [SerializeField]
        GameObject weaponObject;

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            
            SetVisible(false);
        }

        // Update is called once per frame
        

        public void SetVisible(bool value)
        {
            weaponObject.SetActive(value);
        }

        public bool IsVisible()
        {
            return weaponObject.activeSelf;
        }
    }

}
