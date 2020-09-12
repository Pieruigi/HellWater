using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        GameObject buttonContinue;

        // Start is called before the first frame update
        void Start()
        {
            if (!GameManager.Instance.IsSaveGameAvailable())
                buttonContinue.GetComponent<Button>().interactable = false;
        }

        // Update is called once per frame
        void Update()
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
    }

}
