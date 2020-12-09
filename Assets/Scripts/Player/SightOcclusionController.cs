using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
   
    public class SightOcclusionController : MonoBehaviour
    {

        [SerializeField]
        bool useDithering = false;

        bool active = true;

        CapsuleCollider coll;

        [SerializeField]
        List<Transform> hitList;

        private void Awake()
        {
            coll = GetComponent<CapsuleCollider>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            if (!active)
                return;

            if (!Camera.main)
                return;

            //
            // Do raycast
            //
            RaycastHit[] hits;
            // Get direction from player to main camera
            Vector3 dir = Camera.main.transform.position - transform.position;
            Ray ray = new Ray(transform.position + Vector3.up*coll.height*0.5f, dir.normalized);

            // Get mask
            int mask = LayerMask.GetMask(Layers.SightOccluder);
            float radius = 0.275f;
            hits = Physics.SphereCastAll(ray, radius, dir.magnitude, mask);

            
            // Process hit objects
            if(hits.Length > 0)
            {
                foreach(RaycastHit hit in hits)
                {
                    // If it has not been hit yet then add to the hit list and process it
                    if (!hitList.Contains(hit.transform))
                    {
                        hitList.Add(hit.transform);
                        HideOccluder(hit.transform);
                    }
                }

                // Check the previous hit
                List<Transform> remList = new List<Transform>();
                List<RaycastHit> tmpList = new List<RaycastHit>(hits);
                foreach(Transform t in hitList)
                {
                    // Current transform has not been hit, remove it
                    if(!tmpList.Exists(r=>r.transform == t))
                        remList.Add(t);
                    
                }

                // Remove all the objects in the hit list which are contained in the rem list
                foreach(Transform t in remList)
                {
                    ShowOccluder(t);
                    hitList.Remove(t);
                }
            }
            else // Nothing has been hit, so clear the hit list
            {
                foreach(Transform hit in hitList)
                {
                    ShowOccluder(hit);
                }

                hitList.Clear();
            }


           
          
        }

        void HideOccluder(Transform occluder)
        {
            occluder.GetComponent<Occluder>().Hide();
        }

        void ShowOccluder(Transform occluder)
        {
            occluder.GetComponent<Occluder>().Show();
        }
    }

}
