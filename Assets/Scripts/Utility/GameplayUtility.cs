﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum SpeedClass { None, VerySlow, Slow, Average, Fast, VeryFast }

    public class GameplayUtility
    {
        public static float GetMovementSpeedValue(SpeedClass speedClass) 
        {
            float ret = 0;

            switch (speedClass)
            {
                case SpeedClass.VerySlow:
                    ret = 1.5f;
                    break;
                case SpeedClass.Slow:
                    ret = 2f;
                    break;

                case SpeedClass.Average:
                    ret = 2.5f;
                    break;
                case SpeedClass.Fast:
                    ret = 3f;
                    break;
                case SpeedClass.VeryFast:
                    ret = 3.5f;
                    break;

            }

            return ret;
        }
    }

}