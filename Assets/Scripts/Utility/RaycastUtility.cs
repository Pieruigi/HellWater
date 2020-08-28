using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class RaycastUtility
    {
        public static readonly float RaycastVerticalOffset = 1f;

        // Returns multiple hitable objects which are inside a given cone and are not obscured by any obstacle
        public static List<RaycastHit> GetMultipleHits(Ray ray, float distance, int raysPerSide, float maxAngle)
        {
            // Returning list
            List<RaycastHit> ret = new List<RaycastHit>();
            
            // The angle between each ray
            float angle = maxAngle / (float)raysPerSide;

            // Cast the original ray
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 2, false);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, distance))
            {
                ret.Add(hit);
            }

            // Check the other rays to the left and to the right
            for(int i=0; i<raysPerSide; i++)
            {
                // Left 
                Ray newRay = new Ray();
                newRay.origin = ray.origin;
                newRay.direction = Quaternion.Euler(0, -maxAngle * (i+1), 0) * ray.direction;

                // Cast left
                Debug.DrawRay(newRay.origin, newRay.direction * distance, Color.red, 2, false);
                RaycastHit hitLeft;
                if (Physics.Raycast(newRay, out hitLeft, distance))
                {
                    if(!ret.Exists(r=>r.transform == hitLeft.transform)) // Avoid to hit the same object twice
                        ret.Add(hitLeft);
                }
                
                // Right
                newRay.direction = Quaternion.Euler(0, maxAngle * (i+1), 0) * ray.direction;

                // Cast left
                Debug.DrawRay(newRay.origin, newRay.direction * distance, Color.red, 2, false);
                RaycastHit hitRight;
                if (Physics.Raycast(newRay, out hitRight, distance))
                {
                    if (!ret.Exists(r => r.transform == hitRight.transform)) // Avoid to hit the same object twice
                        ret.Add(hitRight);
                }
            }

            return ret;
        }


        public static RaycastHit GetHit(Ray ray, float distance)
        {
            RaycastHit hit;
            Physics.Raycast(ray, out hit, distance);
            return hit;
        }

    }



}
