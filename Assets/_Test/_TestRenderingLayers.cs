using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestRenderingLayers : MonoBehaviour
{
    [SerializeField]
    HW.MeshController meshController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            meshController.MoveToBackground();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            meshController.MoveToForeground();
        }
    }
}
