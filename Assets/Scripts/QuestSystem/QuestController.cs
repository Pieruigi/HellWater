using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.QuestSystem
{
    public class QuestController : MonoBehaviour
    {

        [SerializeField]
        string toSetCurrentCode;

        [SerializeField]
        string toSetCompletedCode;

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

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            // We don't care if the state, this is not even going to be stored in cache
            if (!"".Equals(toSetCompletedCode.Trim()))
                QuestManager.Instance.SetQuestAsCompleted(toSetCompletedCode);

            if(!"".Equals(toSetCurrentCode.Trim()))
                QuestManager.Instance.SetCurrentQuest(toSetCurrentCode);
        }
    }

}
