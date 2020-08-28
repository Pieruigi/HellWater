using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class BackgroundLayerTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Entering layer:"+other.gameObject);
            // Get the ILayerable interface
            ILayerable layerable = other.gameObject.GetComponent<ILayerable>();
            
            // No ILayerable behaviour then return
            if (layerable == null)
                return;

            // Move to background
            layerable.MoveToBackground();
        }

        private void OnTriggerExit(Collider other)
        {
            // Get the ILayerable interface
            ILayerable layerable = other.gameObject.GetComponent<ILayerable>();

            // No ILayerable behaviour then return
            if (layerable == null)
                return;

            // Move to foreground
            layerable.MoveToForeground();
        }
    }

}
