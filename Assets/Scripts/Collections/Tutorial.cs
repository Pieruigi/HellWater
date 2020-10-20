using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.Collections
{
    
    public class Tutorial : ScriptableObject
    {
        public static readonly string ResourceFolder = "Tutorials";

        [SerializeField]
        string code; // The tutorial id

        [SerializeField]
        GameObject prefab; // The tutorial itself
        public GameObject Prefab
        {
            get { return prefab; }
        }

        public static Tutorial GetTutorial(string code)
        {
            // Get resource
            return new List<Tutorial>(Resources.LoadAll<Tutorial>(ResourceFolder)).Find(d => d.code == code);

        }
    }

}
