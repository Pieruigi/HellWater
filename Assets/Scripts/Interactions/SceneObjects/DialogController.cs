using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.UI;
using HW.Interfaces;

namespace HW
{
    public class DialogController : MonoBehaviour, ISkippable
    {
        [SerializeField]
        string dialogCode;

        Dialog dialog;

        int currentId = 0;
        int numberOfSpeeches;

        bool running = false;

        // Timer is dinamically set depending of the length of the current speech
        float timer = 0;

        float readSpeed = 2f; // How many words you can read per second 

        Coroutine loopDialogCoroutine;

        bool skipEnabled = false;

        protected virtual void Awake()
        {
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            SetDialogCode(dialogCode);
        }

        // Update is called once per frame
        void Update()
        {
        }

       

        public void Skip()
        {
            StopDialog();
        }

        public bool CanBeSkipped()
        {
            return skipEnabled;
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

        protected void LoopDialog()
        {
            loopDialogCoroutine = StartCoroutine(CoroutineStartDialog());
        }

        protected void SetDialogCode(string code)
        {
            dialogCode = code;
            dialog = Dialog.GetDialog(dialogCode, GameManager.Instance.Language);
            numberOfSpeeches = dialog.GetNumberOfSpeeches();

        }

        IEnumerator CoroutineStartDialog()
        {
            // Loop through dialogues
            while (currentId < dialog.GetNumberOfSpeeches())
            {
                float length = dialog.GetSpeech(currentId).Length;
                NextDialog();

                if(currentId < dialog.GetNumberOfSpeeches() - 1)
                {
                    if (!skipEnabled)
                        skipEnabled = true;
                }
                else
                {
                    if(skipEnabled)
                        skipEnabled = false;
                }
                //Dialog.Speech speech = dialog.GetSpeech(currentId);
                //DialogViewer.Instance.ShowSpeech(speech.Content, speech.Avatar);
                yield return new WaitForSeconds(length);
                
            }

            StopDialog();
        }
    }

}
