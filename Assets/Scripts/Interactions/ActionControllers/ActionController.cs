﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    //public enum PlayerActionControllerType { Press, Hold, Repeat }

    public class ActionController : MonoBehaviour
    {
        public static UnityAction<ActionController> OnStartActing;
        public static UnityAction<ActionController> OnStopActing;
        public static UnityAction<ActionController> OnActionPerformed;

        bool acting = false;
        public bool Acting
        {
            get { return acting; }
        }

        //protected abstract bool PerformAction();

        PlayerController playerController;
        protected PlayerController PlayerController
        {
            get { return playerController; }
        } 

        // Start is called before the first frame update
        void Start()
        {
            playerController = GameObject.FindObjectOfType<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!acting)
                return;

            if (PerformAction())
            {
                StopActing();
                OnActionPerformed?.Invoke(this);
            }
        }

        public void StartActing()
        {
            acting = true;

            OnStartActing?.Invoke(this);
        }

        public void StopActing()
        {
            acting = false;
            OnStopActing?.Invoke(this);
        }

        protected virtual bool PerformAction()
        {
            if (playerController.GetActionDown())
                return true;

            return false;
        }
    }

}