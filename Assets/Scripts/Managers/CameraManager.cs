using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        //private float cameraRotationAngle = 

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
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
    }

}
