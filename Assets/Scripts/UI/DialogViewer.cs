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
        float writeSpeed = 80f;
        float timer = 0;

        float fadeSpeed = 1f;
        int fadeDir = 0;

        Image bgImage;

        public static DialogViewer Instance { get; private set; }



        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                panel.SetActive(false);
                bgImage = panel.GetComponent<Image>();
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
            if(fadeDir != 0)
            {
                Color targetColor = Color.white;

                if (fadeDir > 0)
                    targetColor.a = 1;
                else
                    targetColor.a = 0;

                bgImage.color = Vector4.MoveTowards(bgImage.color, targetColor, fadeSpeed * Time.deltaTime);
                avatarImage.color = bgImage.color;//Vector4.MoveTowards(avatarImage.color, targetColor, fadeSpeed * Time.deltaTime);

                if (bgImage.color == targetColor)// && avatarImage.color == targetColor)
                {
                    if(fadeDir < 0)
                        panel.SetActive(false);

                    fadeDir = 0;
                }
                    
            }
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
            Color c = Color.white;
           

            if (visible) 
            {
                // Set transparent
                c.a = 0;
                fadeDir = 1;
                panel.SetActive(visible);
            }
            else
            {
                fadeDir = -1;
            }

            bgImage.color = c;
            avatarImage.color = c;
        }

        bool IsVisible()
        {
            return panel.activeSelf;
        }

 
    }

}
