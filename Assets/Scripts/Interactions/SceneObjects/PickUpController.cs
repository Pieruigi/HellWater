using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    public class PickUpController : MonoBehaviour
    {
        [SerializeField]
        Item item;

        [SerializeField]
        int count = 1;

        [SerializeField]
        GameObject target;

        [SerializeField]
        float delay = 0.7f;

        FiniteStateMachine fsm;

        float time = 1f;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {

            // Init the object depending on the finite state machine
            if (fsm.CurrentStateId == (int)PickableState.Picked)
            {
                if(target)
                    target.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (fsm.CurrentStateId == (int)PickableState.Picked)
            {
                StartCoroutine(PickUp());
            }
        }

        IEnumerator PickUp()
        {
            if(delay > 0)
                yield return new WaitForSeconds(delay);

            // Hide
            if (target)
            {
                GeneralUtility.ObjectPopOut(target);
                yield return new WaitForSeconds(time * 1.1f);
            }
                

            //target.SetActive(false);
            if (item)
            {
                //Equipment.Instance.Add(item); // We really need some kind of dipatcher
                //PlayerController.Instance.EquipWeapon(item);
                ItemDispatcher.Dispatch(item, count);
            }
                
        }
    }
}