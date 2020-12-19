using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraAxisSwitcher : MonoBehaviour
    {
        [SerializeField]
        Transform moveHelperBack;

        [SerializeField]
        Transform moveHelperFront;

        Vector3 startPositionBack;
        Vector3 moveAxisBack;
        
        Vector3 startPositionFront;
        Vector3 moveAxisFront;
        
        LerpTrigger lerpTrigger;

        bool inside = false;

        private void Awake()
        {
            lerpTrigger = GetComponent<LerpTrigger>();

            startPositionBack = moveHelperBack.position;
            moveAxisBack = moveHelperBack.forward;
            

            startPositionFront = moveHelperFront.position;
            moveAxisFront = moveHelperFront.forward;
            
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
                if (!inside)
                {
                    inside = true;
                    PlayerCamera.Instance.ComputePositionDisable = true;
                }
               
                // We must set the axis values for when player will exit.
                if(lerpTrigger.LerpFactor < 0.5f)
                {
                    PlayerCamera.Instance.StartPosition = startPositionBack;
                    PlayerCamera.Instance.MoveAxis = moveAxisBack;
                }
                else
                {
                    PlayerCamera.Instance.StartPosition = startPositionFront;
                    PlayerCamera.Instance.MoveAxis = moveAxisFront;
                }

                // Get desired values for each axis.
                Vector3 v1 = PlayerCamera.Instance.GetFollowAxisDesiredPosition(startPositionBack, moveAxisBack);
                Vector3 v2 = PlayerCamera.Instance.GetFollowAxisDesiredPosition(startPositionFront, moveAxisFront);
                // Lerp camera desired position.
                PlayerCamera.Instance.DesiredPosition = Vector3.Lerp(v1, v2, lerpTrigger.LerpFactor);
            }
            else
            {
                if (inside)
                {
                    PlayerCamera.Instance.ComputePositionDisable = false;
                    inside = false;
                }
            }
        }
    }

}
