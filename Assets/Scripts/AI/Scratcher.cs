using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public class Scratcher : MonoBehaviour, IFighter
    {


        [SerializeField]
        float attackRange;

        [SerializeField]
        float damageAmount;


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
            // Get the original ray
            Ray ray = new Ray(transform.root.position + Vector3.up * Constants.RaycastVerticalOffset, transform.root.forward);

            // Raycast
            RaycastHit hit;
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


    }

}
