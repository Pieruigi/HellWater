using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;
using UnityEngine.Events;

namespace HW
{
    public class BreakController : MonoBehaviour
    {
        //public UnityAction<BreakController> OnFadeOutComplete;
        //public UnityAction<BreakController> OnFadeInComplete;

        [SerializeField]
        int desiredStateId = -1; // The state you want to be processed

        [Header("Fade Section")]
        [SerializeField]
        float fadeSpeed = 1f;

        [SerializeField]
        float fadeLength = 0;

        [Header("Message Section")]
        [SerializeField]
        int messageId = -1; // Leave negative if you don't want to show a message

        [SerializeField]
        float messageDelay = 0; // Conunt starts after the fade out on enter

        [SerializeField]
        float messageTime = 0f; // How much time the message will be shown on screen

        [Header("Objects Section")]
        [SerializeField]
        List<GameObject> objectsToShow;

        [SerializeField]
        List<GameObject> objectsToHide;

        [Header("Particles Section")]
        [SerializeField]
        List<ParticleSystem> particlesToPlay;

        [SerializeField]
        List<ParticleSystem> particlesToStop;

        FiniteStateMachine fsm;

        Animator cameraFaderAnimator;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            


        }

        // Start is called before the first frame update
        void Start()
        {
            // Get the camera fader animator
            cameraFaderAnimator = CameraFader.Instance.GetComponent<Animator>();

            // Init
            CheckObjects();
            CheckParticles();
        }

        // Update is called once per frame
        void Update()
        {

        }


        IEnumerator CoroutineBreak()
        {
            // Stop player
            PlayerController.Instance.SetDisabled(true);

            // Since the fader is also used in timelines it has been provided with an animator that must be disabled
            // in order to make manual fade work
            if (cameraFaderAnimator)
                cameraFaderAnimator.enabled = false;

            // Fade out
            yield return CameraFader.Instance.FadeOutCoroutine(fadeSpeed);

            //OnFadeOutComplete?.Invoke(this);

            // Check objects to de/activate
            CheckObjects();
            CheckParticles();

            // Show message if needed
            if (messageId >= 0)
                StartCoroutine(CoroutineShowMessage());

            // Wait 
            yield return new WaitForSeconds(fadeLength);

            // Fade in
            yield return CameraFader.Instance.FadeInCoroutine(fadeSpeed);

            // Enable the animator if needed
            if (cameraFaderAnimator)
                cameraFaderAnimator.enabled = true;

            // Enable player again
            PlayerController.Instance.SetDisabled(false);
        }

        // Handles message to be shown
        IEnumerator CoroutineShowMessage()
        {
            // Is there some delay to wait for?
            if (messageDelay > 0)
                yield return new WaitForSeconds(messageDelay);

            // Get the message text
            string message = MessageFactory.Instance.GetMessage(messageId);

            // Show the message for a given time
            MessageViewer.Instance.ShowMessage(message, messageTime);

        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if(fsm.CurrentStateId == desiredStateId)
                StartCoroutine(CoroutineBreak());
            else
                if(oldState == desiredStateId)
            {
                // Check objects to de/activate
                CheckObjects();
                CheckParticles();
            }
            
                
        }


        void CheckObjects()
        {
            if (fsm.CurrentStateId == desiredStateId)
            {
                // Show all the objects that must be shown after complete
                foreach (GameObject g in objectsToShow)
                    g.SetActive(true);

                // Hide the others
                foreach (GameObject g in objectsToHide)
                    g.SetActive(false);
            }
            else
            {
                // Hide all the objects that must be shown after complete
                foreach (GameObject g in objectsToShow)
                    g.SetActive(false);

                // Show the others
                foreach (GameObject g in objectsToHide)
                    g.SetActive(true);
            }
        }

        void CheckParticles()
        {
            if (fsm.CurrentStateId == desiredStateId)
            {
                // Show all the objects that must be shown after complete
                foreach (ParticleSystem g in particlesToPlay)
                    g.Play();

                // Hide the others
                foreach (ParticleSystem g in particlesToStop)
                    g.Stop();
            }
            else
            {
                // Hide all the objects that must be shown after complete
                foreach (ParticleSystem g in particlesToPlay)
                    g.Stop();

                // Show the others
                foreach (ParticleSystem g in particlesToStop)
                    g.Play();
            }
        }
    }

}
