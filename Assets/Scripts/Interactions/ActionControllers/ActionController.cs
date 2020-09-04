using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    //public enum PlayerActionControllerType { Press, Hold, Repeat }

    public class ActionController : MonoBehaviour
    {
        public UnityAction<ActionController> OnStartActing;
        public UnityAction<ActionController> OnStopActing;
        public UnityAction<ActionController> OnActionPerformed;

        bool acting = false;
        public bool Acting
        {
            get { return acting; }
        }

        

        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            if (!acting)
                return;

            if (PerformAction())
                OnActionPerformed?.Invoke(this);
            
        }

        public virtual void StartActing()
        {
            acting = true;
            OnStartActing?.Invoke(this);
        }

        public virtual void StopActing()
        {
            acting = false;
            OnStopActing?.Invoke(this);
        }

        protected virtual bool PerformAction()
        {
            if (PlayerController.Instance.GetActionButtonDown())
                return true;

            return false;
        }
    }

}
