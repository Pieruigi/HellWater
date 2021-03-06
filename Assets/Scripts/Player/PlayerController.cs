﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.Events;
using HW.Collections;

namespace HW
{
    

    public class PlayerController : MonoBehaviour, IHitable
    {
        #region ACTIONS
        public UnityAction<Weapon> OnShoot; // Called on shoot
        public UnityAction OnStartAiming; // Called when you start aiming
        public UnityAction OnStopAiming; // Called when you stop aiming
        public UnityAction<bool> OnAttack; // Called when melee charging stops; param tell if attack has been charged
        public UnityAction OnIsOutOfAmmo; // Called when your firegun is out of ammo
        public UnityAction OnReload; // Called on reloading
        public UnityAction OnReloadInterrupted; // Call when reloading is interrupted ( you move or you get hit )
        public UnityAction<Weapon> OnSetCurrentWeapon; // Call when you holster up some weapon
        public UnityAction OnResetCurrentWeapon; // Called when you holster the weapon you are holding
        public UnityAction<HitInfo> OnGotHit; // Called when you get hit ( hit info are sent as param )
        public UnityAction<Weapon> OnHitSomething; // Called when you hit something with your weapon
        public UnityAction<Weapon, Transform> OnTargeting; // Called everytime you acquire or switch a target ( null means no target )
        public UnityAction OnDead;
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
        bool fireWeaponAccuracySystem = false;
        public bool FireWeaponAccuracySystem
        {
            get { return fireWeaponAccuracySystem; }
        }
        #endregion


        #region LOCOMOTION FIELDS
        // The max speed the player can reach depending on whether is running or not.
        float maxSpeed;

        // Is player running ?
        bool running = false;

        // The target velocity
        Vector3 desiredVelocity;
        Vector3 currentVelocity;
        #endregion

        #region Noise Ranges
        float idleNoiseRange = 1.5f; // The minimum noise you can do ( normally in idle ).
        float walkNoiseRange = 3.5f; // Noise multiplier while walking.
        float runNoiseRange = 4.5f; // Noise multiplier while running.
        #endregion

        #region FIGHTING FIELDS
        bool aiming = false;
        bool reloading = false;
        bool shooting = false;
        bool attacking = false; // Performing attack with melee weapon
        bool hit = false;
        bool holsterForced = false;

        // Health 
        Health health;

        float toTargetSignedAngleRotation = 0;
        public float ToTargetSignedAngleRotation
        {
            get { return toTargetSignedAngleRotation; }
        }

        // The fireweapon currently equipped ( primary or secondary weapon ).
        FireWeapon fireWeapon;
        public FireWeapon FireWeapon
        {
            get { return fireWeapon; }
        }

        // The melee weapon
        MeleeWeapon meleeWeapon;
        public MeleeWeapon MeleeWeapon
        {
            get { return meleeWeapon; }
        }

        // Is the weapon you are actually holding.
        // You must be in attacking mode ( aiming, shooting or striking ).
        Weapon currentWeapon;
        public Weapon CurrentWeapon
        {
            get { return currentWeapon; }
        }
        

        //Transform desiredTarget;
        Transform currentTarget;


        System.DateTime lastShoot;
        float shootTime = 0.35f;
        #endregion


        #region MISC FIELDS
        Rigidbody rb;
        bool disabled = false; // Is this controller disabled ?
        float sphereCastRadius = 2.0f; // Used to get targets
        float angularSpeedInRadians;
        Vector3 raycastOffset;
        Transform startingPoint;
        CapsuleCollider coll;
        bool loadedFromCache = false;
        public bool LoadedFromCache
        {
            get { return loadedFromCache; }
            set { loadedFromCache = value; }
        }
        #endregion

        public static PlayerController Instance { get; private set; }
        

        #region NATIVE METHODS
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                rb = GetComponent<Rigidbody>();
                angularSpeedInRadians = angularSpeed * Mathf.Deg2Rad;
                health = GetComponent<Health>();
                raycastOffset = Vector3.up * Constants.RaycastVerticalOffset;
                coll = GetComponent<CapsuleCollider>();
                
            }
            else
            {
                Destroy(gameObject);
            }

        }

        // Start is called before the first frame update
        void Start()
        {
           
            Reset();

            //GetComponentInChildren<MeleeWeapon>().OnHit += HandleOnHitSomething;
            // Set equipment handles.
            Equipment.Instance.OnWeaponAdded += HandleOnWeaponAdded;
            Equipment.Instance.OnWeaponRemoved += HandleOnWeaponRemoved;
            

            // If PlayerController starts after Equipment then we must get data by hand.
            fireWeapon = Equipment.Instance.SecondaryWeapon;
            meleeWeapon = Equipment.Instance.MeleeWeapon;

        }

        // Update is called once per frame
        void Update()
        {
            // Update velocity
            float speedChange = ((currentVelocity.sqrMagnitude < desiredVelocity.sqrMagnitude) ? acceleration : deceleration) * Time.deltaTime;
            currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, speedChange);

            // You can't do anything else while you are doing one of these actions
            if (disabled || /*reloading ||*/ shooting || attacking || hit || IsDead())
                return;

            // If you are reloading you can still move, but reloading will be interrupted
            if (reloading)
            {
                if (PlayerInput.GetAxisRaw(PlayerInput.HorizontalAxis) != 0 || PlayerInput.GetAxisRaw(PlayerInput.VerticalAxis) != 0)

                {
                    reloading = false;
                    OnReloadInterrupted?.Invoke();
                    (currentWeapon as FireWeapon).OnReloadInterrupted?.Invoke();
                }

                return;
            }

            // Player is attacking.
            if (attacking)
            {
                // Rotate the player towards the choosen target if there is one
                TryRotateTowardsTarget();

                // We can't move or do anything else while while we are charging attack
                return;
            }

        
            // Switch fire weapon
            //CheckIsSwitchingWeapon();

            // Check if player is aiming
            CheckIsAiming();

            // Check if equipped fire weapon must be reloaded
            CheckIsReloading();


            if (!aiming)
            {
                OnTargeting?.Invoke(currentWeapon, null);


                // Check movement.
                // Check if player is running.
                CheckIsRunning();

                // Get player movement input. 
                Vector2 input = new Vector2(PlayerInput.GetAxisRaw(PlayerInput.HorizontalAxis), PlayerInput.GetAxisRaw(PlayerInput.VerticalAxis)).normalized;

                Vector3 fwd = Camera.main.transform.forward;
                fwd = Vector3.ProjectOnPlane(fwd, Vector3.up).normalized;
                Vector3 rgt = Camera.main.transform.right;
                rgt = Vector3.ProjectOnPlane(rgt, Vector3.up).normalized;
                //desiredVelocity = Vector3.right * input.x + Vector3.forward * input.y;
                desiredVelocity = rgt * input.x + fwd * input.y;

                desiredVelocity *= maxSpeed;

                // Look to the direction you are moving towards.
                Vector3 desiredFwd = desiredVelocity.normalized;

                // Get the direction player is looking at
                Vector3 currentFwd = transform.forward;
                if (desiredVelocity == Vector3.zero)
                    desiredFwd = currentFwd;

                // Look the direction you are heading 
                transform.forward = Vector3.RotateTowards(currentFwd, desiredFwd, angularSpeedInRadians * Time.deltaTime, 0);


                // 
                // Melee attack
                //

                // Start charging melee attack
                if (PlayerInput.GetAxisRaw(PlayerInput.ShootAxis) > 0)
                {
                    if(!holsterForced) // Ok, lets fight
                    {
                        if (meleeWeapon)
                        {
                            // Set melee weapon.
                            SetCurrentWeapon(meleeWeapon);
                            
                            // Stop moving
                            desiredVelocity = Vector3.zero;

                            // Get all the targets inside the weapon range which are not hidden by any obstacle
                            List<Transform> targets = GetAvailableTargets(meleeWeapon.Range * 2f);

                            // Set the target or null
                            currentTarget = GetClosestTarget(targets);

                            TryAttack();
                            
                        }
                    }


                }


            }
            else // Is aiming
            {
                // Stop moving
                desiredVelocity = Vector3.zero;

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
                    Vector3 direction = new Vector3(PlayerInput.GetAxisRaw(PlayerInput.HorizontalAxis), 0, PlayerInput.GetAxisRaw(PlayerInput.VerticalAxis)).normalized;

                    // I'm trying to target someone else
                    if (direction != Vector3.zero)
                    {
                        // Get the target the player is aiming or null
                        Transform newTarget = GetNewTarget(targets, direction, fireWeapon.Range);

                        // 
                        if (newTarget && newTarget != currentTarget)
                            currentTarget = newTarget;
                    }


                }

                // Check for event to be called
                if (lastTarget != currentTarget)
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
                if (PlayerInput.GetAxisRaw(PlayerInput.ShootAxis) > 0 && (System.DateTime.UtcNow - lastShoot).TotalSeconds > shootTime)
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

        void FixedUpdate()
        {
            // Check collision to avoid player keep moving against walls.
            Ray ray = new Ray(rb.position + Vector3.up*0.5f, transform.forward);
            RaycastHit hitInfo;
            // We must add the player collider radius to the distance
            float distance = coll.radius + (currentVelocity.magnitude * Time.fixedDeltaTime);
            if (Physics.Raycast(ray, out hitInfo, distance)) // We hit something
            {
                // Move the player to the collision point
                // We keep the player y position
                float y = rb.position.y;

                // We must take into account the collider radius when moving player towards collider
                Vector3 safePos = hitInfo.point - transform.forward * coll.radius;
                safePos.y = y;
                rb.MovePosition(safePos);

            }
            else // No collision detected
            {
                // Move the player according to his velocity
                rb.MovePosition(rb.position + transform.forward * currentVelocity.magnitude * Time.fixedDeltaTime);
            }
            

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
            
            // You are already dead
            if (IsDead())
                return;

            if (hitInfo.PhysicalReaction != HitPhysicalReaction.None)
            {
                hit = true;
                desiredVelocity = Vector3.zero;
            }


            // Apply damage
            health.Damage(hitInfo.DamageAmount);

            // Hit event
            OnGotHit?.Invoke(hitInfo);

            if (IsDead())
            {
                //GetComponent<Collider>().enabled = false;
                OnDead?.Invoke();
            }

        }
        #endregion

        #region PUBLIC
      
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
            return currentVelocity.magnitude;
        }

        public float GetCurrentSpeedNormalized()
        {
            return currentVelocity.magnitude / maxRunningSpeed;
        }

        public void SetDisabled(bool value)
        {
           
            if (disabled == value)
                return;

            disabled = value;

            if (value)
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

                // Hold the equipped fire weapon
                SetCurrentWeapon(fireWeapon);

                OnStartAiming?.Invoke();
            }
            else
            {
                currentTarget = null;

                ResetCurrentWeapon();

                OnStopAiming?.Invoke();
            }
        }

        public void HolsterWeapon(bool forced = false)
        {
            holsterForced = forced;
            ResetCurrentWeapon();
        }

        public void ResetHolsterForced()
        {
            holsterForced = false;
        }

        public void EquipWeapon(Item item)
        {
          

            if (item.Type != ItemType.Weapon)
                throw new System.Exception("EquipWeapon() can't be called with param of type " + item.Type + ".");

            // Get weapon
            Weapon weapon = null;// new List<Weapon>(GetComponentsInChildren<Weapon>()).Find(w => w.Item == item);
            

            if (weapon == null)
                throw new System.Exception("No weapon can be found from item " + item + ".");

            EquipWeapon(weapon);
        }

        // Equips and holds fire or melee weapon
        private void EquipWeapon(Weapon weapon)
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

            }
            else // Is melee ( we only have bat )
            {
                // No weapon equipped yet
                if (!meleeWeapon)
                    meleeWeapon = weapon as MeleeWeapon;


            }
        }

       
        /// <summary>
        /// Return the noise range.
        /// </summary>
        public float GetNoiseRange()
        {
            // If player is not moving we return the minimum range.
            if (GetCurrentSpeed() == 0)
                return idleNoiseRange;

            // If player is moving we return the minimum range increased by a given amount
            // depending on the locomotion type.
            // First check if player is running.
            if (IsRunning())
                return runNoiseRange;

            // Not in stealth mode.
            return walkNoiseRange;
        }

        #endregion

        #region PRIVATE

        /// <summary>
        /// Set visible the weapon passed as parameter.
        /// </summary>
        /// <param name="weapon"></param>
        void SetCurrentWeapon(Weapon weapon)
        {
            // SetCurrentWeapon(null) == ResetCurrentWeapon().
            if (!weapon)
            {
                ResetCurrentWeapon(); 
                return;
            }

            // Another weapon is held, so reset it. 
            if (currentWeapon != weapon)
            {
                ResetCurrentWeapon();
            }

            // Hold the new weapon.
            if (!currentWeapon)
            {
                currentWeapon = weapon;
                currentWeapon.SetVisible(true);

                OnSetCurrentWeapon?.Invoke(weapon);
            }

        }

        /// <summary>
        /// Set unvisible any weapon.
        /// </summary>
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
            //bool axis = GetAxisRaw(sprintAxis) > 0 ? true : false;
            //bool axis = GetAxisRaw(sprintAxis) > 0 ? true : false;
            bool axis = PlayerInput.GetSprintButton();

            // Run
            if (axis)
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

            bool aim = PlayerInput.GetAxisRaw(PlayerInput.AimAxis) > 0;

 
            SetAiming(aim);


        }

        void CheckIsReloading()
        {
            // Reloading only works on fire weapons
            if (!fireWeapon)
                return;

            if (fireWeapon.IsFull())
                return;

            if (GetCurrentSpeed() > 0)
                return;

            // Check button
            if(currentWeapon == fireWeapon)
            {
                if (PlayerInput.GetButtonDown(PlayerInput.ReloadAxis))
                {

                    Debug.Log("Reload");
                    TryReload();
                }
            }
            

        }

        // Max speed will change depending on whether the player is running or not.
        // It also decrease if player is crouching.
        void ResetMaxSpeed()
        {
            maxSpeed = running ? maxRunningSpeed : maxWalkingSpeed;
        }

        void TryReload()
        {
            if (!fireWeapon)
                return;

            if (fireWeapon.IsOutOfAmmo())
            {
                OnIsOutOfAmmo?.Invoke();
            }
            else
            {

                // Reset all
                Reset();

                // Set reloading
                reloading = true;

                // Set aiming false to fix transition issues
                SetAiming(false);

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

                OnShoot?.Invoke(fireWeapon);
            }

        }

        void TryAttack()
        {

            if (!meleeWeapon)
                return;

            attacking = true;

            

            OnAttack?.Invoke(true);
        }


      

        // Returns all the available target depending on radius and obstacles
        List<Transform> GetAvailableTargets(float radius)
        {
            List<Transform> ret = new List<Transform>();

            // Overlap a sphere with the origin in the player position
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            // Loop through the overlapped colliders
            foreach (Collider collider in colliders)
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

                // If there is no obstacle between player and target then add the target to the returning list
                if (!Physics.Raycast(transform.position + raycastOffset, direction.normalized, direction.magnitude))
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

            foreach (Transform target in targets)
            {
                float sqrDist = (transform.position - target.position).sqrMagnitude;
                if (ret == null || sqrDist < sqrMinDist)
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
            if (Physics.SphereCast(transform.position + raycastOffset, sphereCastRadius, aimingDirection, out hitInfo, distance))
                ret = targets.Find(t => t == hitInfo.transform);

            return ret;
        }

        void Reset()
        {
            running = false;
            aiming = false;
            reloading = false;
            shooting = false;
            attacking = false;
            toTargetSignedAngleRotation = 0;
            hit = false;
            desiredVelocity = Vector2.zero;

            currentTarget = null;

            ResetMaxSpeed();
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
            if (value)
                OnHitSomething(weapon);
        }

        void HandleOnWeaponAdded(Weapon weapon)
        {
            if (weapon.GetType() == typeof(MeleeWeapon))
            {
                (weapon as MeleeWeapon).OnHit += HandleOnHitSomething;
                meleeWeapon = weapon as MeleeWeapon;
            }
            else
            {
                if (weapon.GetType() == typeof(FireWeapon))
                {
                    //if(Equipment.Instance.IsPrimary(weapon as FireWeapon))
                    fireWeapon = weapon as FireWeapon;
                }
            }

            // We must attach the game object.
            weapon.transform.parent = GetComponent<HumanoidNodeCollection>().GetNode(HumanoidNodeName.RightHand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        void HandleOnWeaponRemoved(Weapon weapon)
        {
            if (weapon.GetType() == typeof(MeleeWeapon))
            {
                (weapon as MeleeWeapon).OnHit -= HandleOnHitSomething;
                meleeWeapon = null;
            }
            else
            {
                if(weapon.GetType() == typeof(FireWeapon))
                {
                    fireWeapon = null;
                }
            }

        }

        

        #endregion

        #region ANIMATION CONTROLLER

        #endregion

        #region ANIMATION EVENTS

        // Sent by the melee attack animation

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
            

            hit = false;
        }

        public void Strike()
        {
            if (!meleeWeapon || currentWeapon != meleeWeapon)
                return;

            (currentWeapon as MeleeWeapon).Strike();

        }

        public void ActionCompleted()
        {
            SetDisabled(false);
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


