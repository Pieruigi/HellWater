using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{

    public class Text : ScriptableObject
    {
        [SerializeField]
        [TextAreaAttribute(30, 50)]
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
