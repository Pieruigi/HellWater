using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.Interfaces;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HW.UI
{
    public class GameMenu : MonoBehaviour, IActivable
    {
        [SerializeField]
        Button buttonContinue;

        int leaveMsgId = 3;

        bool inputDisable = false;

        GameObject lastSelectedObject;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            buttonContinue.GetComponent<Selectable>().Select();
            EventSystem.current.SetSelectedGameObject(buttonContinue.gameObject);
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
        }

        void Update()
        {
            
        }

        private void OnEnable()
        {
            buttonContinue.GetComponent<Selectable>().Select();
            EventSystem.current.SetSelectedGameObject(buttonContinue.gameObject);
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            
            Debug.Log("Continue button selected");
        }

        public void Continue()
        {
            GameManager.Instance.CloseMenu();
        }

        public void Leave()
        {
            inputDisable = true;
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;

            //gameObject.SetActive(false);
            string msg = UITextTranslator.GetMessage(leaveMsgId);
            MessageBox.Show(MessageBox.Type.YesNo, msg, HandleOnLeaveYes, HandleOnLeaveNo);
            
        }

        void HandleOnLeaveYes()
        {
            GameManager.Instance.LoadMainMenu();
        }

        void HandleOnLeaveNo()
        {
            inputDisable = false;
            gameObject.SetActive(true);
            lastSelectedObject.GetComponent<Selectable>().Select();
            
        }

        public void Activate(bool value)
        {
            
        }

        public bool IsActive()
        {
            return gameObject.activeSelf && !inputDisable;
        }
    }

}
