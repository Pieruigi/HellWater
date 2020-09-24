using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    // Just a simple door that opens rotating around its hinges
    public class SimpleDoorController : DoorController
    {
      
        [SerializeField]
        GameObject sceneObject;

        [SerializeField]
        bool invertAngle = false;

        [SerializeField]
        float speed = 0.5f; // How fast
        
        float time;

        float currAngle = 0;

        InteractionController ctrl;
        
        protected override void Awake()
        {
            base.Awake();
            time = 1f / speed;

            ctrl = GetComponent<InteractionController>();
        }

        protected override void Open()
        {
            float angle = 90f;
            
            // Check whether the door must rotate cw or ccw
            Vector3 dir = transform.position - PlayerController.Instance.transform.position;
            if (Vector3.Dot(dir, sceneObject.transform.forward) > 0)
                angle = -90;

            // Switch from cw to ccw ( or viceversa, I don't remember )
            if (invertAngle)
                angle *= -1;

            currAngle = angle;

            LeanTween.rotateAroundLocal(sceneObject, Vector3.up, angle, time).setEaseOutElastic();

            if (ctrl)
                ctrl.ForceDisabled(true);
        }

        protected override void Close()
        {
            float angle = 90;
            angle = currAngle * -1f;
            currAngle = angle;

            LeanTween.rotateAroundLocal(sceneObject, Vector3.up, angle, time).setEaseOutElastic();

            if (ctrl)
                ctrl.ForceDisabled(false);
        }

    

    }

}
