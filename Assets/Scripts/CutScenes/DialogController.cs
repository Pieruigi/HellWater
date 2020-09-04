using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.UI;

namespace HW.Cinema
{
    public class DialogController : MonoBehaviour
    {
        [SerializeField]
        Dialog dialog;

        int currentId = 0;
        int numberOfSpeeches;

        bool running = false;

        // Timer is dinamically set depending of the length of the current speech
        float timer = 0;

        float readSpeed = 2f; // How many words you can read per second 

        Coroutine writeDialogCoroutine;

        private void Awake()
        {
            numberOfSpeeches = dialog.GetNumberOfSpeeches();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void StartDialog()
        {
            writeDialogCoroutine = StartCoroutine(WriteDialog());
        }

        public void StopDialog()
        {
            // Stop coroutine
            StopCoroutine(writeDialogCoroutine);

            // Hide dialog
            DialogViewer.Instance.Hide();
        }

        IEnumerator WriteDialog()
        {
            Debug.Log("Writing dialog...");

            // Loop through all the dialog elements
            for(int i=0; i<numberOfSpeeches; i++)
            {
                
                // Get next speech
                Dialog.Speech speech = dialog.GetSpeech(i);

                Debug.Log("Next speech:" +speech.Content);

                // Compute the length in seconds ( it depends on the number of words )
                timer = speech.Content.Split(' ').Length / readSpeed;

                // Show speech
                DialogViewer.Instance.ShowSpeech(speech.Content, speech.Avatar);

                // Wait
                yield return new WaitForSeconds(timer);
            }

            StopDialog();
        }
    }

}
