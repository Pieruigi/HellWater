﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW
{
    public class Messenger : MonoBehaviour
    {
        [SerializeField]
        float delay = 0;

        private void Awake()
        {
            FiniteStateMachine fsm = GetComponent<FiniteStateMachine>();
            if (fsm)
            {
               // Debug.Log(name + " - Messengerrrrrrrrrrrrrrrrrrrrrrrrrr");
                fsm.OnFail += HandleOnFail;
                fsm.OnStateChange += HandleOnStateChange;
            }
                
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            Debug.Log("fsm.LastErrorCode:" + fsm.LastExitCode);
            if (fsm.LastExitCode < 0)
                return;

            // Get the right message depending on the id and language
            string message = MessageFactory.Instance.GetMessage(fsm.LastExitCode);

            // Show message on screen
            //MessageViewer.Instance.ShowMessage(message);
            StartCoroutine(ShowMessage(message, delay));
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            Debug.Log("fsm.LastExitCode:" + fsm.LastExitCode);
            if (fsm.LastExitCode < 0)
                return;

            // Get the right message depending on the id and language
            string message = MessageFactory.Instance.GetMessage(fsm.LastExitCode);


            // Show message on screen
            //MessageViewer.Instance.ShowMessage(message);
            StartCoroutine(ShowMessage(message, delay));
        }

        IEnumerator ShowMessage(string message, float delay)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            MessageViewer.Instance.ShowMessage(message);
        }
    }
}

