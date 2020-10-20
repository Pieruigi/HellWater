using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace HW.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public UnityAction OnOpen;
        public UnityAction OnClose;
        public UnityAction<ItemUI> OnSelect;

        [SerializeField]
        GameObject panel;

        [SerializeField]
        Transform content;

        [SerializeField]
        GameObject itemTemplate;

        [SerializeField]
        TMPro.TMP_Text description;

        bool open = false;
        bool wait = false;

        float height;

        List<GameObject> items = new List<GameObject>();
        int firstEmptyIndex;

        ScrollRect scrollRect;

        int maxItems = 8; // The maximum number of items at any moment in the inventory
        int maxShowingItems = 3; // The maximum number of items that can be shown
        float disp = 0; // How much we move the pointer ( from 0 to 1 ) for each step
        int selectedId = -1; // The selected item from the list ( -1 means there is no item at all )
        bool selecting = false;
        bool scrolling = false;
        float scrollSpeed = 2.5f;
        int sameDirectionCount = 0;
        float scrollValue = 0;
        
        float deltaTime = 0;
        public float DeltaTime
        {
            get { return deltaTime; }
        }
        DateTime lastTime;
        DateTime lastMove;

        private void Awake()
        {
            height = (panel.transform as RectTransform).sizeDelta.y;
            Debug.Log("InventoryHeight:" + height);

            // Create item conteiners
            for(int i=0; i<maxItems; i++)
            {
                GameObject o = GameObject.Instantiate(itemTemplate, content);
                items.Add(o);
            }
            

            // Destroy the template
            GameObject.Destroy(itemTemplate);

            // Step displacement
            disp = 1f / (float)(maxItems - maxShowingItems);
            
        }

        // Start is called before the first frame update
        void Start()
        {
            scrollRect = GetComponentInChildren<ScrollRect>();

        }

        // Update is called once per frame
        void Update()
        {
            
            //if (open)
            //{
            //    DateTime now = DateTime.UtcNow;
            //    deltaTime = (float)(now - lastTime).TotalSeconds;
            //    lastTime = now;

            //    if (selecting)
            //        Select();
            //}

            if (GameManager.Instance != null && GameManager.Instance.CutSceneRunning)
                return;

            if (wait)
                return;




            // Check the player input
            CheckOpenCloseInput();

            // We need to compute the delta time since the timeScale is zero
            if (open)
            {
                DateTime now = DateTime.UtcNow;
                deltaTime = (float)(now - lastTime).TotalSeconds;
                lastTime = now;

                CheckSelectionInput();

                if (selecting)
                    Select();
            }

        }



        void Open()
        {
            // Just to be sure
            Clear();

            // Fill data in
            Fill();

            // Select the first if exists
            if (!items[0].GetComponent<ItemUI>().IsEmpty())
                selectedId = 0;
            else
                selectedId = -1;

            // Set description if selected
            if (selectedId < 0)
            {
                description.text = "";
            }
            else
            {
                description.text = items[0].GetComponent<ItemUI>().Item.Description;
                items[selectedId].GetComponent<ItemUI>().Select();
            }
                

            // Init for deltaTime
            lastTime = DateTime.UtcNow;


           
            // Show up the inventory
            LeanTween.moveY((RectTransform)panel.transform, -height, 0.5f).setEaseSpring().setOnComplete(HandleOnComplete);

            // Call event
            OnOpen?.Invoke();
        }

        void Close()
        {
            // Restart playing
            GameManager.Instance.Unpause();

            Debug.Log("Lean Tween Close:" + height);

            // Hide the inventory
            LeanTween.moveY((RectTransform)panel.transform, 0, 0.25f).setOnComplete(HandleOnComplete);

            // Call event
            OnClose?.Invoke();
        }

        void HandleOnComplete()
        {
            wait = false;

            if (open)
            {
                GameManager.Instance.Pause();
            }
            else
            {
                Clear();
            }

        }

        void CheckOpenCloseInput()
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
    

        }

        void Clear()
        {
            // Clear
            foreach (GameObject g in items)
                g.GetComponent<ItemUI>().Clear();

            selectedId = -1;
            description.text = "";
            selecting = false;
            firstEmptyIndex = 0;
            sameDirectionCount = 0;
        }

        void Fill()
        {
            // Get all the items
            IList<Item> items = Inventory.Instance.GetItems();

            // Loop through all the items
            for(int i=0; i<items.Count; i++)
                this.items[i].GetComponent<ItemUI>().Init(items[i]); // Init the item UI


        }

        void CheckSelectionInput()
        {

            if ((DateTime.UtcNow - lastMove).TotalSeconds < .25f)
                return;

            selecting = false;

            float axisRaw = PlayerController.Instance.GetHorizontalAxisRaw();

            // Nothing to do
            if (axisRaw != 1 && axisRaw != -1)
                return;

            // Invalid selection
            firstEmptyIndex = items.FindIndex(i => i.GetComponent<ItemUI>().IsEmpty());
            if ((selectedId == 0 && axisRaw == -1) ||(selectedId == firstEmptyIndex - 1 && axisRaw == 1))
                return;
                       
            int oldSelectedId = selectedId;
            scrolling = false;
            if (axisRaw == -1)
            {
                selectedId--;
                //if (sameDirectionCount > 0)
                //    sameDirectionCount = 0;
                if (sameDirectionCount == 0)
                    scrolling = true;
                else
                    sameDirectionCount = Mathf.Max(sameDirectionCount-1, 0);
                
            }
            else
            {
                selectedId++;
                //if(sameDirectionCount < 0)
                //    sameDirectionCount = 0;

                if (sameDirectionCount == maxShowingItems-1)
                    scrolling = true;
                else
                    sameDirectionCount = Mathf.Min(sameDirectionCount+1, maxShowingItems);
            }

            // Deselect old
            items[oldSelectedId].GetComponent<ItemUI>().Unselect();

            // Select new
            items[selectedId].GetComponent<ItemUI>().Select();

            // Description
            description.text = items[selectedId].GetComponent<ItemUI>().Item.Description;

            Debug.Log("Start moving...");

            
            

            if (scrolling)
            {
                float d = (selectedId - oldSelectedId) * disp;
                scrollValue = scrollRect.horizontalNormalizedPosition + d;
            }
                
    

            selecting = true; // Start moving

            lastMove = DateTime.UtcNow;

            // Call event
            OnSelect?.Invoke(items[selectedId].GetComponent<ItemUI>());
        }

        void Select()
        {
         
            if(scrolling)
                scrollRect.horizontalNormalizedPosition = Mathf.MoveTowards(scrollRect.horizontalNormalizedPosition, scrollValue, scrollSpeed * deltaTime);
        }
 
    }

}
