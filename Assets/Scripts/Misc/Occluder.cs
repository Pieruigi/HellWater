using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class Occluder : MonoBehaviour
    {
        [SerializeField]
        Material placeholderMaterial;

        Renderer rend;

        Renderer childRend;

        void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Create the placeholder under the current
            GameObject g = new GameObject();
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;
            MeshFilter mf = g.AddComponent<MeshFilter>();
            mf.sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            MeshRenderer r = g.AddComponent<MeshRenderer>();
            r.receiveShadows = false;
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.sharedMaterial = placeholderMaterial;

            // Set and disable the place older
            childRend = r;
            childRend.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
         
        }

        public void Show()
        {
            rend.enabled = true;
            childRend.enabled = false;
        }
        
        public void Hide()
        {

            rend.enabled = false;
            childRend.enabled = true;

           
        }
    }

}
