using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    //public enum PlayerActionControllerType { Press, Hold, Repeat }

    public class ActionController : MonoBehaviour
    {
        public UnityAction<ActionController> OnActionEnable;
        public UnityAction<ActionController> OnActionDisable;
        public UnityAction<ActionController> OnActionPerformed;
        public UnityAction<ActionController> OnActionStart;
        public UnityAction<ActionController> OnActionStop;

        bool actionEnable = false;
        public bool ActionEnable
        {
            get { return actionEnable; }
        }

        

        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            if (!actionEnable)
                return;

            PerformAction();
        }

        public virtual void EnableAction()
        {
            actionEnable = true;
            OnActionEnable?.Invoke(this);
        }

        public virtual void DisableAction()
        {
            actionEnable = false;
            OnActionDisable?.Invoke(this);
        }

        protected virtual void PerformAction()
        {
            if (PlayerInput.GetActionButtonDown())
                OnActionPerformed?.Invoke(this);
            
        }

    }

}
