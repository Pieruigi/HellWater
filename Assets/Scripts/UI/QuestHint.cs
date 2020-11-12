using HW.QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW.UI
{
    public class QuestHint : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_Text questTextField;

        int messageId = 7;

        // Start is called before the first frame update
        void Start()
        {
            QuestManager.Instance.OnUpdateQuest += HandleOnUpdateQuest;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnUpdateQuest(QuestDetail questDetail)
        {
            // Tell the player that the quest has been updated.
            StartCoroutine(ShowHint(questDetail.UIDelay));
            

            

        }

        IEnumerator ShowHint(float delay)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            // Fill the text field.
            questTextField.text = UITextTranslator.GetMessage(messageId);

            // Slide the panel.
            // Get the panel.
            RectTransform panel = transform.GetChild(0) as RectTransform;
            // Get panel width.
            float width = panel.rect.width;
            // Panel is attached to the right edge of the screen with its pivot on the left,
            // so we must move it to the left.
            LeanTween.moveX((RectTransform)panel.transform, -width, 0.25f).setEaseSpring();

            // Wait some seconds
            yield return new WaitForSeconds(3f);

            // Hide the panel.
            LeanTween.moveX((RectTransform)panel.transform, 0, 0.25f);
        }

    }

}
