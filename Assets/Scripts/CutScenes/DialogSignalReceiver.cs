using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.UI;
using HW.Interfaces;

namespace HW.CutScene
{
    public class DialogSignalReceiver : MonoBehaviour
    {
        [SerializeField]
        string dialogCode;

        Dialog dialog;

        int currentId = 0;
        
        
        void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            dialog = Dialog.GetDialog(dialogCode, GameManager.Instance.Language);
        }

        // Update is called once per frame
        void Update()
        {
        }

      

        public void NextDialog()
        {
            
            Dialog.Speech speech = dialog.GetSpeech(currentId);

            DialogViewer.Instance.ShowSpeech(speech.Content, speech.Avatar);

            currentId++;
        }


        public virtual void StopDialog()
        {
            
            StopAllCoroutines();

            // Hide dialog
            DialogViewer.Instance.Hide();
        }

    }

}
