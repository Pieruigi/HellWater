using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class LerpTrigger : MonoBehaviour
    {

        bool inside = false;
        public bool Inside
        {
            get { return inside; }
        }

        GameObject player;

        bool positive = false;

        float length;
        Vector3 startPosition;

        bool front = false;
        bool inBounds = false;


        float t = 0;
        public float LerpFactor
        {
            get { return t; }
        }

        private void Awake()
        {
            BoxCollider coll = GetComponent<BoxCollider>();
            length = coll.size.z * transform.localScale.z;
            startPosition = transform.position - transform.forward * length / 2f;
        }

        // Start is called before the first frame update
        void Start()
        {
            player = PlayerController.Instance.gameObject;
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!inside)
                return;

            Debug.LogFormat("T:{0}", t);

            Vector3 toPlayer = player.transform.position - transform.position;
            // Project trigget to player direction on the trigger z-axis.
            Vector3 proj = Vector3.Project(toPlayer, transform.forward);

            // Check trigger bounds.
            if (proj.magnitude > length / 2f)
            {
                // Adjust t value.
                if (inBounds)
                {
                    if (t < 0.5f)
                        t = 0f;
                    else
                        t = 1f;
                }
                inBounds = false; 
                return; // Out of bounds.
            }
                

            // We need to set the starting t.
            if (!inBounds)
            {
                inBounds = true;
                // Which side is the player entering from?
                if (Vector3.Dot(proj, transform.forward) < 0)
                {
                    front = false;
                    t = 0;
                }
                else
                {
                    front = true;
                    t = 1;
                }

                //playerStartPosition = player.transform.position;
            }

            // Get the delta movement of the player.
            Vector3 delta = player.transform.position - startPosition;
            // Project delta on trigger fwd.
            Vector3 deltaPrj = Vector3.Project(delta, transform.forward);

            // Compute t.
            t = deltaPrj.magnitude / length;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                inside = true;
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                inside = false;
                inBounds = false;
            }
        }
    }

}
