using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HW
{
    public class HoldActionController : ActionController
    {
        [SerializeField]
        float time = 5;

        DateTime last;
        bool down = false;

        protected override bool PerformAction()
        {
            if (!down)
            {
                if (PlayerController.GetActionDown())
                {
                    down = true;
                    last = System.DateTime.UtcNow;
                }
                    
            }
            else
            {
                if (!PlayerController.GetActionDown())
                    down = false;
            }

            if (down)
            {
                if ((System.DateTime.UtcNow - last).TotalSeconds > time)
                    return true;
            }

            return false;
        }
    }

}
