using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    /// <summary>
    /// This class control which objects must be deactivated on start depending on a given
    /// finite state machine.
    /// </summary>
    public class ActivationController : MonoBehaviour
    {
        // The finite state machine.
        [SerializeField]
        FiniteStateMachine fsm;

        [SerializeField]
        int state = 0; 

        // The list of object you want to de/activate on the given fsm state.
        [SerializeField]
        List<GameObject> objects;

        // True if you want the objects in the list to be deactivated. Otherwise leave false.
        [SerializeField]
        bool deactivate = false;

        // Start is called before the first frame update
        void Start()
        {
            if(state == fsm.CurrentStateId)
            {
                foreach(GameObject obj in objects)
                {
                    obj.SetActive(!deactivate);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
