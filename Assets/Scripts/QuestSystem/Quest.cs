using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW.QuestSystem
{
    [System.Serializable]
    public class Quest
    {
        [SerializeField]
        QuestDetail detail;

        [SerializeField]
        int state;
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        public Quest(QuestDetail detail, int state)
        {
            this.detail = detail;
            this.state = state;
        }

        public string GetShortDescription()
        {
            return detail.ShortDescription;
        }

        public string GetLongDescription()
        {
            return detail.LongDescription;
        }

        public string GetCode()
        {
            return detail.Code;
        }

        public bool IsOptional()
        {
            return detail.Optional;
        }
    }

}
