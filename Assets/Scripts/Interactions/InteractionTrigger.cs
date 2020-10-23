using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class InteractionTrigger : MonoBehaviour
    {
        // By default only positive fwd is used to check position, two way also use negative which means you can
        // click whereever you want.
        [SerializeField]
        bool twoWay;

        [SerializeField]
        GameObject interactableObject;

        [SerializeField]
        ActionController actionController;

        bool inside = false;

        IInteractable interactable;

        private void Awake()
        {
            interactable = interactableObject.GetComponent<IInteractable>();

            if(actionController)
                actionController.OnActionPerformed += HandleOnActionPerformed;
        }

        // Start is called before the first frame update
        void Start()
        {
        
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
                        if (Vector3.Dot(transform.forward, PlayerController.Instance.transform.forward) > 0)
                        {
                            // Player must stay behind the trigger 
                            Vector3 dir = transform.position - PlayerController.Instance.transform.position;
                            if(Vector3.Dot(dir, transform.forward) > 0)
                                canInteract = true;
                        }
                            
                    }
                    else // Just look at the center 
                    {
                        Vector3 dir = transform.position - PlayerController.Instance.transform.position;
                        if (Vector3.Dot(dir, PlayerController.Instance.transform.forward) >= 0)
                            canInteract = true;
                    }

                    if (canInteract)
                    {
                        // No action required
                        if (!actionController)
                        {
                            interactable.Interact();
                        }
                   
                    }
                   
                    // Try to enable or disable action controller
                    if (actionController)
                    {
                        if (canInteract)
                            actionController.EnableAction();
                        else
                            actionController.DisableAction();
                    }
                    
                }
                   
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == Tags.Player)
            {
                inside = true;
             
                if(actionController)
                    actionController.DisableAction();

                // Remove weapons
                PlayerController.Instance.HolsterWeapon();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                inside = false;

                if (actionController)
                    actionController.DisableAction();
            }
        }

        void HandleOnActionPerformed(ActionController controller)
        {
            
            interactable.Interact();

            if (actionController)
                actionController.DisableAction();
        }

    }

}
