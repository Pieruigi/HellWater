using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class ActivationTrigger : MonoBehaviour
    {
        [SerializeField]
        GameObject interactable;

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
            if(other.transform.tag == Tags.Player)
            {
                interactable.GetComponent<IInteractable>().Interact();
            }
        }
    }

}
