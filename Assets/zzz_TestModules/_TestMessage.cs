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
            ShowMessage(HW.MessageFactory.Instance.GetMessage(0));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ShowMessage(HW.MessageFactory.Instance.GetMessage(1));
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ShowMessage(HW.MessageFactory.Instance.GetMessage(2));
    }

    void ShowMessage(string message)
    {
        HW.UI.MessageViewer.Instance.ShowMessage(message);
    }
}
