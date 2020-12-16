using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FollowingCameraData : MonoBehaviour
    {
        public class Data
        {
            public Vector3 Offset { get; }
            public float SmoothTime { get; }
            public bool LookAt { get; }
            public Transform ForwardTarget { get; }

            public Data(Vector3 offset, float smoothTime, bool lookAt, Transform forwardTarget)
            {
                Offset = offset;
                SmoothTime = smoothTime;
                LookAt = lookAt;
                ForwardTarget = forwardTarget;
            }

        }

      
        [SerializeField]
        Vector3 offset;
        public Vector3 Offset
        {
            get { return offset; }
            //set { offset = value; }
        }

        [SerializeField]
        float smoothTime = 2f;
        public float SmoothTime
        {
            get { return smoothTime; }
            //set { smoothTime = value; }
        }

        [SerializeField]
        bool lookAt = false;
        public bool LookAt
        {
            get { return lookAt; }
            //set { lookAt = value; }
        }

       
        [SerializeField]
        Transform forwardTarget;
        public Transform ForwardTarget
        {
            get { return forwardTarget; }
        }

        
        public void SetData(Data data)
        {
            this.offset = data.Offset;
            this.smoothTime = data.SmoothTime;
            this.lookAt = data.LookAt;
            this.forwardTarget = data.ForwardTarget;
        }

        public Data GetData()
        {
            return new Data(offset, smoothTime, lookAt, forwardTarget);
        }
        
    }

}
