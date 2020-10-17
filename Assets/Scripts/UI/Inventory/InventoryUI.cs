using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.UI;

namespace HW.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        bool open = false;
        bool wait = false;

        float height;

        List<Item> items = new List<Item>();

        ScrollRect scrollRect;

        private void Awake()
        {
            height = (panel.transform as RectTransform).sizeDelta.y;
            Debug.Log("InventoryHeight:" + height);
        }

        // Start is called before the first frame update
        void Start()
        {
            scrollRect = GetComponentInChildren<ScrollRect>();

            Debug.Log("NumOfSteps:" + scrollRect.horizontalScrollbar.numberOfSteps);
            Debug.Log("Value:" + scrollRect.horizontalScrollbar.value);
            Debug.Log("Direction:" + scrollRect.horizontalScrollbar.direction);
            scrollRect.horizontalScrollbar.numberOfSteps = 0;
            scrollRect.horizontalScrollbar.value = 0f;
            
        }

        // Update is called once per frame
        void Update()
        {
            //scrollRect.horizontalScrollbar.value = 0.2f;
            //scrollRect.velocity = new Vector2(-1000f, 1000f);
            if (GameManager.Instance != null && GameManager.Instance.CutSceneRunning)
                return;

            if (wait)
                return;

           
            CheckInput();

        }

        public void HandleOnValueChange(Vector2 v)
        {
            Debug.Log("Vector2:" + v.x + ","+v.y);
        }

        void Open()
        {
            // Fill data in
            Fill();

            // Show the inventory
            LeanTween.moveY((RectTransform)panel.transform, -height, 0.5f).setEaseSpring().setOnComplete(HandleOnComplete);
        }

        void Close()
        {
            // Clear data
            Clear();

            // Restart playing
            GameManager.Instance.Unpause();

            // Hide the inventory
            LeanTween.moveY((RectTransform)panel.transform, height, 0.5f).setEaseSpring().setOnComplete(HandleOnComplete);
        }

        void HandleOnComplete()
        {
            wait = false;

            if (open)
                GameManager.Instance.Pause();


        }

        void CheckInput()
        {
            if (PlayerController.Instance.GetInventoryButtonDown())
            {
                if (!open)
                    Open();
                else
                    Close();

                GameManager.Instance.InventoryOpen = open;
                wait = true;
                open = !open;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("NumOfSteps:" + scrollRect.horizontalScrollbar.numberOfSteps);
                Debug.Log("Value:" + scrollRect.horizontalScrollbar.value);
                Debug.Log("Direction:" + scrollRect.horizontalScrollbar.direction);
            }

            if (open)
                CheckSelectionInput();

        }

        void Clear()
        {
            // Free list
            items.Clear();
        }

        void Fill()
        {
            // Get all the items
            IList<Item> items = Inventory.Instance.GetItems();

            // Loop through all the items
            foreach(Item item in items)
            {
                // Create the UI element

            }
        }

        void CheckSelectionInput()
        {
            float axisRaw = PlayerController.Instance.GetHorizontalAxisRaw();

            Debug.Log("Input:"+axisRaw);

            //scrollRect.velocity = new Vector2(axisRaw, axisRaw);
            
            
            
        }
    }

}
