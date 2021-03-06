﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HW
{
    public class HoldActionController : ActionController
    {
        [SerializeField]
        float speed = 0.33f;

        float charge;
        public float Charge
        {
            get { return charge; }
        }
        
        bool charging = false;

        public override void EnableAction()
        {
            charge = 0;
            charging = false;
            base.EnableAction();
        }

        public override void DisableAction()
        {
            charge = 0;
            charging = false;
            base.DisableAction();
        }

        protected override void PerformAction()
        {
            if (!charging)
            {
                if (PlayerInput.GetActionButtonDown())
                {
                    charging = true;
                    OnActionStart?.Invoke(this);
                }

            }
            else
            {
                if (PlayerInput.GetActionButtonUp())
                {
                    charging = false;
                    OnActionStop?.Invoke(this);
                }



            }

            if (charging)
                charge = Mathf.Min(1, charge + speed * Time.deltaTime);
            else
                charge = Mathf.Max(0, charge - 2f * Time.deltaTime);

            if (charge == 1)
            {
                
                OnActionStop?.Invoke(this);
                OnActionPerformed?.Invoke(this);
            }


        }

    }

}
