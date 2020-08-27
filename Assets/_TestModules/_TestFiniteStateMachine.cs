using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW;



public class _TestFiniteStateMachine : MonoBehaviour
{
    
    [SerializeField]
    DoorFSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fsm.Lookup(DoorAction.Unlock);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fsm.Lookup(DoorAction.Lock);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            fsm.Lookup(DoorAction.Open);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            fsm.Lookup(DoorAction.Close);
        }
    }
}
