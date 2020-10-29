using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HW.Collections;
using HW.CachingSystem;
using UnityEngine.Events;

namespace HW.QuestSystem
{

    /// <summary>
    /// This class is used to activate quests or set them as completed.
    /// You can only manage one primary quest.
    /// </summary>
    public class QuestManager: MonoBehaviour
    {
        // Called when the quest manager is updated
        public UnityAction<QuestDetail> OnUpdateQuest;

        public static readonly string QuestCacheCode = "qst";

        QuestDetail currentQuest;
        public QuestDetail CurrentQuest
        {
            get { return currentQuest; }
        }
        
        public static QuestManager Instance { get; private set; }

        List<QuestDetail> quests; // List to hold all the quests
        
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

        /// <summary>
        /// Load the quest from the quest list by code and set it as the active quest.
        /// </summary>
        /// <param name="code">The code of the quest.</param>
        public void SetCurrentQuest(string code)
        {
            // Load all the quests from resources.
            if (quests == null)
                quests = new List<QuestDetail>(Resources.LoadAll<QuestDetail>(System.IO.Path.Combine(QuestDetail.ResourceFolder, GameManager.Instance.Language.ToString())));

            if(quests.Count == 0)
            {
                Debug.LogError("QuestManager.SetCurrentQuest(" + code + "): quests list is empty.");
                return;
            }

            // Check param
            if (string.IsNullOrEmpty(code))
            {
                Debug.LogError("QuestManager.SetCurrentQuest("+code+"): failed because code is empty.");
                return;
            }

            // Try to set the quest
            currentQuest = quests.Find(q => q.Code == code);

            // If no quest throw an error
            if (!currentQuest)
            {
                Debug.LogError("QuestManager.SetCurrentQuest(" + code + "): no quest found.");
                return;
            }

            OnUpdateQuest?.Invoke(currentQuest);
        }

        /// <summary>
        /// Reset the current quest ( that means you have no quest at all ).
        /// </summary>
        public void ResetCurrentQuest()
        {
            currentQuest = null;

            OnUpdateQuest?.Invoke(null);
        }


    }

}
