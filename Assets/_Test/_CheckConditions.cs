using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:Assets/_TestModules/_TestFiniteStateMachine.cs
using HW.FSM;


public class _TestFiniteStateMachine : MonoBehaviour
{
    
    [SerializeField]
    FiniteStateMachine fsm;

=======
public class _CheckConditions : MonoBehaviour
{
>>>>>>> parent of ed4f18aa (bug fixing AIM target):Assets/_Test/_CheckConditions.cs
    // Start is called before the first frame update
    void Start()
    {
        
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
            fsm.Lookup("Open");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            fsm.Lookup("Lock");
        }
        
    }
}
