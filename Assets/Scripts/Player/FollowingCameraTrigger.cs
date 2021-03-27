using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class FollowingCameraTrigger : MonoBehaviour
    {
        FollowingCamera fc;

        // Start is called before the first frame update
        void Start()
        {
            fc = FollowingCamera.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform == fc.Target)
            {
                fc.GetComponent<FollowingCameraData>().SetData(GetComponent<FollowingCameraData>().GetData());
            }
        }

        
    }

}
