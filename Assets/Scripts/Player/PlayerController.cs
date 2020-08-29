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
        public UnityAction OnShoot; // Called on shoot
        public UnityAction OnStartAiming; // Called when you start aiming
        public UnityAction OnStopAiming; // Called when you stop aiming
        public UnityAction OnChargeAttack; // Called when you start charging melee attack
        public UnityAction<bool> OnAttackCharged; // Called passing true if melee attack is charged, otherwise false is passed
        public UnityAction<bool> OnAttack; // Called when melee charging stops; param tell if attack has been charged
        public UnityAction OnIsOutOfAmmo; // Called when your firegun is out of ammo
        public UnityAction OnReload; // Called on reloading
        public UnityAction OnReloadInterrupted; // Call when reloading is interrupted ( you move or you get hit )
        public UnityAction<Weapon> OnSetCurrentWeapon; // Call when you holster up some weapon
        public UnityAction OnResetCurrentWeapon; // Called when you holster the weapon you are holding
        public UnityAction<HitInfo> OnGotHit; // Called when you get hit ( hit info are sent as param )
        public UnityAction<Weapon> OnHitSomething; // Called when you hit something with your weapon
        public UnityAction<Weapon, Transform> OnTargeting; // Called everytime you acquire or switch a target ( null means no target )
        
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

        [SerializeField]
        bool chargeMeleeAttack = false;

        [SerializeField]
        bool fireWeaponAccuracySystem = false;
        public bool FireWeaponAccuracySystem
        {
            get { return fireWeaponAccuracySystem; }
        }
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
        float releaseWeaponTimer = 8;
        float currentReleaseWeaponTimer = 0;
        

        //Transform desiredTarget;
        Transform currentTarget;
        

        System.DateTime lastShoot;
        float shootTime = 0.35f;
        #endregion

        #region CONTROLLER AXIS 
        string horizontalAxis = "Horizontal";
        string verticalAxis = "Vertical";
        string sprintAxis = "Run";
        string aimAxis = "Aim";
        string reloadAxis = "Reload";
        string shootAxis = "Shoot";
        string actionAxis = "Action";
        #endregion

        #region MISC FIELDS
        Rigidbody rb;
        bool disabled = false; // Is this controller disabled ?
        float sphereCastRadius = 2.0f; // Used to get targets
        bool qteAction = false;
        float angularSpeedInRadians;
        Vector3 raycastOffset;
        #endregion

        #region NATIVE METHODS
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            angularSpeedInRadians = angularSpeed * Mathf.Deg2Rad;
            health = GetComponent<Health>();
            raycastOffset = Vector3.up * Constants.RaycastVerticalOffset;
        }

        // Start is called before the first frame update
        void Start()
        {
            Reset();

            GetComponentInChildren<MeleeWeapon>().OnHit += HandleOnHitSomething;

        }

        // Update is called once per frame
        void Update()
        {
            

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
        public void GetHit(HitInfo hitInfo)
        {
            if (reloading)
            {
                reloading = false;
                OnReloadInterrupted?.Invoke();
                (currentWeapon as FireWeapon).OnReloadInterrupted?.Invoke();
            }

            shooting = false;
            attacking = false;
            chargingAttack = false;

            // You are already dead
            if (IsDead())
                return;

            if(hitInfo.PhysicalReaction != HitPhysicalReaction.None)
            {
                hit = true;
                desiredVelocity = Vector3.zero;
            }
                

            // Apply damage
            health.Damage(hitInfo.DamageAmount);

            if (IsDead())
                GetComponent<Collider>().enabled = false;

            // Hit event
            OnGotHit?.Invoke(hitInfo);
        }
        #endregion

        #region PUBLIC
        public bool GetActionDown()
        {
            return GetAxisRaw(actionAxis) > 0 ? true : false;
        }

        public bool IsRunning()
        {
            return running;
        }

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
                //ResetCurrentWeapon();

                OnStopAiming?.Invoke();
            }
        }

        public void HolsterWeapon()
        {
            ResetCurrentWeapon();
        }


        // Equips and holds fire or melee weapon
        public void EquipWeapon(Weapon weapon)
        {

            // Is it a fire weapon ?
            if (weapon.GetType() == typeof(FireWeapon))
            {
                // Switching between fire weapons
                if (fireWeapon && fireWeapon != weapon)
                    fireWeapon.SetVisible(false);

                // No weapon equipped yet
                if (!fireWeapon)
                    fireWeapon = weapon as FireWeapon;

                // Ok let's see this weapon
                SetCurrentWeapon(fireWeapon);
            }
            else // Is melee ( we only have bat )
            {
                // No weapon equipped yet
                if (!meleeWeapon)
                    meleeWeapon = weapon as MeleeWeapon;

                // Ok let's see this weapon
                SetCurrentWeapon(meleeWeapon);
            }
        }

        #endregion

        #region PRIVATE

        void SetCurrentWeapon(Weapon weapon)
        {
            

            currentReleaseWeaponTimer = releaseWeaponTimer;

            if (currentWeapon != null && currentWeapon != weapon)
            {
                currentWeapon.SetVisible(false);
                currentWeapon = null;
            }

            if (!currentWeapon)
            {
                currentWeapon = weapon;
                currentWeapon.SetVisible(true);

                OnSetCurrentWeapon?.Invoke(weapon);
            }
                
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

                // Event on player
                OnReload?.Invoke();

                // We also invoke event on current weapon
                fireWeapon.OnReload?.Invoke();
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

            SetCurrentWeapon(meleeWeapon);
            //EquipWeapon(meleeWeapon);

            // Event
            OnChargeAttack?.Invoke();
        }

        void TryAttack()
        {
           
            if (!meleeWeapon)
                return;

            if(!chargeMeleeAttack)
                attackCharged = true; // Only if you want to attack without charging

            // Check is attack will succeed
            if (attackCharged)
            {
                attackCharged = false;

                // Start attacking if attack succeeds
                attacking = true;
                OnAttack?.Invoke(true);
            }
            else
            {
                attacking = false; // Be sure
                attackFailed = true;
                StartCoroutine(AttackFailedCooldown());
                OnAttack?.Invoke(false);
            }

            // Stop charging
            chargingAttack = false;


            OnAttackCharged?.Invoke(false);

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
                if (targetable == null || !targetable.IsTargetable())
                    continue;

                // Get vector heading from player to target
                Vector3 direction = (targetable as MonoBehaviour).transform.position - transform.position;
                
                // get target collider
                Collider targetColl = (targetable as MonoBehaviour).GetComponent<Collider>();

                // Disable target collider if exists
                if (targetColl)
                    targetColl.enabled = false;

                // If there is no obstacle between player and target then add to the returning list
                if (!Physics.Raycast(transform.position+raycastOffset, direction.normalized, direction.magnitude))
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
            //Debug.Log("Targets.Count:" + targets.Count);
            //Transform ret = null;
            //float angle = 0;
            //foreach(Transform target in targets)
            //{

            //    Vector3 toTarget = target.position - transform.position;
            //    float tmpAngle = Vector3.Angle(aimingDirection, toTarget);
            //    Debug.Log("Target:" + target.name + "; angle:" +tmpAngle);
            //    if (ret == null || tmpAngle < angle)
            //    {
            //        ret = target;
            //        angle = tmpAngle;                    
            //    }

            //}

            Transform ret = null;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position+raycastOffset, sphereCastRadius, aimingDirection, out hitInfo, distance))
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
            // You can't do anything else while you are doing one of these actions
            if (disabled || /*reloading ||*/ shooting || attacking || hit || IsDead())
                return;

            // If you are reloading you can still move, but reloading will be interrupted
            if (reloading)
            {
                if (GetAxisRaw(horizontalAxis) != 0 || GetAxisRaw(verticalAxis) != 0)
                {
                    reloading = false;
                    OnReloadInterrupted?.Invoke();
                    (currentWeapon as FireWeapon).OnReloadInterrupted?.Invoke();
                }

                return;
            }

            // Is charging melee attack
            if (chargingAttack)
            {
                // Rotate the player towards the choosen target if there is one
                TryRotateTowardsTarget();
                
                if (GetAxisRaw(shootAxis) == 0)
                {
                    TryAttack();
                }
                // We can't move or do anything else while while we are charging attack
                return;
            }
            
            // Switch fire weapon
            //CheckIsSwitchingWeapon();

            // Check if equipped fire weapon must be reloaded
            CheckIsReloading();

            // Check if player is aiming
            CheckIsAiming();

            if (!aiming)
            {
                OnTargeting?.Invoke(currentWeapon, null);

                // Time to holster weapon?
                currentReleaseWeaponTimer -= Time.deltaTime;
                if (currentReleaseWeaponTimer < 0)
                    ResetCurrentWeapon();

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
                // Look the direction you are heading
                //

                // Get the direction the player is moving towards
                Vector3 desiredFwd = desiredVelocity.normalized;

                // Get the direction player is looking at
                Vector3 currentFwd = transform.forward;
                if (desiredVelocity == Vector3.zero)
                    desiredFwd = currentFwd;

                // Lerp 
                transform.forward = Vector3.RotateTowards(currentFwd, desiredFwd, angularSpeedInRadians * Time.deltaTime, 0);

                // 
                // Melee attack
                //

                // Start charging melee attack
                if (GetAxisRaw(shootAxis) > 0)
                {
                    if (meleeWeapon && !attackFailed)
                    {
                        // Stop moving
                        desiredVelocity = Vector3.zero;

                        // Get all the targets inside the weapon range which are not hidden by any obstacle
                        List<Transform> targets = GetAvailableTargets(fireWeapon.Range * 2f);

                        // Set the target or null
                        //Transform lastTarget = currentTarget;
                        currentTarget = GetClosestTarget(targets);

                        // Send information about targeting
                        //if(lastTarget != currentTarget)
                            

                        TryChargeAttack();
                    }
                        
                }

                
            }
            else // Is aiming
            {
                //
                // Get target
                //

                // Get all the targets inside the weapon range which are not hidden by any obstacle
                List<Transform> targets;
                if (!fireWeaponAccuracySystem)
                    targets = GetAvailableTargets(fireWeapon.Range);
                else
                    targets = GetAvailableTargets(FireWeapon.GlobalAimingRange);


                // Last target will be useful to decide to call or not an event
                Transform lastTarget = currentTarget;

                // Clear current target if not longer available
                if (currentTarget && !targets.Contains(currentTarget))
                    currentTarget = null;


                // If there no target yet then set the closest one if available
                if (!currentTarget)
                {
                    // Set the new target if available
                    currentTarget = GetClosestTarget(targets);
                }
                else
                {
                    // Get the aiming direction
                    Vector3 direction = new Vector3(GetAxisRaw(horizontalAxis), 0, GetAxisRaw(verticalAxis)).normalized;

                    // I'm trying to target someone else
                    if(direction != Vector3.zero)
                    {
                        // Get the target the player is aiming or null
                        Transform newTarget = GetNewTarget(targets, direction, fireWeapon.Range);

                        // 
                        if (newTarget && newTarget != currentTarget)
                            currentTarget = newTarget;
                    }
                    
    
                }

                // Check for event to be called
                if(lastTarget != currentTarget)
                    OnTargeting?.Invoke(currentWeapon, currentTarget);

                // Rotate the player towards the choosen target if there is one
                TryRotateTowardsTarget();


                // Just debug
                //DebugTargets(targets);

                //Debug.Log("CurrentTarget:" + currentTarget);

                // 
                // Shoot
                //

                // If magazine is not empty then shoot
                if (GetAxisRaw(shootAxis) > 0 && (System.DateTime.UtcNow - lastShoot).TotalSeconds > shootTime)
                {
                    lastShoot = System.DateTime.UtcNow;
                    if (!fireWeapon.IsEmpty())
                    {
                        TryShoot();
                    }
                    else
                    {
                        Debug.Log("IsOutOfAmmo...");
                        if (fireWeapon.IsOutOfAmmo())
                            fireWeapon.OnOutOfAmmo?.Invoke();
                        else
                            TryReload();
                    }
                }

            }

        }

        void QteAction()
        {

        }

        void TryRotateTowardsTarget()
        {
            if (currentTarget)
            {
                // Get the direction the player must look at
                Vector3 desiredFwd = currentTarget.position - transform.position;
                desiredFwd.y = 0;
                desiredFwd.Normalize();
                
                // Get the current direction
                Vector3 currentFwd = transform.forward;
                currentFwd.y = 0;
                currentFwd.Normalize();
                
                // Lerp rotation
                transform.forward = Vector3.RotateTowards(currentFwd, desiredFwd, angularSpeedInRadians * Time.deltaTime, 0);
                
                // Get the rotation direction ( 0: no rotation; -1: left; 1: right )
                toTargetSignedAngleRotation = Vector3.SignedAngle(transform.forward, desiredFwd, Vector3.up);

                // If the angle module is less than 1 degrees then reset
                if (Mathf.Abs(toTargetSignedAngleRotation) < 1) 
                    toTargetSignedAngleRotation = 0;
            }
        }

        void HandleOnHitSomething(bool value, Weapon weapon)
        {
            if(value)
                OnHitSomething(weapon);
        }

        #endregion

        #region ANIMATION CONTROLLER

        #endregion

        #region ANIMATION EVENTS
        // Sent by the melee attack animation
        public void ChargingAttackStarted()
        {
            if (chargingAttack)
            {
                // Charging succeeded
                attackCharged = true;

                OnAttackCharged?.Invoke(true);
            }
            
        }

        // Sent by the melee attack animation
        public void ChargingAttackCompleted()
        {
            if (chargingAttack)
            {
                // Charging failed
                attackCharged = false;
                TryAttack();

                OnAttackCharged?.Invoke(false);
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

        public void ReactionCompleted()
        {
            // Some animations could be interrupted by hit, so we need to reset flags to avoid player to be stucked
            reloading = false;
            shooting = false;
            chargingAttack = false;
            attackCharged = false;

            hit = false;
        }

        public void Strike()
        {
            if (!meleeWeapon || currentWeapon != meleeWeapon)
                return;

            (currentWeapon as MeleeWeapon).Strike();

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

