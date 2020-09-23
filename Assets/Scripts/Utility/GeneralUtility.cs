using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class GeneralUtility
    {
        public static GameObject ObjectPopIn(GameObject prefab, Transform parentNode, Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            GameObject ret = GameObject.Instantiate(prefab);

            ret.transform.parent = parentNode;
            ret.transform.localPosition = position;
            ret.transform.localEulerAngles = eulerAngles;
            ret.transform.localScale = Vector3.zero;

            LeanTween.scale(ret, scale, 1f).setEaseInOutBounce();

            return ret;
        }

        public static void ObjectPopOut(GameObject obj)
        {
            
            LeanTween.scale(obj, Vector3.zero, 1f).setEaseInOutBounce();

            GameObject.Destroy(obj, 1.1f);

        }
    }

}
