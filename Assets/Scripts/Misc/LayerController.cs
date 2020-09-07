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
        string foregroundLayer = "CameraLayer3";

        int backgroundCount = 0;

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
            backgroundCount++;

            foreach (GameObject renderer in renderers)
                renderer.layer = LayerMask.NameToLayer(backgroundLayer);
        }

        public void MoveToForeground()
        {
            backgroundCount = Mathf.Max(0, backgroundCount - 1);

            if (backgroundCount > 0)
                return;

            foreach (GameObject renderer in renderers)
                renderer.layer = LayerMask.NameToLayer(foregroundLayer);
        }
    }

}
