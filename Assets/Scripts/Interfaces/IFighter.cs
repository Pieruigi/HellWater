using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW.Interfaces
{
    public interface IFighter
    {
        bool Fight(Transform target);

        float GetFightingRange();

        bool AttackAvailable();
       
    }

}
