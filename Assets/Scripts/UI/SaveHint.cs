using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;
using UnityEngine.UI;

namespace HW.UI
{
    public class SaveHint : MonoBehaviour
    {
        [SerializeField]
        Image image;

        float speed = 360f;

        private void Awake()
        {
            image.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            CacheManager.Instance.OnSave += HandleOnSave;
        }

        // Update is called once per frame
        void Update()
        {
            if (image.enabled)
            {
                // Rotate the ico.
                Vector3 euler = image.transform.eulerAngles;
                euler.z += Time.deltaTime * speed;
                image.transform.eulerAngles = euler;
            }
        }

        void HandleOnSave()
        {
            StartCoroutine(ShowIco());
        }

        IEnumerator ShowIco()
        {
            // Enable the ico.
            image.enabled = true;

            // Wait a bit.
            yield return new WaitForSeconds(3f);

            // Disable the ico.
            image.enabled = false;
        }
    }

}
