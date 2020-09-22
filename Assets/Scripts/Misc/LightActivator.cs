using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class LightActivator : MonoBehaviour
    {
      
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
            Debug.Log("Activate lights:" + value);
            SetVisible(value);
        }

        private void SetVisible(bool value)
        {
            gameObject.SetActive(value);
        }
    }

}
