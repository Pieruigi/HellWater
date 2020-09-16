using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum ActionType { None, PickUp, MakeTheBed }

    // Manages character behaviour ( such as animations, object to hold, ecc ) when you perform some actions: for example
    // starts picking animation when you pick up something.
    // We can also put some tool in the player hands.
    // Action to perform depend on the state of the object we are interacting to.
    public class ActionAnimator : MonoBehaviour
    {
        [SerializeField]
        ActionType actionType = ActionType.None;

        [SerializeField]
        GameObject tool = null;

        [SerializeField]
        Transform target; // If you want the player to stay in a given position

        [SerializeField]
        int state = -1; // The state in which we want this operation to be performed ( -1 means any state )
               

        Animator animator;

        ActionController actionController;

        FiniteStateMachine fsm;

        string paramActionId = "ActionId";
        string paramDoAction = "DoAction";
        string paramActing = "Acting";

        bool move = false;
        float movingSpeed = 10f;
        float angularSpeed = 360f;
        Rigidbody rb;

        private void Awake()
        {
            actionController = GetComponentInParent<ActionController>();

            actionController.OnActionPerformed += HandleOnActionPerformed;
            actionController.OnActionStart += HandleOnActionStart;
            actionController.OnActionStop += HandleOnActionStop;

            fsm = GetComponentInParent<FiniteStateMachine>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get player animator 
            animator = PlayerController.Instance.GetComponent<Animator>();
            rb = PlayerController.Instance.GetComponent<Rigidbody>();
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!move)
                return;

            Vector3 newPos = target.position;
            Transform player = PlayerController.Instance.transform;
            newPos.y = player.position.y;
            
            rb.position = Vector3.MoveTowards(rb.position, newPos, Time.fixedDeltaTime * movingSpeed);
            //rb.MovePosition(target.position);
            player.forward = Vector3.MoveTowards(player.forward, target.forward, angularSpeed * Time.deltaTime);
        }

        // Also called when holding and repeating
        void HandleOnActionStart(ActionController ctrl)
        {
            if (state >= 0 && fsm.CurrentStateId != state)
                return;

            if (actionType == ActionType.None)
                return;


            PlayerController.Instance.SetDisabled(true);

            // We don't use trigger but only action id
            animator.SetInteger(paramActionId, (int)actionType);
            animator.SetBool(paramActing, true);

            // Move player to the desired position
            if (target)
            {
                //rb.isKinematic = true;
                move = true;
            }
                
        }

        // Also called when holding and repeating
        void HandleOnActionStop(ActionController ctrl)
        {
            if (state >= 0 && fsm.CurrentStateId != state)
                return;

            if (actionType == ActionType.None)
                return;

            PlayerController.Instance.SetDisabled(false);

            // We don't use trigger but only action id
            //animator.SetInteger(paramActionId, (int)actionType);
            animator.SetBool(paramActing, false);

            // Move player to the desired position
            if (target)
            {
                //rb.isKinematic = false;
                move = false;
            }
                
        }

        // Also used on simple action controller
        void HandleOnActionPerformed(ActionController ctrl)
        {
            if (state >= 0 && fsm.CurrentStateId != state)
                return;

            if (actionType == ActionType.None)
                return;

            // We only use this on simple action controller. For holding and repeating check actionStart and actionStop.
            if(actionController.GetType() == typeof(ActionController))
            {
                // Set disable, animation must send an ActionCompleted event in order to enable the player again
                PlayerController.Instance.SetDisabled(true);
                
                // Start animation
                animator.SetInteger(paramActionId, (int)actionType);
                animator.SetTrigger(paramDoAction);
            }
        }
    }

}
