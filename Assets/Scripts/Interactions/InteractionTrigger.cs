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

        bool inside;

        IInteractable interactable;

        //GameObject player;

        //PlayerController playerController;

        private void Awake()
        {
            interactable = interactableObject.GetComponent<IInteractable>();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            //player = GameObject.FindGameObjectWithTag(Tags.Player);
            //playerController = player.GetComponent<PlayerController>();
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
                            canInteract = true;
                    }
                    else // Just look at the center 
                    {
                        Vector3 dir = transform.position - PlayerController.Instance.transform.position;
                        if (Vector3.Dot(dir, PlayerController.Instance.transform.forward) > 0)
                            canInteract = true;
                    }

                    if (canInteract)
                    {
                        // No action required
                        if (!actionController)
                        {
                            interactable.Interact();
                        }
                        else // We need to play a little
                        {
                            if(!actionController.Acting)
                                StartActing();
                        }

                    }
                    else
                    {
                        if(actionController.Acting)
                            StopActing();
                    }    
                }
                   
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == Tags.Player)
            {
                inside = true;

                // Remove weapons
                PlayerController.Instance.HolsterWeapon();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                inside = false;
                
                if(actionController.Acting)
                    StopActing();
            }
        }

        void HandleOnActionPerformed(ActionController controller)
        {
            interactable.Interact();
            StopActing();
        }

        void StartActing()
        {
            ActionController.OnActionPerformed += HandleOnActionPerformed;
            actionController.StartActing();
        }

        void StopActing()
        {
            actionController.StopActing();
            ActionController.OnActionPerformed -= HandleOnActionPerformed;
        }

    }

}
