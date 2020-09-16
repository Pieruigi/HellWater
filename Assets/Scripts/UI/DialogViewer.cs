using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HW.Collections;

namespace HW.UI
{
    public class DialogViewer : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        Image avatarImage;

        [SerializeField]
        TMPro.TMP_Text speechText;

        string currentText = "";
        float writeSpeed = 50f;
        float timer = 0;

        public static DialogViewer Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                panel.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Write text
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                   
                    // Get the first character
                    string c = currentText.Substring(0, 1);
                    
                    // Remove first character from the current text
                    currentText = currentText.Remove(0, 1);

                    // Write the caracter on screen
                    speechText.text += c;

                    // If some character is left then loop again
                    if (!"".Equals(currentText))
                        timer = 1f / writeSpeed;
                }
            }

            // Show and hide
        }

        public void ShowSpeech(string text, Sprite avatar)
        {


            // Show panel if not visible yet
            if (!IsVisible())
                SetVisible(true);

            // Set the avatar
            avatarImage.sprite = avatar;

            // Set text
            currentText = text.Trim();

            // Init speech text
            speechText.text = "";

            // Init write
            timer = 1f / writeSpeed;

        }

        public void Hide()
        {
            SetVisible(false);
        }

        void SetVisible(bool visible)
        {
            panel.SetActive(visible);
        }

        bool IsVisible()
        {
            return panel.activeSelf;
        }
    }

}
