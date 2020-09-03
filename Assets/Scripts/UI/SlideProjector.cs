using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.UI
{
    public class SlideProjector : MonoBehaviour
    {
        [SerializeField]
        Image image;

        public static SlideProjector Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                image.enabled = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowSlide(Sprite sprite)
        {
            if (!image.enabled)
                image.enabled = true;

            image.sprite = sprite;
        }

        public void Hide()
        {
            image.sprite = null;
            image.enabled = false;
        }
    }

}
