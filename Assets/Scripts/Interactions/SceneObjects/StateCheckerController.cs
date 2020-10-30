using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// This class takes into account how many fsm have been checked by the StateChecker class, and sends out an 
    /// appropriate message to the UI.
    /// </summary>
    public class StateCheckerController : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("The message id to be shown for each step.")]
        List<int> messageIds;

        // Keep trace of the finite state machines successfully checked by the state checker
        List<FiniteStateMachine> checkedList = new List<FiniteStateMachine>();

        // The state checker attached to this game object
        StateChecker stateChecker;

        // The messenger attached to this object
        Messenger messenger;

        int nextIndex = 0;

        private void Awake()
        {
            
            stateChecker = GetComponent<StateChecker>();

            // Add handles
            stateChecker.OnChecked += HandleOnChecked;

            // Get the messenger
            messenger = GetComponent<Messenger>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnChecked(StateChecker stateChecker, FiniteStateMachine fsm)
        {
            // If the next index is less than zero return
            if (nextIndex >= messageIds.Count)
                return;

            // If this fsm was already checked return
            if (checkedList.Contains(fsm))
                return;

            // Add to the succeed list
            checkedList.Add(fsm);

            // Get the next message id from the list
            int messageId = messageIds[nextIndex];

            // Update the next index
            nextIndex++;

            // If the message id is greater than zero then let the UI show the message
            if (messageId >= 0)
                messenger.ShowMessage(messageId);
        }
    }

}
