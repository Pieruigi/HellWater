using HW;
using HW.Collections;
using HW.CutScene;
using HW.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HW.Collections.Dialog;

public class _TestDialog : MonoBehaviour
{
    [SerializeField]
    string dialogCode;

    int startIndex = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        //HW.UI.DialogViewer.Instance.ShowSpeech("Aaa fs df s fsdfs fsdfsd", null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Dialog d = Dialog.GetDialog(dialogCode, GameManager.Instance.Language);

            Speech speech = d.GetSpeech(startIndex);

            if (speech == null)
                return;

            DialogViewer.Instance.ShowSpeech(speech.Content, speech.Avatar);

            startIndex++;
        }
            
    }


}
