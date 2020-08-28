using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public class LayerController : MonoBehaviour, ILayerable
    {
        [SerializeField]
        List<GameObject> renderers;

        string backgroundLayer = "CameraLayer2";
        string foregroundLayer = "CameraLayer4";

        void Awake()
        {
            MoveToForeground();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MoveToBackground()
        {
            foreach (GameObject renderer in renderers)
                renderer.layer = LayerMask.NameToLayer(backgroundLayer);
        }

        public void MoveToForeground()
        {
            foreach (GameObject renderer in renderers)
                renderer.layer = LayerMask.NameToLayer(foregroundLayer);
        }
    }

}
