using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.UI
{
    public class SkipHint : MonoBehaviour
    {

        [SerializeField]
        Image filler;

        [SerializeField]
        Image button;

        Skipper activeSkipper = null;

        private void Awake()
        {
            filler.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get all skippers in the scene
            Skipper[] skippers = GameObject.FindObjectsOfType<Skipper>();

            // Set handles
            foreach(Skipper skipper in skippers)
            {
                skipper.OnSkipping += HandleOnSkipping;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (activeSkipper)
            {
                filler.fillAmount = activeSkipper.GetNormalizedTime();

                
            }
        }

        void HandleOnSkipping(Skipper skipper, bool skipping)
        {
            if (skipping)
            {
                activeSkipper = skipper;
                filler.fillAmount = 0;
                filler.gameObject.SetActive(true);
            }
            else
            {
                activeSkipper = null;
                filler.gameObject.SetActive(false);
            }
                
        }
    }

}
