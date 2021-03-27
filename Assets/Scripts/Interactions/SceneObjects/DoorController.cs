using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public enum DoorState { Locked, Unlocked }

    public abstract class DoorController : MonoBehaviour
    {
        public UnityAction<DoorController> OnOpen;
        public UnityAction<DoorController> OnClose;
        public UnityAction<DoorController> OnStillLocked;
        public UnityAction<DoorController> OnUnlock;

        //[SerializeField]
        //GameObject sceneObject;

        //[SerializeField]
        //bool invertAngle = false;

        //[SerializeField]
        //float speed = 0.5f;

        //InteractionController interactionController;
        FiniteStateMachine fsm;

        //float time;

        //float openAngle = 0;

        bool opened = false;
        public bool Opened
        {
            get { return opened; }
        }

        protected abstract void Open();
        protected abstract void Close();
        //protected abstract void Unlock();

        protected virtual void Awake()
        {
            //interactionController = GetComponent<InteractionController>();
            //interactionController.OnStateChange += HandleOnStateChange;
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
            fsm.OnFail += HandleOnFail;

            //time = 1f / speed;
         
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        void HandleOnFail(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == (int)DoorState.Locked)
                OnStillLocked?.Invoke(this);
        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            int newState = fsm.CurrentStateId;
            // You are trying to open a locked door
            if (fsm.PreviousStateId == newState && newState == (int)DoorState.Locked)
            {
                OnStillLocked?.Invoke(this);
                return;
            }

            // You unlocked the door
            if (fsm.PreviousStateId == (int)DoorState.Locked && newState == (int)DoorState.Locked)
            {
                OnUnlock?.Invoke(this);
                return;
            }

            // You can open it
            if (newState == (int)DoorState.Unlocked)
            {

                if (!opened)
                {
                    opened = true;
                    Open();
                    OnOpen?.Invoke(this);
                }
                else
                {
                    opened = false;
                    Close();
                    OnClose?.Invoke(this);
                }

            }

        }

      
       


    }

}
