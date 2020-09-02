using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpeedController : MonoBehaviour
{
    [SerializeField]
    private Transform camera3DPivot;

    [SerializeField]
    private bool rotate;

    // Last z position
    float lastCamZ = 0;
    float lastCamX = 0;

    float cosDisp;

    // Start is called before the first frame update
    void Awake()
    {
        if (rotate)
            transform.eulerAngles = new Vector3(0, 0, 0);

        lastCamZ = camera3DPivot.position.z;
        lastCamX = camera3DPivot.position.x;
        //cosDisp = 1f - Mathf.Cos(Mathf.Deg2Rad * camera3DPivot.eulerAngles.x);
        cosDisp = Mathf.Cos(Mathf.Deg2Rad * camera3DPivot.eulerAngles.x);
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
        float diff =  camera3DPivot.position.z - lastCamZ;

        //if(diff != 0)
        //    transform.position += Vector3.forward * diff * cosDisp;
        if (diff != 0)
            transform.position -= Vector3.up * diff * cosDisp;

        diff = camera3DPivot.position.x - lastCamX;

        if (diff != 0)
            transform.position -= Vector3.right * diff;

        lastCamZ = camera3DPivot.position.z;
        lastCamX = camera3DPivot.position.x;
    }
}
