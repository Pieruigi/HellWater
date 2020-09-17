using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW.UI;


namespace HW
{
    public class InstantDialogController : DialogController
    {
        //[SerializeField]
        //List<string> dialogCodes; // Each id corresponds to a given fsm state

        FiniteStateMachine fsm;

        protected override void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            base.Awake();
        }


        // Update is called once per frame
        void Update()
        {

        }

        public override void StopDialog()
        {
            base.StopDialog();

            PlayerController.Instance.SetDisabled(false);
        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            
            //// Multi dialog support
            //if(dialogCodes.Count > 0)
            //{
            //    // Get the dialog
            //    SetDialogCode(dialogCodes[oldState]);
            //}
            
            // Start
            LoopDialog();
        }


    }

}
