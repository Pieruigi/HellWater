using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.QuestSystem
{

    /// <summary>
    /// Attach this class to a finit state machine to update the quest manager on state change.
    /// Leave the next quest code empty to reset the quest manager leaving the player with no quest at all.
    /// </summary>
    public class QuestController : MonoBehaviour
    {

        [SerializeField]
        int desiredState = 0;

        // We may want a particular transition in order to activate the new quest.
        // We could keep interacting with the same object thus updating the quest even if the object stay in the same state.
        [SerializeField]
        int desiredOldState = -1; 

        [SerializeField]
        string nextQuestCode;

        FiniteStateMachine fsm;

        private void Awake()
        {
            GetComponent<FiniteStateMachine>().OnStateChange += HandleOnStateChange;
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
        /// Set the next quest on the quest manager.
        /// If the nextQuestCode field is empty quest will be removed.
        /// </summary>
        /// <param name="fsm"></param>
        /// <param name="oldState"></param>
        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // If the fsm is not in the desired state or is coming from a different state than the desired one then return.
            if (fsm.CurrentStateId != desiredState || (desiredOldState >= 0 && fsm.PreviousStateId != desiredOldState))
                return;

            // Update quest manager.
            // If code is empty the reset, otherwise set the new quest as the active one
            if (string.IsNullOrEmpty(nextQuestCode))
                QuestManager.Instance.ResetCurrentQuest();
            else
                QuestManager.Instance.SetCurrentQuest(nextQuestCode);

        }
    }

}
