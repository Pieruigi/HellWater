using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW.QuestSystem
{
    
    public class QuestManager
    {
        public UnityAction<Quest> OnQuestActivate;

        public UnityAction<Quest> OnQuestUpdate;

        List<Quest> activeQuests;
        
        static QuestManager instance;
        public static QuestManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new QuestManager();

                return instance;
            }
        }

        private QuestManager()
        {
            activeQuests = new List<Quest>();
        }

        public void AddNewQuest(Quest quest)
        {
            activeQuests.Add(quest);
        }
    }

}
