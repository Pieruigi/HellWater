using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;
using UnityEngine.SceneManagement;

namespace HW
{
    public class GameManager : MonoBehaviour
    {

        int mainSceneIndex = 0;
        int loadingSceneIndex = 1;
        int startingSceneIndex = 2;

        private Language language = Language.Italian;
        public Language Language
        {
            get { return language; }
        }

        #region LOADING
        public static GameManager Instance { get; private set; }

        bool inGame = false;
        public bool InGame
        {
            get { return inGame; }
        }
        bool loading = false;
        public bool Loading
        {
            get { return loading; }
        }
        #endregion

        #region GAMEPLAY_FlAGS
        bool cutSceneRunning = false;
        public bool CutSceneRunning
        {
            get { return cutSceneRunning; }
            set { cutSceneRunning = value; }
        }

        bool inventoryOpen = false;
        public bool InventoryOpen
        {
            get { return inventoryOpen; }
            set { inventoryOpen = value; }
        }
        #endregion

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                SceneManager.sceneLoaded += HandleOnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
        }

        public bool IsSaveGameAvailable()
        {
            return CacheManager.Instance.IsSaveGameAvailable();
        }

        public void LoadMainMenu()
        {
            LoadScene(mainSceneIndex);
        }

        public void LoadScene(int index)
        {
            // Reset all, flags, handles ecc.
            ResetAll();

            SceneLoader.LoadingSceneIndex = index;
            SceneManager.LoadScene(index);
            loading = true;


        }

        // Starts a new game
        public void StartNewGame()
        {
            // Delete cache when a new game is started
            CacheManager.Instance.Delete();

            // Load level
            LoadScene(startingSceneIndex);
            
            //SceneLoader.LoadingSceneIndex = startingSceneIndex;
            //SceneManager.LoadScene(loadingSceneIndex);
            //loading = true;
        }

        // Load an existing game
        public void ContinueGame()
        {
            // Load cache first
            CacheManager.Instance.Load();

            // Get the index of the scene that must be loaded
            string index;
            if(!CacheManager.Instance.TryGetCacheValue(Constants.CacheCodeSceneIndex, out index))
                throw new System.Exception("Save game must be corrupted: unable to find the scene to load.");

            // Load the saved level
            LoadScene(int.Parse(index));

            //SceneLoader.LoadingSceneIndex = int.Parse(index);
            //SceneManager.LoadScene(loadingSceneIndex);
            //loading = true;

        }

        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("SceneLoaded:" + scene.buildIndex);

          
            // Skip the loading screen
            if(scene.buildIndex != loadingSceneIndex)
            {
                // Loading completed
                loading = false;

                // Update inGame flag
                if(scene.buildIndex == mainSceneIndex)
                {
                    inGame = false;
                }
                else
                {
                    inGame = true;
                }
            }
        }

        private void ResetAll()
        {
            cutSceneRunning = false;
            inventoryOpen = false;
        }
    }

}
