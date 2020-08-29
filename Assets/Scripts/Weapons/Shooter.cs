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

            if (!GameObject.FindObjectOfType<PlayerController>().FireWeaponAccuracySystem)
            {
                // Raycast 
                RaycastHit hit;
                Ray ray = new Ray(transform.root.position+Vector3.up* Constants.RaycastVerticalOffset, transform.root.forward);
                if (Physics.Raycast(ray, out hit, fireWeapon.Range))
                {
                    Debug.Log("Hit:" + hit.transform.name);

                    // Check if the object we hit has a hitable object
                    IHitable hitable = hit.transform.GetComponent<IHitable>();

                    // Yes, send hit info
                    if (hitable != null)
                    {
                        HitInfo hitInfo = new HitInfo(hit.point, hit.normal, fireWeapon.HitPhysicalReaction, fireWeapon.DamageAmount, fireWeapon.StunnedEffect);
                        hitable.GetHit(hitInfo);
                    }
                }
            }
            else // Accuracy system
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.root.position + Vector3.up * Constants.RaycastVerticalOffset, transform.root.forward);
                if (Physics.Raycast(ray, out hit, FireWeapon.GlobalAimingRange))
                {
                    Debug.Log("Hit:" + hit.transform.name);

                    // Check if the object we hit has a hitable object
                    IHitable hitable = hit.transform.GetComponent<IHitable>();

                    // Yes, send hit info
                    if (hitable != null)
                    {
                        float distance = ((hitable as MonoBehaviour).transform.position - transform.root.position).magnitude;

                        float acc = 1 - FireWeapon.GetAccuracyPenalty(fireWeapon, distance);
                        bool accSucceeded = (Random.Range(0f, 1f) < acc);

                        if (accSucceeded)
                        {
                            HitInfo hitInfo = new HitInfo(hit.point, hit.normal, fireWeapon.HitPhysicalReaction, fireWeapon.DamageAmount, fireWeapon.StunnedEffect);
                            hitable.GetHit(hitInfo);
                        }
                        
                    }
                }
            }


            
        }

    }

}
