using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class InteractionTrigger : MonoBehaviour
    {
        [SerializeField]
        // By default only positive fwd is used to check position, two way also use negative which means you can
        // click whereever you want.
        bool twoWay; 
        
        [SerializeField]
        GameObject interactableObject;

        bool inside;

        IInteractable interactable;

        GameObject player;

        private void Awake()
        {
            interactable = interactableObject.GetComponent<IInteractable>();
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag(Tags.Player);
        }

        // Update is called once per frame
        void Update()
        {
            
            if (inside)
            {
                // If interaction is available then try check player position
                if (interactable.IsAvailable())
                {
                    bool canInteract = false;
                    // You must be look at the trigger positive fwd
                    if (!twoWay)
                    {
                        if (Vector3.Dot(transform.forward, player.transform.forward) > 0)
                            canInteract = true;
                    }
                    else // Just look at the center 
                    {
                        Vector3 dir = transform.position - player.transform.position;
                        if (Vector3.Dot(dir, player.transform.forward) > 0)
                            canInteract = true;
                    }

                    if (canInteract)
                    {
                        if(player.GetComponent<PlayerController>().GetActionInputDown())
                            interactable.Interact();
                    }
                        
                }
                   
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == Tags.Player)
            {
                inside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tags.Player)
                inside = false;       
                 
        }
    }

}
