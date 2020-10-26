using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using TMPro;

namespace HW.UI
{
    public class UITextTranslator : MonoBehaviour
    {
        [SerializeField]
        int textIndex;

        TMP_Text textField;

        private void Awake()
        {
            textField = GetComponent<TMP_Text>();
        }

        // Start is called before the first frame update
        void Start()
        {
            textField.text = GetMessage(textIndex);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static string GetMessage(int index)
        {
            // Load resources depending on the language
            string path = System.IO.Path.Combine(Constants.ResourceFolderMessageCollectionUI, GameManager.Instance.Language.ToString());
            Debug.Log("Message collection path:" + path);
            return Resources.LoadAll<MessageCollection>(path)[0].GetMessage(index);
        }
    }

}
