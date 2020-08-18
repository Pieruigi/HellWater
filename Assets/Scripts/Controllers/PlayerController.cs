using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;

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

        #region LOCOMOTION
        // The max speed the player can reach depending on whether is running or not
        float maxSpeed;

        // Is player running ?
        bool running = false; 

        // The target velocity
        Vector3 desiredVelocity;
        #endregion

        #region FIGHTING
        bool aiming = false;

        [SerializeField]
        FireWeapon fireWeapon;

        //Transform desiredTarget;
        Transform currentTarget;
        #endregion

        #region AXIS
        string horizontalAxis = "Horizontal";
        string verticalAxis = "Vertical";
        string sprintAxis = "Run";
        string aimAxis = "Aim";
        #endregion

        Rigidbody rb;
        bool disabled = false; // Is this controller disabled ?
        float sphereCastRadius = 0.5f;

        #region NATIVE
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

            // Check if player is aiming
            CheckIsAiming();

            if (!aiming)
            {
                //
                // Check movement
                //

                // Check if player is running
                CheckIsRunning();

                // Get player movement input 
                Vector2 input = new Vector2(GetAxisRaw(horizontalAxis), GetAxisRaw(verticalAxis)).normalized;

                // Set the velocity we want or need to reach
                desiredVelocity = new Vector3(input.x, 0, input.y) * maxSpeed;

                // 
                // Adjust look at
                //

                // Get the target direction the player must look at
                Vector3 desiredFwd = desiredVelocity.normalized;

                // Get the current direction player is looking at
                Vector3 currentFwd = transform.forward;
                if (desiredVelocity == Vector3.zero)
                    desiredFwd = currentFwd;

                // Lerp rotation
                transform.forward = Vector3.RotateTowards(currentFwd, desiredFwd, angularSpeedInRadians * Time.deltaTime, 0);
            }
            else // Is aiming
            {
                // Get targets inside the weapon range which are not hidden by any obstacle
                List<Transform> targets = GetAvailableTargets(fireWeapon.Range);

                // Clear current target if not longer available
                if (currentTarget && !targets.Contains(currentTarget))
                    currentTarget = null;


                // If there no target yet then set the closest one if available
                if (!currentTarget)
                {
                    currentTarget = GetClosestTarget(targets);
                }
                else
                {
                    // Get the aiming direction
                    Vector3 direction = new Vector3(GetAxisRaw(horizontalAxis), 0, GetAxisRaw(verticalAxis)).normalized;

                    // Get the target the player is aiming or null
                    Transform newTarget = GetNewTarget(targets, direction, fireWeapon.Range);
                    if (newTarget && newTarget != currentTarget)
                        currentTarget = newTarget;
                }
                    

                // If target exists 
                if (currentTarget)
                {
                    // Get the target direction the player must look at
                    Vector3 desiredFwd = (currentTarget.position - transform.position).normalized;

                    // Get the current direction player is looking at
                    Vector3 currentFwd = transform.forward;
                    
                    // Lerp rotation
                    transform.forward = Vector3.RotateTowards(currentFwd, desiredFwd, angularSpeedInRadians * Time.deltaTime, 0);
                }
                

                // Just debug
                DebugTargets(targets);

                Debug.Log("CurrentTarget:" + currentTarget);
            }

           
        }

        void FixedUpdate()
        {

            // Get starting and target speeds
            float currentSpeed = rb.velocity.magnitude;
            float desiredSpeed = desiredVelocity.magnitude;

            // Compute acceleration/deceleration
            float speedChange = ((currentSpeed < desiredSpeed) ? acceleration : deceleration) * Time.fixedDeltaTime;

            // Interpolate speed
            float speed = Mathf.MoveTowards(currentSpeed, desiredSpeed, speedChange);

            // If the target velocity is zero we must keep the old direction to avoid the player to stop instantly
            Vector3 direction = desiredVelocity.normalized;
            if (desiredSpeed == 0)
                direction = rb.velocity.normalized;

            // Update rigidbody velocity
            rb.velocity = direction * speed;
        }
        #endregion

        #region PUBLIC
        public void SetDisabled(bool value)
        {
            disabled = value;

            if(value)
                Reset();
        }
        public void SetAiming(bool value)
        {
            aiming = value;

            if (aiming)
            {
                running = false;
                desiredVelocity = Vector3.zero;
                ResetMaxSpeed();
            }
            else
            {
                currentTarget = null;
            }
        }

        #endregion

        #region PRIVATE
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

        void CheckIsAiming()
        {
            SetAiming(GetAxisRaw(aimAxis) > 0 ? true : false);
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
            string suffix = "_0";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetAxisRaw(axis + suffix);
        }

        // Returns all the available target depending on radius and obstacles
        List<Transform> GetAvailableTargets(float radius)
        {
            List<Transform> ret = new List<Transform>();

            // Overlap a sphere with the origin in the player position
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            // Loop through the overlapped colliders
            foreach(Collider collider in colliders)
            {
                // Get ITargetable interface
                ITargetable targetable = collider.GetComponent<ITargetable>();

                // If not targetable then continue
                if (targetable == null)
                    continue;

                // Get vector heading from player to target
                Vector3 direction = (targetable as MonoBehaviour).transform.position - transform.position;
                
                // get target collider
                Collider targetColl = (targetable as MonoBehaviour).GetComponent<Collider>();

                // Disable target collider if exists
                if (targetColl)
                    targetColl.enabled = false;

                // If there is no obstacle between player and target then add to the returning list
                if (!Physics.Raycast(transform.position, direction.normalized, direction.magnitude))
                    ret.Add((targetable as MonoBehaviour).transform);

                // Enable target collider if exists
                if (targetColl)
                    targetColl.enabled = true;

            }

            return ret;
        }

        Transform GetClosestTarget(List<Transform> targets)
        {
            float sqrMinDist = 0;
            Transform ret = null;

            foreach(Transform target in targets)
            {
                float sqrDist = (transform.position - target.position).sqrMagnitude;
                if(ret == null || sqrDist < sqrMinDist)
                {
                    ret = target;
                    sqrMinDist = sqrDist;
                }
            }

            return ret;
        }

        Transform GetNewTarget(List<Transform> targets, Vector3 aimingDirection, float distance)
        {
            Transform ret = null;

            RaycastHit hitInfo;
            
            if (Physics.SphereCast (transform.position, sphereCastRadius, aimingDirection, out hitInfo, distance))
                ret = targets.Find(t => t == hitInfo.transform);

            return ret;
        }

        void Reset()
        {
            running = false;
            aiming = false;
            desiredVelocity = Vector2.zero;
            
            currentTarget = null;
           
            ResetMaxSpeed();
        }
        #endregion

        void DebugTargets(List<Transform> targets)
        {
            string s = "";
            foreach (Transform target in targets)
            {
                s += target.gameObject.name + " ";
                
            }
            Debug.Log("Targets:" + s);
        }
    }
}

