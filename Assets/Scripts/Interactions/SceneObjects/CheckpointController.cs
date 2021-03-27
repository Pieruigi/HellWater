using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;

namespace HW
{
    public class CheckpointController : MonoBehaviour
    {

        [SerializeField]
        int stateId = 0;

        [SerializeField]
        int spawnPointId = -1;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

       
        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            Debug.Log("Handle SaveGame");
            if(fsm.CurrentStateId == stateId && fsm.PreviousStateId != fsm.CurrentStateId)
            {
                StartCoroutine(Save());
            }
        }

        IEnumerator Save()
        {
            Debug.Log("Saving....");
            yield return new WaitForEndOfFrame();

            // Set the spawn point
            Spawner.GetSpawner(PlayerController.Instance.transform).SpawnPointId = spawnPointId;

            // Save
            CacheManager.Instance.Save();
        }
    }

}
