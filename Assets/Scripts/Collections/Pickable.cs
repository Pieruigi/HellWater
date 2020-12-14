using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{
    

    public class Pickable : ScriptableObject, IPickable
    {
        [SerializeField]
        string code;

        [SerializeField]
        GameObject placeHolderPrefab;

        [SerializeField]
        GameObject objectPrefab;

        [SerializeField]
        Sprite icon;

        [SerializeField]
        string description;


        public GameObject GetPlaceHolderPrefab()
        {
            return placeHolderPrefab;
        }

        public GameObject GetObjectPrefab()
        {
            return objectPrefab;
        }

        
    }

}
