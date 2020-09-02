using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class Camera2DGroupAligner : MonoBehaviour
    {
        [SerializeField]
        Transform camera3DGroup;

        // Start is called before the first frame update
        void Start()
        {
            Vector3 pos = new Vector3(camera3DGroup.position.x, camera3DGroup.position.z, 0);
            transform.position = pos;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
