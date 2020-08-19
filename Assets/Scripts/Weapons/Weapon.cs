using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Common Stats")]
        [SerializeField]
        float damage;
        public float Damage
        {
            get { return damage; }
        }

        [SerializeField]
        float range;
        public float Range
        {
            get { return range; }
        }

        [Header("Animation")]
        [SerializeField]
        int animationId = 0; // 1:bat; 2:gun; 3:shotgun; 4:combat rifle; 5:rifle
        public int AnimationId
        {
            get { return animationId; }
        }

        // Start is called before the first frame update
        void Start()
        {
            
            SetVisible(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetVisible(bool value)
        {
            transform.GetChild(0).gameObject.SetActive(value);
        }

        public bool IsVisible()
        {
            return transform.GetChild(0).gameObject.activeSelf;
        }
    }

}
