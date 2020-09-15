using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class LoadingDoorController : DoorController
    {
        [SerializeField]
        int sceneBuildingIndex; // The scene you want to be loaded

        [SerializeField]
        int spawnPointIndex; // Where the player must show up in the new scene

        protected override void Close()
        {
            //throw new System.NotImplementedException();
        }

        protected override void Open()
        {
            StartCoroutine(OpenCoroutine());
        }

        IEnumerator OpenCoroutine()
        {
            PlayerController.Instance.SetDisabled(true);

            yield return CameraFader.Instance.FadeOutCoroutine(5);

            yield return new WaitForSeconds(1);

            PlayerController.Instance.GetComponent<Spawner>().SpawnPointId = spawnPointIndex;
            GameManager.Instance.LoadScene(sceneBuildingIndex);
        }
    }

}
