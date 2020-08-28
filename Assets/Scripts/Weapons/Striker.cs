using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class Striker : MonoBehaviour, IStriker
    {

        float maxAngle = 30;
        int raysPerSide = 1;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Strike(MeleeWeapon weapon)
        {
            bool hitSomething = false;

            // Get the original ray
            Ray ray = new Ray(transform.root.position + Vector3.up * RaycastUtility.RaycastVerticalOffset, transform.root.forward);

            // Raycast
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, weapon.Range))
            {
                IHitable hitable = hit.transform.GetComponent<IHitable>();

                Debug.Log("Hitable:" + hitable);

                // If we hit an hitable object we send out hit information
                if (hitable != null)
                {
                    hitSomething = true;

                    HitInfo hitInfo = new HitInfo(hit.point, hit.normal, weapon.HitPhysicalReaction, weapon.DamageAmount, weapon.StunnedEffect);
                    hitable.GetHit(hitInfo);
                }
            }
                
            // Hit or miss event
            weapon.OnHit?.Invoke(hitSomething, weapon);
        }
    }

}
