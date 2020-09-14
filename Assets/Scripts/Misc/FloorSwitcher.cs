using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FloorSwitcher : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != PlayerController.Instance.gameObject)
                return;

            StartCoroutine(ChangeFloor());

        }

        IEnumerator ChangeFloor()
        {
            float speed = 2.5f;
            // Fade out
            yield return CameraFader.Instance.FadeOutCoroutine(speed);

            // Disable player
            PlayerController.Instance.SetDisabled(true);

            // Move player to the other floor
            PlayerController.Instance.transform.position = target.position;
            PlayerController.Instance.transform.rotation = target.rotation;

            yield return new WaitForSeconds(1f);


            // Fade in
            yield return CameraFader.Instance.FadeInCoroutine(speed);

            // Enable player
            PlayerController.Instance.SetDisabled(false);

        }
    }

}
