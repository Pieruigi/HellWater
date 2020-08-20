using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;

namespace HW
{
    public class PlayerController : MonoBehaviour, IHitable
    {
        #region ACTIONS
        public UnityAction OnShoot;
        public UnityAction OnStartAiming;
        public UnityAction OnStopAiming;
        public UnityAction OnChargeAttack;
        public UnityAction<bool> OnAttack;
        public UnityAction OnIsOutOfAmmo;
        public UnityAction OnReload;
        public UnityAction<Weapon> OnSetCurrentWeapon;
        public UnityAction OnResetCurrentWeapon;
        public UnityAction<HitInfo> OnHit;
        #endregion

        #region SERIALIZED FIELDS
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
        #endregion

        #region LOCOMOTION FIELDS
        // The max speed the player can reach depending on whether is running or not
        float maxSpeed;

        // Is player running ?
        bool running = false; 

        // The target velocity
        Vector3 desiredVelocity;
        #endregion

        #region FIGHTING FIELDS
        bool aiming = false;
        bool reloading = false;
        bool shooting = false;
        bool chargingAttack = false; // Charging melee attack
        bool attacking = false; // Performing attack with melee weapon
        bool attackCharged = false;
        bool attackFailed = false;
        bool hit = false;

        // Health 
        Health health;

        float toTargetSignedAngleRotation = 0;
        public float ToTargetSignedAngleRotation
        {
            get { return toTargetSignedAngleRotation; }
        }

        FireWeapon fireWeapon;
        MeleeWeapon meleeWeapon;
        Weapon currentWeapon;
        public Weapon CurrentWeapon
        {
            get { return currentWeapon; }
        }
       
        //Transform desiredTarget;
        Transform currentTarget;
        #endregion

        #region CONTROLLER AXIS 
        string horizontalAxis = "Horizontal";
        string verticalAxis = "Vertical";
        string sprintAxis = "Run";
        string aimAxis = "Aim";
        string reloadAxis = "Reload";
        string shootAxis = "Shoot";
        #endregion

        #region MISC FIELDS
        Rigidbody rb;
        bool disabled = false; // Is this controller disabled ?
        float sphereCastRadius = 0.5f; // Used to cast targets
        bool qteAction = false;
        float angularSpeedInRadians;
        #endregion

        #region NATIVE METHODS
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            angularSpeedInRadians = angularSpeed * Mathf.Deg2Rad;
            health = GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            // You can't do anything else while you are doing one of these actions
            if (disabled || reloading || shooting || attacking || hit)
                return;

            if (qteAction)
                QteAction();
            else
                RealtimeAction();
           
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

        #region INTERFACES IMPLEMENTATION
        public void Hit(HitInfo hitInfo)
        {
            if(hitInfo.PhysicalReaction != HitPhysicalReaction.None)
                hit = true;

            // Apply damage
            health.Damage(hitInfo.DamageAmount);

            // Hit event
            OnHit?.Invoke(hitInfo);
        }
        #endregion

        #region PUBLIC
        public bool IsDead()
        {
            return health.CurrentHealth == 0;
        }

        public float GetCurrentSpeed()
        {
            return rb.velocity.magnitude;
        }

        public float GetMaximumSpeed()
        {
            return maxRunningSpeed;
        }

        public void SetDisabled(bool value)
        {
            disabled = value;

            if(value)
                Reset();
        }
        public void SetAiming(bool value)
        {
            aiming = value;
            toTargetSignedAngleRotation = 0;
            if (aiming)
            {
                running = false;
                desiredVelocity = Vector3.zero;
                ResetMaxSpeed();

                // Show fire weapon
                //ShowFireWeapon();
                SetCurrentWeapon(fireWeapon);

                OnStartAiming?.Invoke();
            }
            else
            {
                currentTarget = null;
               

                // Hide weapon
                //HideWeapon();
                ResetCurrentWeapon();

                OnStopAiming?.Invoke();
            }
        }

        // Equips and holds fire or melee weapon
        public void EquipWeapon(Weapon weapon)
        {
            // Is it a fire weapon ?
            if (weapon.GetType() == typeof(FireWeapon))
            {
                // Switching
                if (fireWeapon && fireWeapon != weapon)
                    fireWeapon.SetVisible(false);

                // No weapon equipped
                if (!fireWeapon)
                    fireWeapon = weapon as FireWeapon;

                // Ok let's see this weapon
                //ShowFireWeapon();
                SetCurrentWeapon(fireWeapon);
            }
            else // Is melee ( we only have bat )
            {
                // Not equipped yet
                if (!meleeWeapon)
                    meleeWeapon = weapon as MeleeWeapon;

                // Show melee
                //ShowMeleeWeapon();
                SetCurrentWeapon(meleeWeapon);
            }
        }

        #endregion

        #region PRIVATE
        void SetCurrentWeapon(Weapon weapon)
        {
            if (currentWeapon != null)
                currentWeapon.SetVisible(false);

            currentWeapon = weapon;
            currentWeapon.SetVisible(true);

            OnSetCurrentWeapon?.Invoke(weapon);
        }

        void ResetCurrentWeapon()
        {
            if (!currentWeapon)
                return;

            currentWeapon.SetVisible(false);
            currentWeapon = null;

            OnResetCurrentWeapon?.Invoke();
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

        void CheckIsAiming()
        {
            // Aiming only works with fire weapons
            if (!fireWeapon)
            {
                SetAiming(false);
                return;
            }
                
            // Check input
            SetAiming(GetAxisRaw(aimAxis) > 0 ? true : false);
        }

       void CheckIsReloading()
        {
            // Reloading only works on fire weapons
            if (!fireWeapon)
                return;

            if (fireWeapon.IsFull())
                return;

            // Check button
            if (GetAxisRaw(reloadAxis) > 0)
            {
                TryReload();
            }
                
        }

        // Max speed will change depending on whether the player is running or not
        void ResetMaxSpeed()
        {
            maxSpeed = running ? maxRunningSpeed : maxWalkingSpeed;
        }

        void TryReload()
        {
            if (!fireWeapon)
                return;

            if(fireWeapon.IsOutOfAmmo())
            {
                OnIsOutOfAmmo?.Invoke();
            
            }
            else
            {
                // Reset all
                Reset();

                // Set reloading
                reloading = true;

                // Event
                OnReload?.Invoke();
            }
            
           
        }

        void TryShoot()
        {
            if (!fireWeapon)
                return;

            // Shoot
            if (fireWeapon.Shoot())
            {
                // Set shooting flag
                shooting = true;

                OnShoot?.Invoke();
            }
                
        }

        void TryChargeAttack()
        {
            if (!meleeWeapon)
                return;

            // Set flags
            chargingAttack = true;
            attackCharged = false;

            // Event
            OnChargeAttack?.Invoke();
        }

        void TryAttack()
        {
           
            if (!meleeWeapon)
                return;

            // Check is attack will succeed
            if (attackCharged)
            {
                Debug.Log("Attack OK");
                // Start attacking if attack succeeds
                attacking = true;
                OnAttack?.Invoke(true);
            }
            else
            {
                Debug.Log("Attack KO");
                attacking = false; // Be sure
                attackFailed = true;
                StartCoroutine(AttackFailedCooldown());
                OnAttack?.Invoke(false);
            }

            // Stop charging
            chargingAttack = false;
            attackCharged = false;

            
        }

        IEnumerator AttackFailedCooldown()
        {
            yield return new WaitForSeconds(0.5f);
            attackFailed = false;
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
            reloading = false;
            shooting = false;
            chargingAttack = false;
            attacking = false;
            attackCharged = false;
            toTargetSignedAngleRotation = 0;
            hit = false;

            desiredVelocity = Vector2.zero;

            currentTarget = null;

            ResetMaxSpeed();
        }

        void RealtimeAction()
        {
            // Melee attack ( we must check button release )
            if (chargingAttack)
            {
                if(GetAxisRaw(shootAxis) == 0)
                {
                    
                    TryAttack();
                }
                // We can't move while charging melee attack
                return;
            }

            // Switch fire weapon
            //CheckIsSwitchingWeapon();

            // Check if the equipped fire weapon must be reloaded
            CheckIsReloading();

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

                // 
                // Melee attack
                //

                // If magazine is not empty then shoot
                if (GetAxisRaw(shootAxis) > 0)
                {
                    if (meleeWeapon && !attackFailed)
                    {
                        desiredVelocity = Vector3.zero;
                        TryChargeAttack();
                    }
                        
                }
            }
            else // Is aiming
            {
                //
                // Get target
                //

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

                    // Get the rotation direction ( 0: no rotation; -1: left; 1: right )
                    toTargetSignedAngleRotation = Vector3.SignedAngle(currentFwd, desiredFwd, Vector3.up);

                    
                    //Debug.Log("toTargetSignedAngleRotation:" + toTargetSignedAngleRotation);
                }

                // Just debug
                //DebugTargets(targets);

                //Debug.Log("CurrentTarget:" + currentTarget);

                // 
                // Shoot
                //

                // If magazine is not empty then shoot
                if (GetAxisRaw(shootAxis) > 0)
                {
                    if (!fireWeapon.IsEmpty())
                    {
                        TryShoot();
                    }
                    else
                    {
                        // Try to reload
                        TryReload();
                    }
                }

            }

        }

        void QteAction()
        {

        }



        #endregion

        #region ANIMATION CONTROLLER

        #endregion

        #region ANIMATION EVENTS
        // Sent by the melee attack animation
        public void ChargingAttackStarted()
        {
            // Charging succeeded
            attackCharged = true;
        }

        // Sent by the melee attack animation
        public void ChargingAttackCompleted()
        {
            if (chargingAttack)
            {
                // Charging failed
                attackCharged = false;
                TryAttack();
            }
                
        }

        // Sent by the melee attack animation
        public void AttackCompleted()
        {
            attacking = false;
        }

        public void ShootCompleted()
        {
            shooting = false;
            Debug.Log("ShootCompleted");
        }

        public void ReloadCompleted()
        {
            Debug.Log("ReloadCompleted");
            reloading = false;

            fireWeapon.Reload();
        }

        public void HitCompleted()
        {
            // Some animations could be interrupted by hit, so we need to reset flags to avoid player to be stucked
            reloading = false;
            shooting = false;
            chargingAttack = false;
            attackCharged = false;

            hit = false;
        }
        #endregion

        #region DEBUG
        void DebugTargets(List<Transform> targets)
        {
            string s = "";
            foreach (Transform target in targets)
            {
                s += target.gameObject.name + " ";
                
            }
            Debug.Log("Targets:" + s);
        }

        #endregion
    }
}

