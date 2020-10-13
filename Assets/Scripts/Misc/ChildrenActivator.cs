using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class ChildrenActivator : MonoBehaviour, IActivable
    {
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate(bool value)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
                transform.GetChild(i).gameObject.SetActive(value);
        }

        public bool IsActive()
        {
            return true;
        }
    }

}
