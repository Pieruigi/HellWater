using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CraneController : MonoBehaviour
    {
        [SerializeField]
        GameObject branchPivot;

        [SerializeField]
        GameObject slider;

        [SerializeField]
        GameObject magnet;

        [SerializeField]
        Transform magnetRoot;

        float maxSpeed = 5f;
        


        // Start is called before the first frame update
        void Start()
        {
            // Only for test
            PlayerController.Instance.SetDisabled(true);
            GetComponentInChildren<Camera>().gameObject.SetActive(true);
            Camera.main.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // Get input
            Vector2 input = new Vector2(PlayerInput.GetHorizontalAxisRaw(), PlayerInput.GetVerticalAxisRaw());

            if (input == Vector2.zero)
                return;

            // Check horizontal collision
            //Ray ray = new Ray(magnetRoot.position, input.normalized);
            //float speed = maxSpeed * Time.deltaTime * input.x > 0 ? ;
            //if(!Physics.Raycast(ray, speed))
            //{
            //    // No collision, move
            //    Vector3 angles = branchPivot.transform.eulerAngles;
            //    angles.y += maxSpeed * Time.deltaTime;
            //    branchPivot.transform.eulerAngles = angles;
            //}

        }
    }

}
