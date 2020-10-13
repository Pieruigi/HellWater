using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    public class PlayerBlockController : MonoBehaviour
    {
        public UnityAction<PlayerBlockController> OnBlockOpen;

        [SerializeField]
        Transform target;

        FiniteStateMachine fsm;

        float fadeSpeed = 0.5f;
        float blackScreenTime = 1f;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnFail += HandleOnFail;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            // Can't walk through
            StartCoroutine(CoroutineBlock());
            
        }

        IEnumerator CoroutineBlock()
        {
            // Disable player controller
            PlayerController.Instance.SetDisabled(true);

            // Fade out
            CameraFader.Instance.TryDisableAnimator();
            yield return CameraFader.Instance.FadeOutCoroutine(fadeSpeed);

            // Wait a little bit
            yield return new WaitForSeconds(blackScreenTime);

            // Move player to a safe position
            Rigidbody rb = PlayerController.Instance.GetComponent<Rigidbody>();
            rb.position = target.position;
            PlayerController.Instance.transform.rotation = target.rotation;

            // Fade in
            yield return CameraFader.Instance.FadeInCoroutine(fadeSpeed*2f);
            CameraFader.Instance.TryEnableAnimator();

            // Enable player controller
            PlayerController.Instance.SetDisabled(false);

        }
    }

}
