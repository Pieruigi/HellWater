using HW.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class ItemRemover : MonoBehaviour
    {
        [SerializeField]
        Item itemToRemove;

        [SerializeField]
        int desiredState = -1;

        [SerializeField]
        int desiredOldState = -1;

        // The attached finite state machine.
        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (desiredState >= 0 && fsm.CurrentStateId != desiredState)
                return;// Wrong state.

            if (desiredOldState >= 0 && oldState != desiredOldState)
                return; // Wrong old state.

            // Remove the item.
            Inventory.Instance.Remove(itemToRemove);
        }
    }

}
