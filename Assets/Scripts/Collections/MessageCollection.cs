using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{
    public class MessageCollection : ScriptableObject
    {
        [SerializeField]
        List<string> messages;

        public string GetMessage(int id)
        {
            if (id < 0 || id >= messages.Count)
                return "";

            return messages[id];
        }
    }

}
