using HW.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HW.UI
{
    public class InventoryHint : MonoBehaviour
    {
        [SerializeField]
        TMP_Text textField;
        
        // We set this flag to false in order to avoid warnings on start, when the inventory is
        // filled by the cache.
        bool active = false;
        bool visible = false;

        float transitionTime = 0.25f;
        
        int messageId = 9;



        // Start is called before the first frame update
        void Start()
        {
            Inventory.Instance.OnItemAdded += HandleOnItemAdded;
            Inventory.Instance.OnItemRemoved += HandleOnItemRemoved;

            active = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnItemAdded(Item item)
        {
            if (!active || visible)
                return;

            // Tell the player that the quest has been updated.
            StartCoroutine(ShowHint());
        }

        void HandleOnItemRemoved(Item item)
        {
            HandleOnItemAdded(item);
        }

        IEnumerator ShowHint()
        {
            // Set visible.
            visible = true;

            // Just wait a little bit.
            yield return new WaitForSeconds(1f);

            // Fill the text field.
            textField.text = UITextTranslator.GetMessage(messageId);

            // Slide the panel.
            // Get the panel.
            RectTransform panel = transform.GetChild(0) as RectTransform;
            // Get panel width.
            float width = panel.rect.width;
            // Panel is attached to the left edge of the screen with its pivot on the right,
            // so we must move it to the right.
            LeanTween.moveX((RectTransform)panel.transform, width, transitionTime).setEaseSpring();

            // Wait some seconds
            yield return new WaitForSeconds(3f);

            // Hide the panel.
            LeanTween.moveX((RectTransform)panel.transform, 0, transitionTime);

            yield return new WaitForSeconds(transitionTime);

            // Reset visible.
            visible = false;
        }
    }

}
