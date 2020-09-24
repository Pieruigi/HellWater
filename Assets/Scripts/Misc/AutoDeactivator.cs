using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class AutoDeactivator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
