using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestQuest : MonoBehaviour
{

    [SerializeField]
    GameObject task1;

    [SerializeField]
    GameObject task2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            task1.GetComponent<HW.FiniteStateMachine>().Lookup();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            task2.GetComponent<HW.FiniteStateMachine>().Lookup();
    }
}
