using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW.UI
{
    public class MenuManager : MonoBehaviour
    {
        public UnityAction OnOpen;
        public UnityAction OnClose;

        [SerializeField]
        List<GameObject> menuList;
        protected IList<GameObject> MenuList
        {
            get { return menuList.AsReadOnly(); }
        }

        [SerializeField]
        GameObject menuDefault;

        //[SerializeField]
        //bool closedOnStart = false;

        GameObject current;

        bool open = false;
        //public bool IsOpen
        //{
        //    get { return open; }
        //    protected set { open = value; }
        //}

        public GameObject Current
        {
            get { return current; }
        }

        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            //if (!closedOnStart)
                //Open();
            //else
            //    Close();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public bool IsOpen()
        {
            return open;
        }

        public virtual void Close()
        {
            open = false;
            HideAll();

            
            OnClose?.Invoke();
        }

        public virtual void Open()
        {
           
            HideAll();
            open = true;
            Open(menuDefault);

            
            OnOpen?.Invoke();
        }

        public void Open(GameObject menu)
        {
            if (!open)
            {
                Debug.LogWarning("The menu is closed; call open() first.");
                return;
            }

            if (current != null)
                current.SetActive(false);

            current = menu;
            current.SetActive(true);
        }


        protected void HideAll()
        {
            foreach (GameObject menu in menuList)
                menu.SetActive(false);
        }


    }

}
