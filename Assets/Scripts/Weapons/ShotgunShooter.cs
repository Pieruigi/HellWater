using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class ShotgunShooter : MonoBehaviour, IShooter
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

        public void Shoot(FireWeapon weapon)
        {
            // Get the original ray
            Ray ray = new Ray(transform.root.position + Vector3.up * RaycastUtility.RaycastVerticalOffset, transform.root.forward);

            // Get multiple hits
            List<RaycastHit> hits = RaycastUtility.GetMultipleHits(ray, weapon.Range, raysPerSide, maxAngle);
            

            foreach(RaycastHit hit in hits)
            {
                // Get the hitable object if exists
                IHitable hitable = hit.transform.GetComponent<IHitable>();

                // If we hit an hitable object we send out hit information
                if (hitable != null)
                {
                    HitInfo hitInfo = new HitInfo(hit.point, hit.normal, weapon.HitPhysicalReaction, weapon.DamageAmount, weapon.StunnedEffect);
                    hitable.GetHit(hitInfo);
                }
                
            }

            
        }
    }

}
