using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraLookSwitcher : MonoBehaviour
    {
        [SerializeField]
        Transform lookAxisHelperBack;

        [SerializeField]
        Transform lookAxisHelperFront;

        LerpTrigger lerpTrigger;

        Vector3 lookAxisBack, lookAxisFront;

        private void Awake()
        {
            lerpTrigger = GetComponent<LerpTrigger>();

            lookAxisBack = lookAxisHelperBack.forward;
            lookAxisFront = lookAxisHelperFront.forward;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (lerpTrigger.Inside)
            {
                if(lerpTrigger.LerpFactor < 0.5f)
                {
                    PlayerCamera.Instance.LookAxis = lookAxisBack;
                }
                else
                {
                    PlayerCamera.Instance.LookAxis = lookAxisFront;
                }

                PlayerCamera.Instance.LookAxis = Vector3.Lerp(lookAxisBack, lookAxisFront, lerpTrigger.LerpFactor);
            }
        }


    }

}
