using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CachingSystem
{
    public class EnemyGroupCacher : Cacher
    {
       
        protected override string GetCacheValue()
        {
            EnemyGroup eg = GetComponent<EnemyGroup>();
            FiniteStateMachine fsm = GetComponent<FiniteStateMachine>();
            EnemyGroupData data = new EnemyGroupData(fsm.CurrentStateId, new List<bool>(eg.DeadList));
            return data.Format();
        }

        protected override void Init(string cacheValue)
        {
            // Parse data
            EnemyGroupData data = new EnemyGroupData();
            data.Parse(cacheValue);

            // Init fsm  
            FiniteStateMachine fsm = GetComponent<FiniteStateMachine>();
            fsm.ForceState(data.State, false, false);

            Debug.Log("Initialized:" + data.State);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }
    }

}
