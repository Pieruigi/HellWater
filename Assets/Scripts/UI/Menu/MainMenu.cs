using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using HW.Interfaces;

namespace HW.UI
{
    public class MainMenu : MonoBehaviour, IActivable
    {
        [SerializeField]
        GameObject buttonContinue;

        [SerializeField]
        GameObject newGameButton;

        List<Button> buttons;

        bool inputDisabled = false;

        GameObject lastSelectedObject;

        int exitGameTextId = 4;

        int newGameTextId = 8;

        
        // Start is called before the first frame update
        void Start()
        {
            buttons = new List<Button>(GetComponentsInChildren<Button>());

            if (!GameManager.Instance.IsSaveGameAvailable()) // No game saved
            {
                // Disable continue button
                buttonContinue.GetComponent<Button>().interactable = false;

                // Select the new game button
                newGameButton.GetComponent<Selectable>().Select();
            }
            else // You can continue from the last save game
            {
                buttonContinue.GetComponent<Selectable>().Select();
            }

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("IsFocused:"+EventSystem.current.currentSelectedGameObject);
  
        }


        public void StartNewGame()
        {
            
            if (GameManager.Instance.IsSaveGameAvailable())
            {
                inputDisabled = true;

                // We need to store the last selected button ( the exit button we assume ) in order
                // the be able to select it again if we abort exit.
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;

                // If a save game is available we must warn the player that starting a new game
                // will destroy the old saves.
                string message = UITextTranslator.GetMessage(newGameTextId);
                MessageBox.Show(MessageBox.Type.YesNo, message, HandleOnNewGameYes, HandleOnNewGameNo);
                
            }
            else
            {
                GameManager.Instance.StartNewGame();
            }
            
        }

        public void ContinueGame()
        {
            GameManager.Instance.ContinueGame();
        }

        public void ExitGame()
        {
            inputDisabled = true;

            // We need to store the last selected button ( the exit button we assume ) in order
            // the be able to select it again if we abort exit.
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;

            // Show the message box.
            string msg = UITextTranslator.GetMessage(exitGameTextId);
            MessageBox.Show(MessageBox.Type.YesNo, msg, HandleOnExitYes, HandleOnExitNo);
        }



        void HandleOnExitYes()
        {
            GameManager.Instance.ExitGame();
        }

        void HandleOnExitNo()
        {
            inputDisabled = false;

            // Reset focus on the last button.
            lastSelectedObject.GetComponent<Selectable>().Select();
        }

        void HandleOnNewGameYes()
        {
            GameManager.Instance.StartNewGame();
        }

        void HandleOnNewGameNo()
        {
            inputDisabled = false;

            // Reset focus on the last button.
            lastSelectedObject.GetComponent<Selectable>().Select();
        }

        // We don't need the activate function to be implemented
        public void Activate(bool value)
        {
            //throw new NotImplementedException();
        }

        // This is called by the navigator
        public bool IsActive()
        {
            return gameObject.activeSelf && !inputDisabled;
        }
    }

}
