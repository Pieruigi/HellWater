using System.Collections;
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

        /// <summary>
        /// Send the corresponding message to the UI.
        /// </summary>
        /// <param name="messageId">The id of the message you want to show.</param>
        public void ShowMessage(int messageId)
        {
            if(messageId < 0)
            {
                Debug.LogErrorFormat("Messenger has been receiving a wrong message id: {0}", messageId);
                return;
            }

            // Get the right message depending on the id and language
            string message = MessageFactory.Instance.GetMessage(messageId);

            //MessageViewer.Instance.ShowMessage(message);
            StartCoroutine(ShowMessage(message, delay));
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

        void HandleOnStateChange(FiniteStateMachine fsm)
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

