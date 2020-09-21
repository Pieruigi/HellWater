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

        bool started = false;

        protected override bool PerformAction()
        {
            
           
            charge = Mathf.Max(0, charge - Time.deltaTime);

            if (PlayerController.Instance.GetActionButtonDown())
                charge = Mathf.Min(1, charge + speed);
            

            if (charge == 1)
            {
                started = false;
                OnActionStop?.Invoke(this);
                return true;
            }
            else
            {
                if(charge > 0)
                {
                    if (!started)
                    {
                        started = true;
                        OnActionStart?.Invoke(this);
                    }
                }
                else
                {
                    if (started)
                    {
                        started = false;
                        OnActionStop?.Invoke(this);
                    }
                }
            }

            return false;
            
            
        }

        public override void EnableAction()
        {
            charge = 0;
            base.EnableAction();
        }

        public override void DisableAction()
        {
            charge = 0;
            base.DisableAction();
        }
    }

}
