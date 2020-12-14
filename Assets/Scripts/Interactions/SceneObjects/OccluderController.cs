using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class OccluderController : MonoBehaviour
    {
        public enum State { Visible = 0, Hidden = 1 }

        FiniteStateMachine fsm;

        Renderer[] renderers;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get all children renderers
            renderers = GetComponentsInChildren<Renderer>();

            // If invisible hide all renderers
            if (fsm.CurrentStateId == (int)State.Hidden)
                ShowRenderers(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // This should not be happening
            if (fsm.CurrentStateId == fsm.PreviousStateId)
                return;

            // Show...
            if (fsm.CurrentStateId == (int)State.Visible)
                ShowRenderers(true);
            else //... or hide
                ShowRenderers(false);
        }

        void ShowRenderers(bool value)
        {
            foreach (Renderer r in renderers)
                r.enabled = value;
        }
    }

}
