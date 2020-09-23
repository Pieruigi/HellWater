using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{

    public class Text : ScriptableObject
    {
        [SerializeField]
        string code;
        public string Code
        {
            get { return code; }
        }

        [SerializeField]
        [TextAreaAttribute(15, 25)]
        string content;
        public string Content
        {
            get { return content; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
    }

}
