using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class LightActivatorController : MonoBehaviour
    {
        [SerializeField]
        List<LightActivator> objects;


        private void Awake()
        {
            Deactivate();
        }
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
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            

            Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            Deactivate();
        }

        void Activate()
        {
            foreach (LightActivator o in objects)
            {
                o.Activate(true);
            }
        }

        void Deactivate()
        {
            foreach (LightActivator o in objects)
            {
                o.Activate(false);
            }
        }
    }

}
