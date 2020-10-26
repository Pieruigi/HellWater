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

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            buttons = new List<Button>(GetComponentsInChildren<Button>());

            if (!GameManager.Instance.IsSaveGameAvailable()) // No game saved
            {
                // Disable continue button
                buttonContinue.GetComponent<Button>().interactable = false;

                Debug.Log("Selecting new game button");

                // Select the new game button
                newGameButton.GetComponent<Selectable>().Select();
            }
            else // You can continue from the last save game
            {
                buttonContinue.GetComponent<Selectable>().Select();
            }

            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnEnable()
        {
           
        }

        public void StartNewGame()
        {
            GameManager.Instance.StartNewGame();
        }

        public void ContinueGame()
        {
            GameManager.Instance.ContinueGame();
        }

        public void ExitGame()
        {
            inputDisabled = true;
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
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
