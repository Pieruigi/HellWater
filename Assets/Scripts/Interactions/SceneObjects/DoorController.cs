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
        
        InteractionController interactionController;

        GameObject player;

        private void Awake()
        {
            interactionController = GetComponent<InteractionController>();
            interactionController.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag(Tags.Player);
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
                Vector3 dir = sceneObject.transform.position - player.transform.position;
                if (Vector3.Dot(dir, sceneObject.transform.forward) > 0)
                    angle = -90;
                
            }
            else
            {
                if (newState == Constants.DoorClosedState)
                {
                    Vector3 dir = sceneObject.transform.position - player.transform.position;
                    if (Vector3.Dot(dir, sceneObject.transform.forward) < 0)
                        angle = -90;

                }
            }

            if (invertAngle)
                angle *= -1;

            LeanTween.rotateAroundLocal(sceneObject, Vector3.up, angle, 0.5f).setEaseOutElastic();
        }
    }

}
