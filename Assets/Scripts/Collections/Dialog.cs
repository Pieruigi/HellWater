using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.Collections
{
    public class Dialog : ScriptableObject
    {
        public static readonly string ResourceFolder = "Dialogs";

        [System.Serializable]
        public class Speech
        {
           
            [SerializeField]
            [TextAreaAttribute(10, 10)]
            string content;
            public string Content
            {
                get { return content; }
            }

            [SerializeField]
            float length;
            public float Length
            {
                get { return length; }
            }

            [SerializeField]
            Sprite avatar;
            public Sprite Avatar
            {
                get { return avatar; }
            }

            [SerializeField]
            int index = 0;
            public int Index
            {
                get { return index; }
            }
        }

        [SerializeField]
        string code;

        [SerializeField]
        bool useSpeechIndex = false;

        [SerializeField]
        List<Speech> speeches;
        
        public int GetNumberOfSpeeches()
        {
            return speeches.Count;
        }

        public Speech GetSpeech(int id)
        {
            if (!useSpeechIndex) // Use the array id
                return speeches[id];
            else // Use the index in the speech class
                return speeches.Find(s => s.Index == id);
        }

        //void Update()
        //{
        //    if (!orderBySpeechIndex)
        //        return;
            
        //    // Order by index
        //    List<Speech> tmp = new List<Speech>();

        //    // Start from 0
        //    int currentIndex = 0;
           
        //    // Loop through all the elements
        //    while(speeches.Count > 0)
        //    {
        //        // Get all the elements having the current index
        //        List<Speech> sList = speeches.FindAll(s => s.Index == currentIndex);

        //        // Add the elements found to the tmp list
        //        tmp.AddRange(sList);

        //        // Remove elements from the original list
        //        speeches.RemoveAll(s => s.Index == currentIndex);

        //        // Increase index
        //        currentIndex++;
                
        //    } 

        //    // Add tmp to speeches
        //    speeches.AddRange(tmp);

        //}

        public static Dialog GetDialog(string code, Language language)
        {
            // Get resource
            return new List<Dialog>(Resources.LoadAll<Dialog>(System.IO.Path.Combine(ResourceFolder, language.ToString()))).Find(d=>d.code == code);

        }
    }


}
