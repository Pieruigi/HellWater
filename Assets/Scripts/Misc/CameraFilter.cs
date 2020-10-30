using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraFilter : MonoBehaviour
    {
        [SerializeField]
        GameObject filterImage;

        [SerializeField]
        bool enableOnAwake = false;

        private void Awake()
        {
            if (!enableOnAwake)
                filterImage.SetActive(false);
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
