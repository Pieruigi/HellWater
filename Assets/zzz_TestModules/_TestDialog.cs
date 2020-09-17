using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestDialog : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        //HW.UI.DialogViewer.Instance.ShowSpeech("Aaa fs df s fsdfs fsdfsd", null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(Test());

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    GetComponent<HW.Cinema.DialogController>().StopDialog();
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);
        HW.UI.DialogViewer.Instance.ShowSpeech("Aaa fs df s fsdfs fsdfsd", null);

        yield return new WaitForSeconds(3f);
        HW.UI.DialogViewer.Instance.Hide();
    }
}
