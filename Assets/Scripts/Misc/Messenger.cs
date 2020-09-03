using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class Messenger : MonoBehaviour
    {
        private void Awake()
        {
            FiniteStateMachine fsm = GetComponent<FiniteStateMachine>();
            if (fsm)
                fsm.OnFail += HandleOnFail;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnFail(FiniteStateMachine fsm, int errorId)
        {

        }

        void ShowMessage(int messageId)
        {

        }
    }
}

