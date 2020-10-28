using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using HW.UI;

namespace HW
{
    public class GameManager : MonoBehaviour
    {
        //public UnityAction<int> OnGameOver;

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

        bool gameBusy = false; // Set true when game is doing some important stuff, for example cut scenes
        public bool GameBusy
        {
            get { return gameBusy; }
            set { gameBusy = value; }
        }

        MenuManager menuManager;
        InventoryUI inventory;

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
            //PlayerController.Instance.OnDead += HandleOnDead;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (loading)
                return;

            // Check in game input
            if (inGame)
            {

                // Game is busy, is problably running some cut scene or whatever else important stuff
                if (gameBusy)
                    return;

                if (PlayerInput.GetButtonDown(PlayerInput.EscapeAxis))
                {
                    Debug.Log("GameManager - player input: " + PlayerInput.EscapeAxis);

                    if (!menuManager.IsOpen())
                    {
                        // Can't be opened if you already open the inventory or a cut
                        if (inventory.IsOpen())
                            return;

                        // Open menu
                        menuManager.Open();

                        // Disable player controller
                        PlayerController.Instance.SetDisabled(true);
                    }
                    else
                    {
                        CloseMenu();

                        
                    }

                }

                if (PlayerInput.GetInventoryButtonDown())
                {
                    // Can't open both inventory and menu
                    if (menuManager.IsOpen())
                        return;

                    if (inventory.IsOpen())
                        inventory.Close();
                    else
                        inventory.Open();
                }
            }
        }

        public void CloseMenu()
        {
            // Close menu
            menuManager.Close();

            // Enable player controller
            PlayerController.Instance.SetDisabled(false);
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
            //// Reset all, flags, handles ecc.
            //ResetAll();

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

        }

        // Returns to main menu if in game, otherwise exit
        public void ExitGame()
        {
            if (inGame)
                LoadMainMenu();
            else
                Application.Quit();
        }

        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            
            // Skip the loading screen
            if(scene.buildIndex != loadingSceneIndex)
            {
                // Loading completed
                loading = false;

                menuManager = GameObject.FindObjectOfType<HW.UI.MenuManager>();

                // Update inGame flag
                if (scene.buildIndex == mainSceneIndex) // Not in game
                {
                    inGame = false;

                    // Release inventory
                    inventory = null;

                    // Open main menu
                    menuManager.Open();
                }
                else
                {
                    inGame = true;

                    // Get inventory
                    inventory = GameObject.FindObjectOfType<InventoryUI>();

                    // Close the game menu
                    menuManager.Close();
                }
            }
        }

       
    }

}
