using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    [ExecuteAlways]
    public class RotationController : MonoBehaviour
    {
        [SerializeField]
        Transform child;

        
        [SerializeField]
        bool keepChildOrientation;

        Vector3 childEulerAnglesDefault;

        private void Awake()
        {
            childEulerAnglesDefault = new Vector3(Constants.OrthographicAngle, 0, 0);
            Adjust();
            
            //childEulerAnglesDefault = child.localEulerAngles;
            //childEulerAnglesDefault.x = Constants.OrthographicAngle;
            //child.localEulerAngles = childEulerAnglesDefault;
        }

        // Start is called before the first frame update
        void Start()
        {
                            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            Adjust();
            
        }

        void Adjust()
        {
            //return;
            // Get the parent rotation global angle
            float y = transform.parent.eulerAngles.y;

            // Rotate this transform in the opposite direction
            Vector3 v = transform.localEulerAngles;
            v.y = -y;
            transform.localEulerAngles = v;


            if (!keepChildOrientation)
            {
                // Reset child angles
                child.localEulerAngles = childEulerAnglesDefault;

                // Rotate child
                child.RotateAround(child.transform.position, child.transform.up, y);
            }
        }
    }

}
