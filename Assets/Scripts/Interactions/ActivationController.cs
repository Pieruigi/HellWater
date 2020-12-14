using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// This class control which objects must be de/activated on start depending on a given
    /// finite state machine. All the objects in the object list will be activated if the 
    /// current fsm state is contained in the state list, otherwise they will be deactivated.
    /// </summary>
    public class ActivationController : MonoBehaviour
    {
        // The finite state machine.
        [SerializeField]
        FiniteStateMachine fsm;

        [SerializeField]
        List<int> states = new List<int>();

        // False if you want to check only on start, otherwise true.
        [SerializeField]
        bool onStateChanged = false;

        // The list of object you want to activate when states match and deactivate when 
        // states don't match.
        [SerializeField]
        List<GameObject> positiveObjects;

        // The list of object you want to deactivate when states match and activate when
        // states don't match.
        [SerializeField]
        List<GameObject> negativeObjects;

        private void Awake()
        {
            if (onStateChanged)
                fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Return true if the current state of the active state machine is in the state list.
            bool activate = states.Exists(s => s == fsm.CurrentStateId);

            // De/activate positive objects in the list.
            foreach (GameObject obj in positiveObjects)
            {
                obj.SetActive(activate);
            }

            // De/activate negative objects in the list.
            foreach (GameObject obj in negativeObjects)
            {
                obj.SetActive(!activate);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            Check();
        }

        void Check()
        {
            // Return true if the current state of the active state machine is in the state list.
            bool activate = states.Exists(s => s == fsm.CurrentStateId);

            // De/activate positive objects in the list.
            foreach (GameObject obj in positiveObjects)
            {
                obj.SetActive(activate);
            }

            // De/activate negative objects in the list.
            foreach (GameObject obj in negativeObjects)
            {
                obj.SetActive(!activate);
            }
        }
    }

}
