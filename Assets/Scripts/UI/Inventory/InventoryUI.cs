using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

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

        private void Awake()
        {
            height = (panel.transform as RectTransform).sizeDelta.y;
            Debug.Log("InventoryHeight:" + height);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CutSceneRunning)
                return;

            if (wait)
                return;

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

            //if (open)
            //{
            //    CheckInput();
            //}
        }


        void Open()
        {
            // Fill data in
            Fill();

            LeanTween.moveY((RectTransform)panel.transform, -height, 0.5f).setEaseSpring().setOnComplete(HandleOnComplete);
        }

        void Close()
        {
            // Clear data
            Clear();

            LeanTween.moveY((RectTransform)panel.transform, height, 0.5f).setEaseSpring().setOnComplete(HandleOnComplete);
        }

        void HandleOnComplete()
        {
            wait = false;
            if(open)
                GameManager.Instance.Pause();
            else
                GameManager.Instance.Unpause();
        }

        void CheckInput()
        {
            while(open)
            if (PlayerController.Instance.GetInventoryButtonDown())
            {
                    open = false;
                    GameManager.Instance.Unpause();
            }
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
    }

}
