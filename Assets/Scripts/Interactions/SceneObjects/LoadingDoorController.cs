using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;

namespace HW
{
    public class LoadingDoorController : DoorController
    {
        [SerializeField]
        int sceneBuildingIndex; // The scene you want to be loaded

        [SerializeField]
        int spawnPointIndex; // Where the player must show up in the new scene

        //Spawner playerSpawner;

        

        protected override void Start()
        {
            //playerSpawner = Spawner.GetSpawner(PlayerController.Instance.transform);
            base.Start();
        }

        protected override void Close()
        {
            //throw new System.NotImplementedException();
        }

        protected override void Open()
        {
            //StartCoroutine(OpenCoroutine());
            GeneralUtility.LoadScene(this, sceneBuildingIndex, spawnPointIndex);
        }

        //IEnumerator OpenCoroutine()
        //{
        //    PlayerController.Instance.SetDisabled(true);

        //    CameraFader.Instance.TryDisableAnimator();
          
        //    yield return CameraFader.Instance.FadeOutCoroutine(5);

        //    yield return new WaitForSeconds(1);

        //    playerSpawner.SpawnPointId = spawnPointIndex;

        //    //PlayerController.NewSceneSpawnPointId = spawnPointIndex;
        //    CacheManager.Instance.Update();
        //    GameManager.Instance.LoadScene(sceneBuildingIndex);
        //}
    }

}
