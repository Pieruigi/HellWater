using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class RepeatActionController : ActionController
    {
        [SerializeField]
        float speed = 0.25f;

        float charge = 0;
        public float Charge
        {
            get { return charge; }
        }

        System.DateTime lastClickTime;
        

        protected override bool PerformAction()
        {
            //if ((System.DateTime.UtcNow - lastClickTime).TotalSeconds > clickTime)
            //{
            //    charging = false;
            //    charge = Mathf.Max(0, charge - Time.deltaTime);
            //}


            //if (PlayerController.GetActionButtonDown())
            //{
            //    if (charging)
            //    {
            //        float time = (float)(System.DateTime.UtcNow - lastClickTime).TotalSeconds;
            //        charge = Mathf.Min(1, charge + time);
            //    }

            //    lastClickTime = System.DateTime.UtcNow;
            //    charging = true;
            //}

           
            charge = Mathf.Max(0, charge - Time.deltaTime);
            
            Debug.Log("Charge:" + charge);

            if (PlayerController.GetActionButtonDown())
                charge = Mathf.Min(1, charge + speed);
                

            if (charge == 1)
                return true;

            return false;
        }

        public override void StartActing()
        {
            charge = 0;
            base.StartActing();
        }

        public override void StopActing()
        {
            charge = 0;
            base.StopActing();
        }
    }

}
