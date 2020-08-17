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

        // The max speed the player can reach depending on whether is running or not
        float maxSpeed;
        float sqrMaxSpeed;

        // The actual velocity the player is moving as vector
        Vector2 currentVelocity = Vector2.zero;


        bool running = false; // Is player running ?
        bool aiming = false; // Is player aiming ?
        bool disabled = false; // Is this controller disabled ?

        Rigidbody rb;
        float radius;

        string horizontalAxis = "Horizontal";
        string verticalAxis = "Vertical";
        string sprintAxis = "Run";

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            radius = GetComponent<CapsuleCollider>().radius;
        }

        // Start is called before the first frame update
        void Start()
        {
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            // Get the current rigidbody velocity
            //currentVelocity = new Vector2(rb.velocity.x, rb.velocity.z);

            // Init the target velocity
            Vector2 targetVelocity = Vector2.zero;

            // Check running input axis
            CheckIsRunning();

            // If is not aiming player can move
            if (!aiming)
            {
                // Get input and target velocity
                targetVelocity = ComputeTargetVelocity(GetMovementInput());
            }
            else
            {
                // Stop moving on aiming
                targetVelocity = ComputeTargetVelocity(Vector2.zero);

                // Adjust rotation with aiming
                
            }

            // Check collision with wall
            currentVelocity = targetVelocity;

            

        }

        private void FixedUpdate()
        {
            CheckCollisions();

            // Adjust position
            rb.velocity = new Vector3(currentVelocity.x, 0, currentVelocity.y);
            
            //// Get the current rigidbody velocity
            //currentVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
        }


        // Computes target velocity depending on the input
        Vector2 ComputeTargetVelocity(Vector2 input)
        {
            // The velocity we need or want to reach
            Vector2 targetVelocity = Vector2.zero;

            if (input != Vector2.zero)
            {
                // Normalize input ( this only works for raw axis, 0 or 1 input value ), otherwise we need to clamp
                input.Normalize();

                // We stopped running some frames ago
                if (!running && currentVelocity.sqrMagnitude > sqrMaxSpeed)
                {

                    // Get the current speed
                    float speed = currentVelocity.magnitude;

                    // Decelerate 
                    speed -= deceleration * Time.deltaTime;

                    // Too much???
                    if (speed < maxSpeed)
                        speed = maxSpeed;

                    // Set new target velocity 
                    targetVelocity = input * speed;
                }
                else
                {
                    // Get current speed
                    float speed = currentVelocity.magnitude;

                    // Accelerate
                    speed += input.magnitude * acceleration * Time.deltaTime;

                    // Set new target velocity
                    targetVelocity = input * speed;

                    // Clamp velocity depending on the max speed
                    if (targetVelocity.sqrMagnitude > sqrMaxSpeed)
                    {
                        targetVelocity = targetVelocity.normalized * maxSpeed;
                    }
                }
            }
            else // No input at all, decelerate
            {
                float speed = currentVelocity.magnitude - deceleration * Time.deltaTime;
                if (speed < 0)
                    speed = 0;

                targetVelocity = currentVelocity.normalized * speed;
            }

            return targetVelocity;
        }

        
        void CheckCollisions()
        {
            RaycastHit info;
            if (Physics.Raycast(rb.position, currentVelocity.x > 0 ? Vector3.right : Vector3.left, out info, currentVelocity.x * Time.fixedDeltaTime + radius))
                currentVelocity.x = 0;

            if (Physics.Raycast(rb.position, currentVelocity.y > 0? Vector3.forward : Vector3.back, out info, currentVelocity.y * Time.fixedDeltaTime + radius))
                currentVelocity.y = 0;
        }

        // Check running input
        void CheckIsRunning()
        {
            // Get input
            bool axis = GetAxisRaw(sprintAxis);

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
            sqrMaxSpeed = maxSpeed * maxSpeed;
        }

        // Gets horizontal and vertical input axis as 2d vector
        Vector2 GetMovementInput()
        {
            // You can't move
            if (disabled)
                return Vector2.zero;

            // No suffix for mouse and keyboard
            string suffix = "";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            

            // Get input
            return new Vector2(Input.GetAxisRaw(horizontalAxis + suffix), Input.GetAxisRaw(verticalAxis + suffix));
        }

        // Returns true if axis raw is higher than 0, otherwise false
        bool GetAxisRaw(string axis)
        {
            // No suffix for mouse and keyboard
            string suffix = "";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetAxisRaw(axis + suffix) == 1 ? true : false;
        }

        private void Reset()
        {
            running = false;
            aiming = false;
            currentVelocity = Vector2.zero;
          
            ResetMaxSpeed();
        }
    }
}

