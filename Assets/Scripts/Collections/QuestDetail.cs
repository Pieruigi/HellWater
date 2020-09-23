using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{

    public class QuestDetail : ScriptableObject
    {
        public static readonly string ResourceFolder = "Quests";
        
        [SerializeField]
        string code;
        public string Code
        {
            get { return code; }
        }

        [SerializeField]
        bool optional = false;
        public bool Optional
        {
            get { return optional; }
        }

        [SerializeField]
        [TextAreaAttribute(1, 8)]  
        string shortDescription;
        public string ShortDescription
        {
            get { return shortDescription; }
        }

        [SerializeField]
        [TextAreaAttribute(3, 8)]
        string longDescription;
        public string LongDescription
        {
            get { return longDescription; }
        }

      

        public static QuestDetail GetQuestDetail(string code, Language language)
        {
            // Get resource
            return new List<QuestDetail>(Resources.LoadAll<QuestDetail>(System.IO.Path.Combine(ResourceFolder, language.ToString()))).Find(d => d.code == code);
        }

    }

}
