using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HW.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        [SerializeField]
        TMP_Text textGameOver;

        [SerializeField]
        Button buttonContinue;

        [SerializeField]
        Button buttonBack;

        // The following texts are from the message collection
        int deathTextId = 28;
        int catchTextId = 29;
        int arrestTextId = 30;

        private void Awake()
        {
            buttonContinue.gameObject.SetActive(false);
            buttonBack.gameObject.SetActive(false);
            panel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController.Instance.OnDead += HandleOnDead;

            buttonContinue.onClick.AddListener(HandleOnContinue);
            buttonBack.onClick.AddListener(HandleOnBack);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnDead()
        {
            StartCoroutine(CoroutineGameOver((int)GameOverType.Death));
        }

        void HandleOnContinue()
        {
            GameManager.Instance.ContinueGame();
        }

        void HandleOnBack()
        {
            GameManager.Instance.LoadMainMenu();
        }

        int GetGameOverTextId(GameOverType gameOverType)
        {
            int ret = -1;
            switch (gameOverType)
            {
                case GameOverType.Death:
                    ret = deathTextId;
                    break;
                case GameOverType.Capture:
                    ret = catchTextId;
                    break;
                case GameOverType.Arrest:
                    ret = arrestTextId;
                    break;
            }

            return ret;
        }

        IEnumerator CoroutineGameOver(int gameOverType)
        {
            

            // Wait for a while
            yield return new WaitForSeconds(3f);

            // Fade out
            CameraFader.Instance.TryDisableAnimator();
            yield return CameraFader.Instance.FadeOutCoroutine(0.2f);

            // Show message
            panel.SetActive(true);
            string message = MessageFactory.Instance.GetMessage(GetGameOverTextId((GameOverType)gameOverType));
            textGameOver.text = message;

            yield return new WaitForSeconds(3f);

            // Activate buttons
            buttonContinue.gameObject.SetActive(true);
            buttonBack.gameObject.SetActive(true);
        }
    }

}
