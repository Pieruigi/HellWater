using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using HW.Interfaces;
using UnityEngine.EventSystems;

namespace HW.UI
{
    /**
        * Simply create a message box calling GameObject.Instantiate(prefab); then call Init().
        * */
    public class MessageBox : MonoBehaviour, IActivable
    {
        public enum Type { Ok, YesNo }

        [SerializeField]
        private Button yesButton;

        [SerializeField]
        private Button okButton;

        [SerializeField]
        private Button noButton;

        [SerializeField]
        private TMP_Text msgText;

        [SerializeField]
        private Image panelImage;

        //private UnityAction _okYesAction;

        //private UnityAction _noAction;

        static MessageBox instance;
        

        private Color panelColorDefault;

        private RectTransform box;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //transform.SetAsLastSibling();
                panelColorDefault = panelImage.color;
                box = transform.GetChild(0) as RectTransform;
                gameObject.SetActive(false);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        private void OnDisable()
        {
            panelImage.sprite = null;
            panelImage.color = panelColorDefault;
            instance.box.localPosition = new Vector3(0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }



        public static void Show(Type type, string msgText = null, UnityAction okYesAction = null, UnityAction noAction = null)
        {
            switch (type)
            {
                case Type.Ok:
                    instance.okButton.gameObject.SetActive(true);
                    instance.yesButton.gameObject.SetActive(false);
                    instance.noButton.gameObject.SetActive(false);

                    if (okYesAction != null)
                    {
                        instance.okButton.onClick.AddListener(okYesAction);
                    }

                    instance.okButton.onClick.AddListener(delegate { instance.gameObject.SetActive(false); });
                    break;

                case Type.YesNo:
                    instance.okButton.gameObject.SetActive(false);
                    instance.yesButton.gameObject.SetActive(true);
                    instance.noButton.gameObject.SetActive(true);
                    if (okYesAction != null)
                    {
                        instance.yesButton.onClick.AddListener(okYesAction);
                    }
                    if (noAction != null)
                    {
                        instance.noButton.onClick.AddListener(noAction);

                    }
                    instance.yesButton.onClick.AddListener(delegate { instance.gameObject.SetActive(false); });
                    instance.noButton.onClick.AddListener(delegate { instance.gameObject.SetActive(false); });
                    break;
            }

            if (msgText != null)
                instance.msgText.text = msgText;

            instance.gameObject.SetActive(true);

            SetDefaultSelection(type);
        }

        public static void SetPanel(Sprite sprite)
        {
            instance.panelImage.sprite = sprite;
            instance.panelImage.color = Color.white;
        }

        public static void SetBottomPosition()
        {
            instance.box.localPosition = new Vector3(0, -380, 0);
        }

        static void SetDefaultSelection(Type type)
        {
            if (type == Type.YesNo)
            {
                instance.noButton.Select();
                //EventSystem.current.SetSelectedGameObject(instance.noButton.gameObject);
            }

            else
            {
                instance.okButton.Select();
                //EventSystem.current.SetSelectedGameObject(instance.noButton.gameObject);
            }
                
        }

        public void Activate(bool value)
        {
            //throw new System.NotImplementedException();
            Debug.LogWarning("Use the MessageBox.Show() static method.");
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }


}
