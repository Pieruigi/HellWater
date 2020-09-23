using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum CameraOrientation { ToNorth, ToSouth, ToEast, ToWest }

    [ExecuteAlways]
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        CameraOrientation cameraOrientation;

        [SerializeField]
        bool external;

        [SerializeField]
        bool overrideRotX = false;

        [SerializeField]
        Transform target;

        [SerializeField]
        float distance = 100;

        Vector3 eulerAngles;
        float yAngle = -20f;
        float xAngle;

        GameObject player;

        

        public static PlayerCamera Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;

                eulerAngles = new Vector3(0, yAngle, 0f);

                if (external)
                    eulerAngles.x = 20f;
                else
                {
                    if (!overrideRotX)
                        eulerAngles.x = 80f;
                    else
                        eulerAngles.x = transform.eulerAngles.x;

                }
                    

                xAngle = eulerAngles.x;

                //worldDirection = new Vector3(1f, 0f, 1f);

                ComputeOrientationAngle();
                //ComputeWorldDirection();


                transform.eulerAngles = eulerAngles;

                
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            //player = PlayerController.Instance.gameObject;
            transform.position = target.position - transform.forward * distance;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // Follow the player
            transform.position = target.position - transform.forward * distance;

            
        }

        public Vector3 GetForwardOrientation()
        {
            Vector3 ret = Vector3.zero;
            switch (cameraOrientation)
            {
                case CameraOrientation.ToNorth:
                    ret = Vector3.forward;
                    break;
                case CameraOrientation.ToSouth:
                    ret = Vector3.back;
                    break;
                case CameraOrientation.ToEast:
                    ret = Vector3.right;
                    break;
                case CameraOrientation.ToWest:
                    ret = Vector3.left;
                    break;
            }

            return ret;
        }

        public Vector3 GetRightOrientation()
        {
            Vector3 ret = Vector3.zero;
            switch (cameraOrientation)
            {
                case CameraOrientation.ToNorth:
                    ret = Vector3.right;
                    break;
                case CameraOrientation.ToSouth:
                    ret = Vector3.left;
                    break;
                case CameraOrientation.ToEast:
                    ret = Vector3.back;
                    break;
                case CameraOrientation.ToWest:
                    ret = Vector3.forward;
                    break;
            }

            return ret;
        }


        void ComputeOrientationAngle()
        {
            switch (cameraOrientation)
            {
                case CameraOrientation.ToNorth:
                    eulerAngles.y = yAngle;
                    break;
                case CameraOrientation.ToSouth:
                    eulerAngles.y = yAngle + 180f;
                    break;
                case CameraOrientation.ToEast:
                    eulerAngles.y = yAngle + 90f;
                    break;
                case CameraOrientation.ToWest:
                    eulerAngles.y = yAngle - 90;
                    break;
            }
        }

        
    }

}
