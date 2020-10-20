using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.UI
{
    public class InGameTutorialViewer : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        public static InGameTutorialViewer Instance { get; private set; }

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

        }


        void HandleOnShow(int tutorialId)
        {
            panel.SetActive(true);
        }

        public void Hide()
        {
            panel.SetActive(false);
        }
    }

}
