using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class RendererActivator : MonoBehaviour
    {
      
        [SerializeField]
        bool children = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate(bool value)
        {
            SetVisible(value);
        }

        private void SetVisible(bool value)
        {
            if (children)
            {
                MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer rend in renderers)
                        rend.enabled = value;
                    
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = value;
            }

        }
    }

}
