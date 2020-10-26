using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.UI;

namespace HW.UI
{
    public class InGameTutorialViewer : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        Transform tutorialRoot;

        public static InGameTutorialViewer Instance { get; private set; }

        bool open = false;

        GameObject currentTutorial;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                Hide();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            InGameTutorialController[] list = FindObjectsOfType<InGameTutorialController>();
            foreach (InGameTutorialController l in list)
                l.OnShow += HandleOnShow;
        }

        // Update is called once per frame
        void Update()
        {
            if (open)
            {
                if (PlayerInput.GetActionButtonDown())
                    Hide();
            }
        }


        void HandleOnShow(string tutorialCode)
        {
            // Get the tutorial asset
            Tutorial tutorial = Tutorial.GetTutorial(tutorialCode);
            
            // Freeze game
            Time.timeScale = 0;
            open = true;

            // Create object
            currentTutorial = Instantiate(tutorial.Prefab, tutorialRoot);

            // Set panel active
            panel.SetActive(true);


        }

        public void Hide()
        {
            // Deactivate panel
            panel.SetActive(false);

            // Remove current tutorial
            if(currentTutorial)
                Destroy(currentTutorial.gameObject);

            open = false;
            Time.timeScale = 1;
        }
    }

}
