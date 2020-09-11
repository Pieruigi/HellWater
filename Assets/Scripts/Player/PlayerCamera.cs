using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        bool external;

        float distance = 100;
        Vector3 eulerAngles;

        GameObject player;

        private void Awake()
        {
            eulerAngles = new Vector3(0, -20f, 0f);

            if (external)
                eulerAngles.x = 20f;
            else
                eulerAngles.x = 50f;

            transform.eulerAngles = eulerAngles;
        }

        // Start is called before the first frame update
        void Start()
        {
            player = PlayerController.Instance.gameObject;
            transform.position = player.transform.position - transform.forward * distance;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // Follow the player
            transform.position = player.transform.position - transform.forward * distance;

            
        }

        
    }

}
