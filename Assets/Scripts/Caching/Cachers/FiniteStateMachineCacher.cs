using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class FiniteStateMachineCacher : Cacher
    {
        FiniteStateMachine fsm;

        protected override void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();

            base.Awake();
        }

        protected override string GetCacheValue()
        {
            if (fsm.CurrentStateId < 0)
                return "";
            
            return new FiniteStateMachineData(fsm.CurrentStateId).Format();

            
        }

        protected override void Init(string cacheValue)
        {
            FiniteStateMachineData data = new FiniteStateMachineData();
            data.Parse(cacheValue);
            fsm.ForceState(data.StateId, false);
        }
    }

}
