using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HW
{
    public class InGameTutorialController : MonoBehaviour, IInteractable
    {
        public UnityAction<string> OnShow;

        [SerializeField]
        string tutorialCode;

        FiniteStateMachine fsm;
        int stateDisabled = -1;
        
        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Interact()
        {
            fsm.Lookup();
        }

        public bool IsAvailable()
        {
            return fsm.CurrentStateId != stateDisabled;
        }


        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // Since the new state is -1 we are always sure to come from the enable state.
            OnShow?.Invoke(tutorialCode);
        }
    }

}
