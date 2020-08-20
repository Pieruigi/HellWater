using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    // Only works with fire weapons and uses raycast in order to hit enemies.
    public class Shooter : MonoBehaviour, IShooter
    {
       
        public void Shoot(FireWeapon fireWeapon)
        {
            Debug.Log("Shoot");

            // Raycast 
            RaycastHit hit;
            Ray ray = new Ray(transform.root.position, transform.root.forward);
            if(Physics.Raycast(ray, out hit, fireWeapon.Range))
            {
                Debug.Log("Hit:" + hit.transform.name);

                // Check if the object we hit has a hitable object
                IHitable hitable = hit.transform.GetComponent<IHitable>();

                // Yes, send hit info
                if (hitable != null)
                {
                    HitInfo hitInfo = new HitInfo(hit.point, hit.normal, fireWeapon.HitPhysicalReaction, fireWeapon.DamageAmount, fireWeapon.StunnedEffect);
                    hitable.Hit(hitInfo);
                }
            }

            
        }

    }

}
