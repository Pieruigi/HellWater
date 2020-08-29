using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using System;

namespace HW
{
    public class InteractionController : MonoBehaviour, IInteractable
    {
        public static readonly float CooldownTime = 1;

        [SerializeField]
        List<int> unavailableStates = new List<int>();

        FiniteStateMachine fsm;
               

        DateTime lastInteractionTime;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Interact()
        {
            Debug.Log("Interact...");

            if ((DateTime.UtcNow - lastInteractionTime).TotalSeconds < CooldownTime)
                return;

            lastInteractionTime = DateTime.UtcNow;

            fsm.Lookup();
        }

        public bool IsAvailable()
        {
            // Cooldown wait
            if ((DateTime.UtcNow - lastInteractionTime).TotalSeconds < CooldownTime)
                return false;

            // State machine is unavailable
            if (fsm.CurrentStateId == -1 || unavailableStates.Contains(fsm.CurrentStateId))
                return false;

            return true;
        }
    }

}
