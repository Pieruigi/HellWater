﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using System;
using UnityEngine.Events;

namespace HW
{
    public class InteractionController : MonoBehaviour, IInteractable
    {
        //public UnityAction<int, int> OnStateChange;


        [SerializeField]
        List<int> unavailableStates = new List<int>();

        FiniteStateMachine fsm;
               

        DateTime lastInteractionTime;

        bool disabled = false;

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

        public void ForceDisabled(bool value)
        {
            disabled = value;
        }

        public void Interact()
        {
            Debug.Log("Interact...");

            if (!IsAvailable())
                return;

            lastInteractionTime = DateTime.UtcNow;

            fsm.Lookup();
        }

        public bool IsAvailable()
        {
            if (disabled)
                return false;

            // Cooldown wait
            if ((DateTime.UtcNow - lastInteractionTime).TotalSeconds < Constants.InteractionCooldownTime)
                return false;

            // State machine is unavailable
            if (fsm.CurrentStateId == -1 || unavailableStates.Contains(fsm.CurrentStateId))
                return false;

            return true;
        }

    }

}
