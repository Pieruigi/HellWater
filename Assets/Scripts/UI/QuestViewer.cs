using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HW.QuestSystem;
using HW.Collections;

namespace HW.UI
{
    public class QuestViewer : MonoBehaviour
    {
        [SerializeField]
        TMP_Text shortDescription;

        [SerializeField]
        TMP_Text longDescription;

        [SerializeField]
        GameObject rootPanel;

        public static QuestViewer Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                // Simply clear fields
                shortDescription.text = "";
                longDescription.text = "";

                // Hide
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

        }

        public bool IsOpen()
        {
            return rootPanel.activeSelf;
        }

        public void Show(bool value)
        {
            if (value)
            {
                // Fill data
                FillData();
            }
            

            // Show panel
            rootPanel.SetActive(value);
        }

     

        void FillData()
        {
            QuestDetail currentQuest = QuestManager.Instance.CurrentQuest;

            // Quest can be null, that means you were given no quest at moment ( you will be given a new one in a while
            // hopefully ).
            if(currentQuest == null) // No quest
            {
                shortDescription.text = "";
                longDescription.text = "";
            }
            else // New quest added
            {
                shortDescription.text = currentQuest.ShortDescription;
                longDescription.text = currentQuest.LongDescription;

            }

            
        }
    }

}
