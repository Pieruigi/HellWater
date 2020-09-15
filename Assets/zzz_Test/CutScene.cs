using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

namespace HW.Cinema
{
    public abstract class CutScene : MonoBehaviour, ISkippable
    {
        [SerializeField]
        bool executeOnEnter = false; // True if you want this cs to execute after the scene has been loaded

        DialogController dialogController; 

        FiniteStateMachine fsm;
        CinemaController cinemaController;

        bool skippable = false;

        protected virtual void Awake()
        {
            // Get the finite state machine
            fsm = GetComponent<FiniteStateMachine>();

            // Try to get the dialog controller if exists ( otherwise the cut scene will show no dialog )
            dialogController = GetComponent<DialogController>();

            // Get cinema controller and set handles
            cinemaController = GetComponent<CinemaController>();
            cinemaController.OnFadeOutOnEnterComplete += HandleOnFadeOutOnEnterComplete;
            cinemaController.OnFadeOutOnExitComplete += HandleOnFadeOutOnExitComplete;
            cinemaController.OnEnter += HandleOnEnter;
            cinemaController.OnExit += HandleOnExit;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // If execute is set to auto and cs has not been executed then execute it
            if (executeOnEnter && fsm.CurrentStateId == (int)CutSceneState.Ready)
            {
                cinemaController.Enter();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool CanBeSkipped()
        {
            return skippable;
        }

        public virtual void Skip()
        {
            if (!skippable)
                return;

            //skippable = false;
            //StopAllCoroutines();
            //cinemaController.Exit();
            Exit();
        }

        public virtual void Exit()
        {
            Debug.Log("Exit");
            skippable = false;
            StopAllCoroutines();
            cinemaController.Exit();
        }

        protected virtual void HandleOnFadeOutOnEnterComplete(CinemaController ctrl)
        {
            if (dialogController)
                dialogController.StartDialog();
        }

        protected virtual void HandleOnFadeOutOnExitComplete(CinemaController ctrl)
        {
            if (dialogController)
                dialogController.StopDialog();
        }

        protected virtual void HandleOnEnter(CinemaController ctrl)
        {
            // Set skippable
            skippable = true;

        }

        protected virtual void HandleOnExit(CinemaController ctrl)
        {

            // Update finit state machine 
            fsm.Lookup();
        }
    }

}
