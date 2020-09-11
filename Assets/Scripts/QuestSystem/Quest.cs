using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HW.QuestSystem
{
   
    public enum QuestType { Main, Optional }

    public class Quest: MonoBehaviour
    {
        public static readonly int TaskCompletedStateId = 0;

        [System.Serializable]
        class Goal
        {
            [SerializeField]
            List<Task> tasks;
        }

        [System.Serializable]
        class Task
        {
            [SerializeField]
            string description;

            [SerializeField]
            FiniteStateMachine fsm;

            public bool IsCompleted()
            {
                return fsm.CurrentStateId == TaskCompletedStateId;
            }
        }

        public static readonly int CompletedStateId = 0;

        [SerializeField]
        QuestType type = QuestType.Main;
        public QuestType Type
        {
            get { return type; }
        }

        [SerializeField]
        string description;

        [SerializeField]
        List<Goal> goals;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
        }

        private void Start()
        {
            
        }

        // Set this as an active quest
        public void Activate()
        {
            QuestManager.Instance.AddNewQuest(this);
        }

        public void Check()
        {
            fsm.Lookup();
        }

        
    }

}
