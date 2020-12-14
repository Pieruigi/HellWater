using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class GeneralUtility
    {
        public static GameObject ObjectPopIn(GameObject prefab, Transform parentNode, Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            GameObject ret = GameObject.Instantiate(prefab);

            ret.transform.parent = parentNode;
            ret.transform.localPosition = position;
            ret.transform.localEulerAngles = eulerAngles;
            ret.transform.localScale = Vector3.zero;

            LeanTween.scale(ret, scale, 1f).setEaseInOutBounce();

            return ret;
        }

        public static void ObjectPopOut(GameObject obj)
        {
            if (obj == null)
                return;

            LeanTween.scale(obj, Vector3.zero, 1f).setEaseInOutBounce();

            GameObject.Destroy(obj, 1.1f);

        }

        public static void LoadScene(MonoBehaviour caller, int sceneBuildIndex, int spawnPointIndex)
        {
            caller.StartCoroutine(CoroutineLoadScene(sceneBuildIndex, spawnPointIndex));
        }


        static IEnumerator CoroutineLoadScene(int sceneBuildIndex, int spawnPointIndex)
        {
            PlayerController.Instance.SetDisabled(true);

            CameraFader.Instance.TryDisableAnimator();

            yield return CameraFader.Instance.FadeOutCoroutine(5);

            yield return new WaitForSeconds(0.1f);

            Spawner.GetSpawner(PlayerController.Instance.transform).SpawnPointId = spawnPointIndex;

            HW.CachingSystem.CacheManager.Instance.Update();
            GameManager.Instance.LoadScene(sceneBuildIndex);
        }
    }

}
