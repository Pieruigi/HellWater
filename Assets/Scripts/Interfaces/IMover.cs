using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW.Interfaces
{
    //public delegate void StartMoving(IMover mover);
    //public delegate void StopMoving(IMover mover);
    //public delegate void DestinationReached(IMover mover);

    public interface IMover
    {
        //event StartMoving OnStartMoving;
        //event StopMoving OnStopMoving;
        event UnityAction<IMover> OnDestinationReached;

        void MoveTo(Vector3 destination);

        void SetMaxSpeed(float maxSpeed);

        float GetMaxSpeed();
        
        float GetSpeed();
     

    }

}
