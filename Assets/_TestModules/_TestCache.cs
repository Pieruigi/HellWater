using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.CachingSystem;

public class _TestCache : MonoBehaviour
{


    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Cache:" + )
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CacheManager.Instance.Save();
        }
    }
}
