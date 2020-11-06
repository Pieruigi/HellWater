using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;
using System;

namespace HW
{
    public class Scratcher : MonoBehaviour, IFighter
    {
        [SerializeField]
        float attackCooldown;

        [SerializeField]
        float attackRange;

        [SerializeField]
        float damageAmount;

        DateTime lastAttack;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Fight(Transform target)
        {
            // Set last time for cooldown.
            lastAttack = DateTime.UtcNow;

            // Get the original ray
            Ray ray = new Ray(transform.root.position + Vector3.up * Constants.RaycastVerticalOffset, transform.root.forward);

            // Raycast
            RaycastHit hit;
            //if (Physics.SphereCast(ray, 0.75f, out hit, attackRange))
            if (Physics.Raycast(ray, out hit, attackRange))
            {
                IHitable hitable = hit.transform.GetComponent<IHitable>();

                Debug.Log("Hitable:" + hitable);

                // If we hit an hitable object we send out hit information
                if (hitable != null)
                {
                    HitInfo hitInfo = new HitInfo(hit.point, hit.normal, HitPhysicalReaction.Stop, damageAmount, false);
                    hitable.GetHit(hitInfo);
                }
            }

            return true;
        }

        public float GetFightingRange()
        {
            return attackRange;
        }

        public bool AttackAvailable()
        {
            // We need to wait for the attack cooldown.
            if ((DateTime.UtcNow - lastAttack).TotalSeconds < attackCooldown)
                return false;
            else
                return true;
        }

    }

}
