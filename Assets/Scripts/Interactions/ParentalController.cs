using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class ParentalController : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        List<Transform> parents; 

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();

            fsm.OnStateChange += HandleOnStateChange;

            if(fsm.CurrentStateId >= 0)
                target.parent = parents[fsm.CurrentStateId];
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
       
        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            target.parent = parents[fsm.CurrentStateId];
        }
    }

}
