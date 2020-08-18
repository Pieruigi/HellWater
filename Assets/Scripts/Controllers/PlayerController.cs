using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float maxWalkingSpeed;

        [SerializeField]
        float maxRunningSpeed;

        [SerializeField]
        float acceleration;

        [SerializeField]
        float deceleration;

        [SerializeField]
        float angularSpeed;
        float angularSpeedInRadians;

        // The max speed the player can reach depending on whether is running or not
        float maxSpeed;
       
        // The target velocity
        Vector3 targetVelocity;

        bool running = false; // Is player running ?
        bool disabled = false; // Is this controller disabled ?
        bool aiming = false;

        Rigidbody rb;
        
        string horizontalAxis = "Horizontal";
        string verticalAxis = "Vertical";
        string sprintAxis = "Run";

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            angularSpeedInRadians = angularSpeed * Mathf.Deg2Rad;
        }

        // Start is called before the first frame update
        void Start()
        {
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            // You can't move
            if (disabled)
                return;

            //
            // Check movement
            //

            // Check if player is running
            CheckIsRunning();

            // Get player movement input 
            Vector2 input = new Vector2(GetAxisRaw(horizontalAxis), GetAxisRaw(verticalAxis)).normalized; 
            
            // Set the velocity we want or need to reach
            targetVelocity = new Vector3(input.x, 0, input.y) * maxSpeed;

            // 
            // Adjust look at
            //

            // Get the target direction the player must look at
            Vector3 targetFwd = targetVelocity.normalized;

            // Get the current direction player is looking at
            Vector3 currentFwd = transform.forward;
            if (targetVelocity == Vector3.zero)
                targetFwd = currentFwd;
           
            // Lerp rotation
            transform.forward = Vector3.RotateTowards(currentFwd, targetFwd, angularSpeedInRadians * Time.deltaTime, 0);
        }

        private void FixedUpdate()
        {

            // Get starting and target speeds
            float currentSpeed = rb.velocity.magnitude;
            float targetSpeed = targetVelocity.magnitude;

            // Compute acceleration/deceleration
            float speedChange = ((currentSpeed < targetSpeed) ? acceleration : deceleration) * Time.fixedDeltaTime;

            // Interpolate speed
            float speed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChange);

            // If the target velocity is zero we must keep the old direction to avoid the player to stop instantly
            Vector3 direction = targetVelocity.normalized;
            if (targetSpeed == 0)
                direction = rb.velocity.normalized;

            // Update rigidbody velocity
            rb.velocity = direction * speed;
        }

        public void SetDisabled(bool value)
        {
            disabled = value;

            if(value)
                Reset();
        }
        

        // Check running input
        void CheckIsRunning()
        {
            // Get input
            bool axis = GetAxisRaw(sprintAxis) > 0 ? true : false;

            // Run
            if(axis)
            {
                if (!running) // Let's start running
                {
                    running = true;
                    ResetMaxSpeed();
                }
            }
            else
            {
                if (running) // Stop running
                {
                    running = false;
                    ResetMaxSpeed();
                }
            }
        }

        // Max speed will change depending on whether the player is running or not
        void ResetMaxSpeed()
        {
            maxSpeed = running ? maxRunningSpeed : maxWalkingSpeed;
        }

        // Returns true if axis raw is higher than 0, otherwise false
        float GetAxisRaw(string axis)
        {
            // No suffix for mouse and keyboard
            string suffix = "";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetAxisRaw(axis + suffix);
        }

        private void Reset()
        {
            running = false;
            aiming = false;
            targetVelocity = Vector2.zero;
           
            ResetMaxSpeed();
        }
    }
}

