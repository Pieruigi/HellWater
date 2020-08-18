using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW
{
    public enum RenderingLayer { Background, Foreground }

    public class MeshController : MonoBehaviour, ILayerable
    {
        [SerializeField]
        Transform root;

        // The global euler angles
        Vector3 eulerAngles;

        float yBackground = 2;
        float yForeground = 20;

        int backgroundCount = 0;
        Transform child;

        private void Awake()
        {
            // Store initial euler angels
            //eulerAngles = transform.eulerAngles;
            eulerAngles = root.eulerAngles;

            // Move to background
            MoveToForeground();
        }

        // Start is called before the first frame update
        void Start()
        {
            child = root.GetChild(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {

            // Reset angles
            root.eulerAngles = eulerAngles;

            // Adjust forward rotation

            root.RotateAround(root.position, child.up, transform.eulerAngles.y);
            
        }

        // Used to move mesh to the foreground
        public void MoveToForeground()
        {
            backgroundCount = Mathf.Max(backgroundCount - 1, 0);
            if (backgroundCount > 0)
                return;

            Vector3 pos = root.position;
            pos.y = yForeground;
            root.position = pos;
        }

        // Used to move mesh to the background
        public void MoveToBackground()
        {
            Vector3 pos = root.position;
            pos.y = yBackground;
            root.position = pos;

            backgroundCount++;
        }
    }

}
