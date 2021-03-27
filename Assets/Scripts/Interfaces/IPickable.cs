using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.Interfaces
{
    public interface IPickable
    {
        /// <summary>
        /// The object you need to create.
        /// </summary>
        GameObject GetObjectPrefab();

        /// <summary>
        /// Return a 3d game object to set as place holder in the scene.
        /// </summary>
        /// <returns></returns>
        GameObject GetPlaceHolderPrefab();
    }

}
