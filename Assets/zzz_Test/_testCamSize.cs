using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _testCamSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.rect = new Rect(0f, 0f, 1f, 0.766f);
        //camera.pixelRect = new Rect(0, 0, camera.pixelWidth, 1.3f * camera.pixelHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
