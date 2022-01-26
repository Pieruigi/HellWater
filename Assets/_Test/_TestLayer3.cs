using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestLayer3 : MonoBehaviour
{
    [SerializeField]
    Renderer rend;

    //int sortingLayerId = 2;
    string sortingLayerName = "Default";

    private void Awake()
    {
        rend.sortingLayerName = sortingLayerName;
        Debug.Log("sLayerName:" + rend.sortingLayerName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rend.sortingOrder -= 10;
            if (rend.sortingOrder < 0)
                rend.sortingOrder = 0;

            Debug.Log("sLayerName:" + rend.sortingLayerName);
            Debug.Log("SortingOrder:" + rend.sortingOrder);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rend.sortingOrder += 10;
            if (rend.sortingOrder > 100)
                rend.sortingOrder = 100;

            Debug.Log("sLayerName:" + rend.sortingLayerName);
            Debug.Log("SortingOrder:" + rend.sortingOrder);
        }
    }
}
