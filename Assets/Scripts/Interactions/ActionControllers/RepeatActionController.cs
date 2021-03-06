﻿using System.Collections;
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

        protected override void PerformAction()
        {


            charge = Mathf.Max(0, charge - Time.deltaTime);

            if (PlayerInput.GetActionButtonDown())
                charge = Mathf.Min(1, charge + speed);


            if (charge == 1)
            {
                started = false;
                OnActionStop?.Invoke(this);
                OnActionPerformed?.Invoke(this);
            }
            else
            {
                if (charge > 0)
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
