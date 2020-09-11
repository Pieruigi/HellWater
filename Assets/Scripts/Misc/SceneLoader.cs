using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HW
{
    public class SceneLoader : MonoBehaviour
    {

        public static int LoadingSceneIndex;

        // Start is called before the first frame update
        void Start()
        {
            // Start loading the game scene
            StartCoroutine(LoadingSceneAsync());
        }

        IEnumerator LoadingSceneAsync()
        {
            // Start async operation in order to have some control over the process
            AsyncOperation loading = SceneManager.LoadSceneAsync(LoadingSceneIndex);

            // Just wait until loading is completed and in the meanwhile update the progress bar
            while (!loading.isDone)
            {
                //progressBar.fillAmount = loading.progress;

                // Just wait the next frame
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
