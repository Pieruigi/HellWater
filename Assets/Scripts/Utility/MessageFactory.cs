using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    public class MessageFactory
    {
        MessageCollection messageCollection;

        static MessageFactory instance;
        public static MessageFactory Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new MessageFactory();
                }

                return instance;
            }
        }

        private MessageFactory()
        {
            // Load resources depending on the language
            string path = System.IO.Path.Combine(Constants.ResourceFolderMessageCollection, GameManager.Instance.Language.ToString());
            Debug.Log("Message collection path:" + path);
            messageCollection = Resources.LoadAll<MessageCollection>(path)[0];
        }

        // Returns the message text or an empty string
        public string GetMessage(int id)
        {
            return messageCollection.GetMessage(id);
        }
    }

}
