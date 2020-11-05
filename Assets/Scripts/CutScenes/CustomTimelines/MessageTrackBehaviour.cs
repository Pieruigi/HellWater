using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using HW;

namespace HW.CutScene
{
    [System.Serializable]
    public class MessageTrackBehaviour : PlayableBehaviour
    {
        [SerializeField]
        int messageId;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            // Get the object attached to the track
            TMP_Text textField = playerData as TMP_Text;

            // Get the message from the id

            string message = MessageFactory.Instance.GetMessage(messageId);
            textField.text = message;

            //g.transform.position += g.transform.forward * 0.03f;

        }
    }
}
