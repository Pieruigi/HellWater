using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HW.Collections;
using HW.CachingSystem;

namespace HW.QuestSystem
{
    
    public enum QuestState { None, Current, Completed }

    

    public class QuestManager: MonoBehaviour
    {
        public UnityAction<Quest> OnQuestActivate;

        public UnityAction<Quest> OnQuestUpdate;

        List<Quest> quests = new List<Quest>();

        Quest currentMain;
        Quest currentOptional;
        
        public static QuestManager Instance { get; private set; }
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {
            Init();
        }


        public void SetCurrentQuest(string code)
        {
            Quest quest = quests.Find(q => code.Trim().ToLower().Equals(q.GetCode().Trim().ToLower()));
            quest.State = (int)QuestState.Current;

            if (quest.IsOptional())
                currentOptional = quest;
            else
                currentMain = quest;
        }

        public void SetQuestAsCompleted(string code)
        {
            Quest quest = quests.Find(q => code.Trim().ToLower().Equals(q.GetCode().Trim().ToLower()));
            quest.State = (int)QuestState.Completed;

            if (currentOptional == quest)
                currentOptional = null;

            if (currentMain == quest)
                currentMain = null;
        }

        private void Init()
        {
            // Load all the quests
            QuestDetail[] details = Resources.LoadAll<QuestDetail>(System.IO.Path.Combine(QuestDetail.ResourceFolder, GameManager.Instance.Language.ToString()));

            // Create quests
            foreach(QuestDetail detail in details)
            {
                // Search in cache for the state id
                int state = (int)QuestState.None;
                string v;
                if (CacheManager.Instance.TryGetCacheValue(detail.Code, out v))
                {
                    state = int.Parse(v);
                }

                // Create quest
                Quest quest = new Quest(detail, state);

                // Add the quest to the list
                quests.Add(quest);

                // Set as current if needed
                if(state == (int)QuestState.Current)
                {
                    if (detail.Optional)
                        currentOptional = quest;
                    else
                        currentMain = quest;
                }

            }

       
        }
    }

}
