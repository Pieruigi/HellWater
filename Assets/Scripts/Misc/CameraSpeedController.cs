using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpeedController : MonoBehaviour
{
    [SerializeField]
    private Transform camera3DPivot;

    // Last z position
    float lastZ = 0;

    // Start is called before the first frame update
    void Awake()
    {
        lastZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void LateUpdate()
    //{
    //    float diff = (transform.position.z - lastZ) * ( 1 - Mathf.Cos(Mathf.Deg2Rad * camera3DPivot.eulerAngles.x));
    //    //diff = diff - diff * Mathf.Cos(Mathf.Deg2Rad * 40f);
    //    transform.position -= Vector3.forward * diff;
    //    lastZ = transform.position.z;
    //}

    private void LateUpdate()
    {
        float diff =  camera3DPivot.position.z - lastZ;
        if(diff != 0)
        {
            transform.position += Vector3.forward * diff * (1f-.766f);
        }
        
        lastZ = camera3DPivot.position.z;
    }
}
