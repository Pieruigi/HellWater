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

        public override void StartActing()
        {
            charge = 0;
            charging = false;
            base.StartActing();
        }

        public override void StopActing()
        {
            charge = 0;
            charging = false;
            base.StopActing();
        }

        protected override bool PerformAction()
        {
            if (!charging)
            {
                if (PlayerController.Instance.GetActionButtonDown())
                {
                    charging = true;
                }
                    
            }
            else
            {
                if (PlayerController.Instance.GetActionButtonUp())
                    charging = false;
                
                    
            }

            if (charging)
                charge = Mathf.Min(1, charge + speed * Time.deltaTime);
            else
                charge = Mathf.Max(0, charge - 2f * Time.deltaTime);
            
            if (charge == 1)
                return true;

            return false;
        }
    }

}
