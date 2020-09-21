﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PickUpController : MonoBehaviour
    {
        [SerializeField]
        GameObject target;

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
            // Hide
            LeanTween.scale(target, Vector3.zero, time).setEaseInOutBounce();

            yield return new WaitForSeconds(time * 1.1f);

            target.SetActive(false);
        }
    }
}