using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;

namespace HW
{
    public class CheckpointController : MonoBehaviour
    {
       
        [SerializeField]
        int spawnPointId = -1;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

       
        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if(oldState >= 0 && fsm.CurrentStateId < 0)
            {
                // Set the spawn point
                Spawner.GetSpawner(PlayerController.Instance.transform).SpawnPointId = spawnPointId;

                // Save
                CacheManager.Instance.Save();
            }
        }
    }

}
