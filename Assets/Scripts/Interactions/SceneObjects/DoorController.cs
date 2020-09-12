using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class DoorController : MonoBehaviour
    {
     

        [SerializeField]
        GameObject sceneObject;

        [SerializeField]
        bool invertAngle = false;

        [SerializeField]
        float speed = 0.5f;
        
        InteractionController interactionController;

        float time;

        float openAngle = 0;

        private void Awake()
        {
            interactionController = GetComponent<InteractionController>();
            interactionController.OnStateChange += HandleOnStateChange;

            time = 1f / speed;
         
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(int oldState, int newState)
        {
            float angle = 90;
            if(newState == Constants.DoorOpenState)
            {
                Vector3 dir = transform.position - PlayerController.Instance.transform.position;
                if (Vector3.Dot(dir, sceneObject.transform.forward) > 0)
                    angle = -90;

                if (invertAngle)
                    angle *= -1;
            }
            else
            {
                if (newState == Constants.DoorClosedState)
                {
                    angle = openAngle * -1f;
                }
            }

            openAngle = angle;

            LeanTween.rotateAroundLocal(sceneObject, Vector3.up, angle, time).setEaseOutElastic();

            
        }
    }

}
