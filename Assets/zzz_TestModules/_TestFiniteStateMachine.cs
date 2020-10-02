using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW;




public class _TestFiniteStateMachine : MonoBehaviour
{
    
    [SerializeField]
    FiniteStateMachine fsm;

    [SerializeField]
    int fromStateId;

    [SerializeField]
    int toStateId;

    [SerializeField]
    bool callEvent;



    // Start is called before the first frame update
    void Start()
    {
        fsm.ForceState(fromStateId, true, false);    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fsm.Lookup();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fsm.ForceState(toStateId, callEvent, false);
        }
        
        
    }
}
