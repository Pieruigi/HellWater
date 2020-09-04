using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public class Skipper : MonoBehaviour
    {
        public UnityAction<Skipper, bool> OnSkipping;

        ISkippable skippable;

        float timer = 0;
        float skipInSecs = 2;

        bool buttonIsDown = false;

        private void Awake()
        {
            skippable = GetComponent<ISkippable>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!skippable.CanBeSkipped())
            {
                if (buttonIsDown)
                {
                    buttonIsDown = false;
                    OnSkipping?.Invoke(this, false);
                }
                return;
            }

            // Check back button and eventually set timer to skip
            if (!buttonIsDown)
            {
                if (PlayerController.Instance.GetBackButtonDown())
                {
                    buttonIsDown = true;
                    timer = skipInSecs;

                    OnSkipping?.Invoke(this, true);
                }
            }
            else
            {
                if (PlayerController.Instance.GetBackButtonUp())
                {
                    buttonIsDown = false;
                    timer = 0;

                    OnSkipping?.Invoke(this, false);
                }
            }

            // Trying to skip
            if (buttonIsDown)
            {
                // Decrease timer
                timer -= Time.deltaTime;

                // If timer reaches zero then skip
                if (timer < 0)
                {
                    timer = 0;

                    skippable.Skip();
                }
            }
            
        }

        // Return the normalized skipping time ( from 0 to 1 )
        public float GetNormalizedTime()
        {
            return (skipInSecs - timer) / skipInSecs;
        }
    }

}
