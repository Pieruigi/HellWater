using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestMessage : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Debug.Log("message:" + HW.MessageFactory.Instance.GetMessage(0));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Debug.Log("message:" + HW.MessageFactory.Instance.GetMessage(1));
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Debug.Log("message:" + HW.MessageFactory.Instance.GetMessage(2));
    }
}
