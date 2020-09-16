using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;

namespace HW
{
    public class Messenger : MonoBehaviour
    {
        private void Awake()
        {
            FiniteStateMachine fsm = GetComponent<FiniteStateMachine>();
            if (fsm)
                fsm.OnFail += HandleOnFail;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnFail(FiniteStateMachine fsm, int errorId)
        {
            // Get the right message depending on the id and language
            string message = MessageFactory.Instance.GetMessage(errorId);

            // Show message on screen
            MessageViewer.Instance.ShowMessage(message);
        }

    }
}

