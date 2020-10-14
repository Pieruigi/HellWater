using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public enum ActionType { None, PickUp, MakeTheBed, TurnHandle, PickUpFromGround, PutFuel, FillTeapot, UseHammer, Search }

    // Manages character behaviour ( such as animations, object to hold, ecc ) when you perform some actions: for example
    // starts picking animation when you pick up something.
    // We can also put some tool in the player hands.
    // Action to perform depend on the state of the object we are interacting to.
    public class ActionAnimator : MonoBehaviour
    {
        //[SerializeField]
        //[Tooltip("The state you want the fsm to be in order to activate this action; leave -1 to set all states.")]
        //int desiredState = -1;

        [SerializeField]
        ActionType actionType = ActionType.None;

        [SerializeField]
        bool disablePlayerOnActionPerformed = false;

        [SerializeField]
        GameObject toolPrefab = null;
       
        [SerializeField]
        //Transform toolParentNode = null;
        HumanoidNodeName playerNodeName = HumanoidNodeName.None;

        [SerializeField]
        Vector3 toolPosition;

        [SerializeField]
        Vector3 toolRotation;

        [SerializeField]
        Transform target; // If you want the player to stay in a given position
       

        Animator animator;

        ActionController actionController;

        FiniteStateMachine fsm;

        string paramActionId = "ActionId";
        string paramDoAction = "DoAction";
        string paramActing = "Acting";
        string paramActionPerformed = "ActionPerformed";

        bool move = false;
        float movingSpeed = 2f;
        float angularSpeed = 360f;
        Rigidbody rb;

        System.DateTime lastMove;

        GameObject tool;

        Transform playerNode;

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

            playerNode = PlayerController.Instance.GetComponent<HumanoidNodeCollection>().GetNode(playerNodeName);
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!move)
                return;

            Vector3 newPos = target.position;
            //Transform player = PlayerController.Instance.transform;
            newPos.y = PlayerController.Instance.transform.position.y;
            
            rb.position = Vector3.MoveTowards(rb.position, newPos, Time.fixedDeltaTime * movingSpeed);
            //rb.MovePosition(target.position);
            PlayerController.Instance.transform.forward = Vector3.MoveTowards(PlayerController.Instance.transform.forward, target.forward, angularSpeed * Time.deltaTime);

            if ((System.DateTime.UtcNow - lastMove).TotalSeconds > 0.5f)
                move = false;
        }

     

        // Also called when holding and repeating
        void HandleOnActionStart(ActionController ctrl)
        {


            //if (state >= 0 && fsm.CurrentStateId != state)
            //    return;

            PlayerController.Instance.SetDisabled(true);

            // Create tool if needed
            if (toolPrefab)
            {
                tool = GeneralUtility.ObjectPopIn(toolPrefab, playerNode, toolPosition, toolRotation, Vector3.one);
            }

            // Reset the param action performed to avoid keeping an old value
            animator.SetBool(paramActionPerformed, false);

            if (actionType != ActionType.None)
            {
                // We don't use trigger but only action id
                animator.SetInteger(paramActionId, (int)actionType);
                animator.SetBool(paramActing, true);
            }
            

            // Move player to the desired position
            if (target)
            {
                move = true;
                lastMove = System.DateTime.UtcNow;
            }
                
        }

        // Also called when holding and repeating
        void HandleOnActionStop(ActionController ctrl)
        {
    
            //if (state >= 0 && fsm.CurrentStateId != state)
            //    return;

            PlayerController.Instance.SetDisabled(false);

            // Remove tool if needed
            if (tool)
                GeneralUtility.ObjectPopOut(tool);

            // We don't use trigger but only action id
            if (actionType != ActionType.None)
                animator.SetBool(paramActing, false);

            // Move player to the desired position
            if (target)
            {
                move = false;
            }
                
        }

        // Also used on simple action controller
        void HandleOnActionPerformed(ActionController ctrl)
        {

            //if (state >= 0 && fsm.CurrentStateId != state)
            //    return;

            // We only use this on simple action controller. For holding and repeating check actionStart and actionStop.
            if (actionController.GetType() == typeof(ActionController))
            {
                // Set disable, animation must send an ActionCompleted event in order to enable the player again
                if (disablePlayerOnActionPerformed)
                    PlayerController.Instance.SetDisabled(true);

                // Start animation
                if (actionType != ActionType.None)
                {
                    animator.SetInteger(paramActionId, (int)actionType);
                    animator.SetTrigger(paramDoAction);
                }

                // Move player to the desired position
                if (target)
                {
                    move = true;
                    lastMove = System.DateTime.UtcNow;
                }
            }
            else
            {
                // Set disable, animation must send an ActionCompleted event in order to enable the player again
                if(disablePlayerOnActionPerformed)
                    PlayerController.Instance.SetDisabled(true);

                // Start animation
                if (actionType != ActionType.None)
                {
                    animator.SetBool(paramActionPerformed, true);
                    
                }
            }
        }
    }

}
