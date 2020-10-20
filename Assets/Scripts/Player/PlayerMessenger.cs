using HW.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerMessenger : MonoBehaviour
    {
        [SerializeField]
        int noWeaponAllowedMessageId;
        int noWeaponAllowedMessageIdDefault;

        float delay = 0.5f;

        private void Awake()
        {
            PlayerController player = GetComponent<PlayerController>();
            player.OnHolsterForced += HandleOnHolsterForced;

            noWeaponAllowedMessageIdDefault = noWeaponAllowedMessageId;              
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetNoWeaponAllowedMessageId(int messageId)
        {
            noWeaponAllowedMessageId = messageId;
        }

        public void ResetNoWeaponAllowedMessageId(int messageId)
        {
            noWeaponAllowedMessageId = noWeaponAllowedMessageIdDefault;
        }

        void HandleOnHolsterForced()
        {
            // Get the right message depending on the id and language
            string message = MessageFactory.Instance.GetMessage(noWeaponAllowedMessageId);

            StartCoroutine(ShowMessage(message, delay));
        }

        IEnumerator ShowMessage(string message, float delay)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            MessageViewer.Instance.ShowMessage(message);
        }
    }

}
