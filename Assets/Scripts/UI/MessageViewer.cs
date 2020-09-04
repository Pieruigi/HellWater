using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HW.UI
{
    public class MessageViewer : MonoBehaviour
    {
        [SerializeField]
        Image labelImage;

        [SerializeField]
        TMP_Text textField;

        public static MessageViewer Instance { get; private set; }

        float timer = 3;
        float lastTime;


        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                Show(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(lastTime > 0)
            {
                lastTime -= Time.deltaTime;
                if (lastTime < 0)
                    Show(false);
            }
        }

        public void ShowMessage(string message)
        {
            Show(true);

            textField.text = message;
            lastTime = timer;
        }

        void Show(bool value)
        {
            labelImage.gameObject.SetActive(value);
            //textField.enabled = value;
        }

    }

}
