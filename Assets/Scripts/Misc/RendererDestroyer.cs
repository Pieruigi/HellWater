using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{

    public class RendererDestroyer : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
            Destroy(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
