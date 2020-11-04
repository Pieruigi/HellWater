using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MessageTrackBehaviour : PlayableBehaviour
{
    [SerializeField]
    string text;

    [SerializeField]
    bool enabled = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TMP_Text textField = playerData as TMP_Text;
        textField.transform.parent.gameObject.SetActive(enabled);
        if (enabled)
        {
            textField.text = text;
        }
        

        
        //g.transform.position += g.transform.forward * 0.03f;
        
    }
}