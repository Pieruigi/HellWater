using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    [ExecuteAlways]
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        bool external;

        [SerializeField]
        bool overrideAngles;

        [SerializeField]
        Vector3 angles;

        [SerializeField]
        Transform target;

        [SerializeField]
        float distance = 100;
        Vector3 eulerAngles;

        GameObject player;

        private void Awake()
        {
            if (!overrideAngles)
            {
                eulerAngles = new Vector3(0, -20f, 0f);

                if (external)
                    eulerAngles.x = 20f;
                else
                    eulerAngles.x = 50f;
            }
            else
            {
                eulerAngles = angles;
            }

            transform.eulerAngles = eulerAngles;
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

        
    }

}
