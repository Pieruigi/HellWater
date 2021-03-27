using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Collections
{
    

    public class Pickable : ScriptableObject, IPickable
    {

        [SerializeField]
        GameObject objectPrefab;

        [SerializeField]
        GameObject placeHolderPrefab;


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
