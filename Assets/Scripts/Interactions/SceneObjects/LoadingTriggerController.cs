using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;

namespace HW
{
    public class LoadingTriggerController : MonoBehaviour
    {
        [System.Serializable]
        public class LoadingData
        {
            [SerializeField]
            public int sceneBuildingIndex; // The scene you want to be loaded

            [SerializeField]
            public int spawnPointIndex; // Where the player must show up in the new scene
        }

        [SerializeField]
        List<LoadingData> dataList;

        Spawner playerSpawner;

        FiniteStateMachine fsm;

        int sceneToLoadBuildingIndex;
        int spawnPointIndex;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnFail += HandleOnFail; // We avoid to create lot of transisions
        }

        //void Start()
        //{
        //    playerSpawner = Spawner.GetSpawner(PlayerController.Instance.transform);
        //}

        void HandleOnFail(FiniteStateMachine fsm)
        {
            // The scene to load data is stored in the array at the index corresponding to the fsm state
            LoadingData data = dataList[fsm.CurrentStateId];
            sceneToLoadBuildingIndex = data.sceneBuildingIndex;
            spawnPointIndex = data.spawnPointIndex;

            //StartCoroutine(CoroutineLoad());
            GeneralUtility.LoadScene(this, sceneToLoadBuildingIndex, spawnPointIndex);
        }
        
        //IEnumerator CoroutineLoad()
        //{
        //    PlayerController.Instance.SetDisabled(true);

        //    CameraFader.Instance.TryDisableAnimator();
          
        //    yield return CameraFader.Instance.FadeOutCoroutine(5);

        //    yield return new WaitForSeconds(1);

        //    playerSpawner.SpawnPointId = spawnPointIndex;

        //    CacheManager.Instance.Update();
        //    GameManager.Instance.LoadScene(sceneToLoadBuildingIndex);
        //}
    }

}
