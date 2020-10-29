using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HW.QuestSystem;

namespace HW.CachingSystem
{
    public class QuestCacher : Cacher
    {
        protected override string GetCacheValue()
        {
            if (QuestManager.Instance.CurrentQuest == null)
                return "";

            return QuestManager.Instance.CurrentQuest.Code;
        }

        /// <summary>
        /// Init the quest manager.
        /// </summary>
        /// <param name="cacheValue"></param>
        protected override void Init(string cacheValue)
        {
            // Set the quest by code.
            // Since the Init() is called by the parent class in the Awake() it may be possibile for the QuestManager 
            // to be not yet been instantiated as singletone, but for sure it exists as component because belongs
            // to the same game object this script belogns to.
            GetComponent<QuestManager>().SetCurrentQuest(cacheValue);

        }

      
    }

}
