using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using UnityEngine.UI;

namespace HW.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField]
        Image icon;

        [SerializeField]
        TMPro.TMP_Text _name;


        Item item;
        public Item Item
        {
            get { return item; }
        }

        Vector3 scaleDefault;
        bool scaling = false;
        Vector3 desiredScale;
        float scaleSpeed = 5f;

        float deltaTime;
        System.DateTime lastTime;

        private void Awake()
        {
            _name.text = "";
            scaleDefault = Vector3.one;
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (scaling)
            {
                System.DateTime now = System.DateTime.UtcNow;
                float delta = (float)(now - lastTime).TotalSeconds;
                lastTime = now;

                transform.localScale = Vector3.MoveTowards(transform.localScale, desiredScale, scaleSpeed * delta);
                if (transform.localScale == desiredScale)
                    scaling = false;
            }
                
                
            
        }

        public void Clear()
        {
            item = null;
            icon.sprite = null;
            _name.text = "";
        }

        public bool IsEmpty()
        {
            return item == null;
        }

        public void Init(Item item)
        {
            this.item = item;
            icon.sprite = item.Icon;
            _name.text = item.Name;
            transform.localScale = scaleDefault;
        }

        public void Select()
        {
            Debug.Log("Selecting:" + name);
            //transform.localScale = scaleDefault * 1.2f;
            desiredScale = scaleDefault * 1.2f;
            scaling = true;
            lastTime = System.DateTime.UtcNow;
        }

        public void Unselect()
        {
            Debug.Log("Unselecting:" + name);
            //transform.localScale = scaleDefault;
            desiredScale = scaleDefault;
            scaling = true;
            lastTime = System.DateTime.UtcNow;
        }
    }

}
