using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class GameObjectActivator : MonoBehaviour, IActivable
    {
        [SerializeField]
        [Tooltip("If false only this game object will be de/activated, otherwise all its children ( but not this game object ).")]
        bool isGroup = false;

        Transform[] children;

        bool active = false;

        // Start is called before the first frame update
        void Start()
        {
            // Is is a group then fill the children list
            if (isGroup)
            {
                children = GetComponentsInChildren<Transform>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate(bool value)
        {
            active = value;
            if (isGroup)
            {
                foreach (Transform child in children)
                    child.gameObject.SetActive(value);
            }
            else
            {
                gameObject.SetActive(value);
            }
        
        }

        public bool IsActive()
        {
            return active;
        }
    }

}
