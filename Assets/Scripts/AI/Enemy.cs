﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.AI;
using UnityEngine.Events;
using System;

namespace HW
{
    public class Enemy : MonoBehaviour, ITargetable, IHitable, IMover, IActivable
    {
        enum State { Dead, Idle, Alerted, Engaged }

        public UnityAction OnFight;
        public UnityAction<HitInfo> OnGotHit;

        [Header("Engagement ranges")]
        [SerializeField]
        float perceptionRange = 2f; // You are too close ( zero or negative to disable )
        float sqrPerceptionRange;

        /// <summary>
        /// When you move or do something making noise, the noise range is multiplied by this value.
        /// </summary>
        [SerializeField]
        float hearingMul = 1f; 
        //float sqrHearingRange;
        
        [SerializeField]
        float sightRange = 8f; // They can see you ( zero or negative to disable )
        float sqrSightRange;

        [SerializeField]
        float sightAngle = 30f;

        //[SerializeField]
        //float shotHearingRange = 8f; // Enemy can hear you if you shoot within this distance ( zero or negative to disable )
        //float sqrShotHearingRange;

        [Header("Behaviours")]
        [SerializeField]
        MonoBehaviour idleBehaviour;

        [SerializeField]
        MonoBehaviour fightBehaviour;

        //[SerializeField]
        float attackCooldown = 0;

        [SerializeField]
        SpeedClass speedClass = SpeedClass.Average;
        public SpeedClass SpeedClass
        {
            get { return speedClass;}
        }

        [Header("Features")]
        [SerializeField]
        bool canNotBePushed = false;
        public bool CanNotBePushed
        {
            get { return canNotBePushed; }
        }

        [SerializeField]
        float eyesVerticalOffset = 1.75f;
        

        #region FIGHTING
        // The target this enemy is looking for ( can be both the player and any NPC )
        Transform target;
        //float attackRange = 1.25f;
        bool reacting = false;
        float pushDistance = .75f;
        float pushSpeed = 4;
        Health health;
        bool fighting = false;
        bool attackRecharging = false;
        DateTime lastAttack;
        #endregion

        #region MOVEMENT
        public event UnityAction<IMover> OnDestinationReached;
        bool hasDestination = false;
        float closeEnoughDeceleration = 20f;
        float angularSpeedDefault;
        #endregion

       

        #region ALERT
        float alertTimer = 8;
        float waitingTimer = 2f;
        Vector3 alertPosition;
        bool movingAlerted = false;
        #endregion

        State state = State.Idle;
        System.DateTime lastEngagementCheckTime;
        float engagementCheckTimer = 0.2f;
        System.DateTime lastPlayerOnSight;
        float playerLostMaxTime = 3f;
        NavMeshAgent agent;
        //GameObject player;
        bool active = false;





        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            angularSpeedDefault = agent.angularSpeed;
            ResetSpeed();

            // Attack speed
            attackCooldown = 1f/GameplayUtility.GetAttackSpeedValue(speedClass);

            // Ranges
            sqrPerceptionRange = perceptionRange * perceptionRange;
            //sqrHearingRange = hearingRange * hearingRange;
            sqrSightRange = sightRange * sightRange;
            //sqrShotHearingRange = shotHearingRange * shotHearingRange;
           

            health = GetComponent<Health>();

            
        }


        // Start is called before the first frame update
        void Start()
        {
            Activate(true);

            //player = GameObject.FindGameObjectWithTag(Tags.Player);
            PlayerController.Instance.OnHitSomething += HandlePlayerOnHitSomething;
            PlayerController.Instance.OnShoot += HandlePlayerOnShoot;
            target = PlayerController.Instance.transform;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!active)
                return;

            if(reacting || fighting || attackRecharging)
            {
                if (fighting)
                {
                    Vector3 dir = PlayerController.Instance.transform.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();

                    transform.forward = Vector3.RotateTowards(transform.forward, dir, agent.angularSpeed * Time.deltaTime, 0);
                }

                if (attackRecharging)
                {
                    if ((DateTime.UtcNow - lastAttack).TotalSeconds > attackCooldown)
                        attackRecharging = false;
                }

                return;
            }


            switch (state)
            {
                case State.Engaged:
                    //if (CheckForDisengagement())
                    if (!CheckForEngagement())
                    {
                        // If disangaged the enemy stays for a while in alert mode.
                        Alert(target.position);
                    }
                    else
                    {
                        // Check if player is inside the enemy fighting range.
                        if ((transform.position - target.position).magnitude < (fightBehaviour as IFighter).GetFightingRange())
                        {
                            // If there is nothing between the AI and the target then attack,
                            // otherwise skip and keep moving.
                            if (!IsOccluded(PlayerController.Instance.transform))
                            {
                                // Enemy starts figthing.
                                // First he stop moving.
                                agent.velocity = Vector3.MoveTowards(agent.velocity, Vector3.zero, closeEnoughDeceleration * Time.deltaTime);

                                // Start fighting if available ( we may check the cool down ).
                                if (!fighting && (fightBehaviour as IFighter).AttackAvailable())
                                {
                                    fighting = true;
                                    OnFight?.Invoke();
                                    agent.ResetPath();

                                }
                            }
                   

                        }
                        else // Out of the fighting range.
                        {
                            // Keep moving towards target.
                            //if(!IsOccluded(PlayerController.Instance.transform))
                            MoveTo(target.position);
                        }
                    }
                    
                    break;

                case State.Alerted:

                    // If alerted the enemy can still turn to engaged, otherwise, after a given time
                    // he goes back in idle state.
                    if (CheckForEngagement())
                    {
                        // Engaged, stop the alert coroutine.
                        StopCoroutine(DoAlert());
                        movingAlerted = false; // No moving alerted.
                        // Engage the player.
                        Engage(); 
                    }

                    break;
                case State.Idle:
                    // Check for engamement.
                    if (CheckForEngagement())
                    {
                        // Engage
                        Engage();
                    }
                        
                    break;

                case State.Dead:
                    break;
            }

            

            // Check if destination has been reached
            if(hasDestination && agent.remainingDistance == 0)
            {
                hasDestination = false;
                OnDestinationReached?.Invoke(this);
            }
                
        }

        #region INTERFACES IMPLEMENTATION

        public bool IsTargetable()
        {
            return !IsDead();
        }

        public void Activate(bool value)
        {
            active = value;
            Reset();
            
            if (active)
                (idleBehaviour as IBehaviour)?.StartBehaving();
            else
               (idleBehaviour as IBehaviour)?.StopBehaving();
            
        }

        public bool IsActive()
        {
            return active;
        }

        public void GetHit(HitInfo hitInfo)
        {
            Debug.Log("Receiving HitInfo:" + hitInfo);

            if (state == State.Dead)
                return;

            if (state != State.Engaged)
            {
                if((PlayerController.Instance.transform.position - transform.position).sqrMagnitude < sqrSightRange)
                    Engage();
            }
                

            // Apply damage
            health.Damage(hitInfo.DamageAmount);

            if(health.CurrentHealth <= 0)
            {
                // Stop moving
                agent.ResetPath();
                agent.speed = 0;

                state = State.Dead;
            }
                
                
         
            // Manage physical reaction
            if(hitInfo.PhysicalReaction != HitPhysicalReaction.None)
            {
                // Set true in order to avoid get target destination
                reacting = true;

                // Reset
                agent.ResetPath();

                if (hitInfo.PhysicalReaction == HitPhysicalReaction.Push && !canNotBePushed) // Push back
                {
                    // Set push speed
                    agent.speed = pushSpeed;

                    // Move back
                    agent.SetDestination(transform.position - transform.forward * pushDistance);

                    // Avoid enemy to rotate 
                    agent.angularSpeed = 0;

                }
                    
            }

            OnGotHit?.Invoke(hitInfo);


        }

        public void MoveTo(Vector3 destination)
        {
            Debug.Log("Setting destination:" + destination);
            agent.SetDestination(destination);
            hasDestination = true;
        }

        public void SetMaxSpeed(float maxSpeed)
        {
            agent.speed = maxSpeed;
        }

        // Get the maximum agent speed
        public float GetMaxSpeed()
        {
            return agent.speed;
        }

        // Get the agent speed
        public float GetSpeed()
        {
            return agent.velocity.magnitude;
        }

        #endregion

        #region PUBLIC
        public bool IsDead()
        {
            return state == State.Dead;
        }
        #endregion

        #region PRIVATE
        void Reset()
        {
            state = State.Idle;
            
            (idleBehaviour as IBehaviour)?.StopBehaving();
            ResetSpeed();
        }

        void ResetSpeed()
        {
            agent.speed = GameplayUtility.GetMovementSpeedValue(speedClass);
            agent.angularSpeed = angularSpeedDefault;
        }


        void Engage()
        {
            state = State.Engaged;
            Debug.Log("Engaged");

            // Set now
            lastPlayerOnSight = System.DateTime.UtcNow;

            // Stop idle mode
            (idleBehaviour as IBehaviour)?.StopBehaving();

            // Reset speed
            ResetSpeed();
        }


        void Alert(Vector3 position)
        {
            
            // Always update alert position
            alertPosition = position;
            
            // If already alerted then reset position and continue with the already running 
            // coroutine.
            if (state == State.Alerted)
            {
                // Maybe coroutine started moving AI, so we need to update manually the direction
                if (movingAlerted)
                    MoveTo(alertPosition);

                return;
            }
                
            // Set alerted state
            state = State.Alerted;

            movingAlerted = false;
            StartCoroutine(DoAlert());
        }

        IEnumerator DoAlert()
        {
            // Stop idle mode
            (idleBehaviour as IBehaviour)?.StopBehaving();

            // Reset speed
            ResetSpeed();

            // Stop moving
            agent.ResetPath();

            // Wait a while
            //yield return new WaitForSeconds(waitingTimer);

            // State might be changed in the meantime
            if (state != State.Alerted)
                yield break;

            // Move to alert position
            if(transform.position != alertPosition)
                MoveTo(alertPosition);

            // We want to know if coroutine started moving the IA, in order to update the alert position
            movingAlerted = true;

            // Wait a while
            yield return new WaitForSeconds(alertTimer);

            // AI is no longer moving to alert position
            movingAlerted = false;

            // State might be changed in the meantime
            if (state != State.Alerted)
                yield break;

            // Return in idle mode
            Idle();
        }

        

        void Idle()
        {
            
            state = State.Idle;

            if(idleBehaviour == null)
            {
                hasDestination = false;
                agent.ResetPath();
            }
            else
            {
                // Start idle mode
                (idleBehaviour as IBehaviour)?.StartBehaving();
            }

            
        }

        // Returns true if player is engaged otherwise false
        bool CheckForEngagement()
        {
            
            // Is already time to check for engagement?
            if ((System.DateTime.UtcNow - lastEngagementCheckTime).TotalSeconds < engagementCheckTimer)
                return state == State.Engaged;

            lastEngagementCheckTime = System.DateTime.UtcNow;

            //// I should not be here... anyway.
            //if (state == State.Engaged) 
            //    return true;

            // You can't be eangaged by dead enemies.
            if (IsDead())
                return false;

            // No reason to engage player if he's dead.
            if (PlayerController.Instance.IsDead())
                return false;

            // Distance vector from enemy to player.
            Vector3 toPlayer = PlayerController.Instance.transform.position - transform.position;
            toPlayer.y = 0; // Get rid of the y

            // Distance from player.
            float sqrDistance = toPlayer.sqrMagnitude;
            Debug.Log("PlayerDistance:" + toPlayer.magnitude);

            // If player is in the perception range then engage him.
            if (sqrPerceptionRange > 0 && sqrDistance < sqrPerceptionRange)
                return true;

            // Check if the enemy can hear the player.
            // We must compute the difference between the noise range given by the player 
            // and the hearing range of the enemy and then check if the value is bigger 
            // than the distance from the player.
            // First we check if the enemy has a hearing reange at all.
            if (hearingMul > 0)
            {
                Debug.Log("PlayerNoise:" + PlayerController.Instance.GetNoiseRange());

                // Add the enemy hearing range to the player noise range.
                float sqrRange = PlayerController.Instance.GetNoiseRange() * hearingMul;
                sqrRange *= sqrRange;
                // If player is inside the above range then engage him.
                if (sqrDistance < sqrRange)
                    return true;
            }

            // Check if the enemy can see the player ( only if sight range > 0 ).
            if(sightRange > 0)
            {
                // We simply check if the enemy can see the player.
                if (IsInFieldOfView(PlayerController.Instance.transform, sightRange, sightAngle) && !IsOccluded(PlayerController.Instance.transform))
                {
                    // If enemy can see the player we need to check if he's already engaged,
                    // and eventually engage him.
                    
                    // This will come in handy when disengaging.
                    lastPlayerOnSight = System.DateTime.UtcNow;

                    // Now if the player was not engaged then we engage him.
                    //if (state != State.Engaged)
                    return true;
                }
                else
                {
                    // The enemy can't see the player.
                    // If he's engaged we need to check how long the player is out of the enemy
                    // sight range, and eventually leave him engaged.
                    if ((System.DateTime.UtcNow - lastPlayerOnSight).TotalSeconds < playerLostMaxTime)
                    {
                        return true;
                    }
                }
  
            }
            

            return false;
            
        }

        //bool CheckForDisengagement()
        //{
        //    // Already time to check?
        //    if ((System.DateTime.UtcNow - lastEngagementCheckTime).TotalSeconds < engagementCheckTimer)
        //        return false;

        //    lastEngagementCheckTime = System.DateTime.UtcNow;

        //    // I should not be here... anyway
        //    if (state != State.Engaged)
        //        return true;
    

        //    // Dead enemies can't engage noone.
        //    if (IsDead())
        //        return true;
            

        //    // You already kill the player, disengage him.
        //    if (PlayerController.Instance.IsDead())
        //        return true;

        //    // Now we must check for sight, hearing and perception.
        //    // If all them fail checking then disangage.


        //    // Mutants can't see or hear anything, so their sight and hear range are zero. This
        //    // means they can only feel you presence when you are too close ( generally their 
        //    // proximity range is very large ) and they disangage you when you go too far away.
        //    // Humans work in a different way: they can still fell you when you go too close but
        //    // their range is smaller than mutants' range. But they can hear or see you in a long
        //    // range yet. This means that if you are too close to a mutant he know where you are,
        //    // instead if you go away from a human field of view he doesn't know where you are, but
        //    // he reaches to check your last position.
        //    //
        //    // If sight range is bigger than zero then we are dealing with a human AI and we
        //    // take into account the field of view.
        //    if(sightRange > 0)
        //    {
        //        if (IsInFieldOfView(PlayerController.Instance.transform, sightRange, sightAngle) && !IsOccluded(PlayerController.Instance.transform))
        //        {
        //            lastPlayerOnSight = System.DateTime.UtcNow;
        //        }
        //        else
        //        {
        //            if ((System.DateTime.UtcNow - lastPlayerOnSight).TotalSeconds > playerLostMaxTime)
        //            {
        //                return true;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        // Mutants, only proximity range is taken into account, increasing by 30%.
        //        float sqrDist = (PlayerController.Instance.transform.position - transform.position).sqrMagnitude;
        //        // Since we are using the square, 1.69 is 1.3 x 1.3. 
        //        if (sqrDist > 1.69f * sqrPerceptionRange)
        //            return true;
        //    }

        //    return false;
        //}

        // Check for the target to be inside a visual cone given angle and distance 
        bool IsInFieldOfView(Transform target, float maxDistance, float maxAngle)
        {
            // Vector to target
            Vector3 toTarget = target.position - transform.position;

            float distance = toTarget.magnitude;

            // If target is not within the max distance return false
            if (distance > maxDistance)
                return false;

            // If target if out of max angle then return false
            if (Vector3.Angle(toTarget, transform.forward) > maxAngle)
                return false;


            return true;
        }

        bool IsOccluded(Transform target)
        {
            // Direction from ai to target ( ex. player )                        
            Vector3 toTarget = target.position - transform.position;

            // Avoid ground ( charaters have pivot on feet )
            int layer = ~LayerMask.GetMask(Layers.Ground);

            // Distance between ai and target
            float distance = toTarget.magnitude;


            RaycastHit hitInfo;
            Ray ray = new Ray(transform.position + Vector3.up * eyesVerticalOffset, toTarget.normalized);
            
            // Avoid to hit itself
            GetComponent<Collider>().enabled = false;

            // Cast ray
            bool hit = Physics.Raycast(ray, out hitInfo, distance, layer);

            // Enable collision back
            GetComponent<Collider>().enabled = true;
            
            // Something has been hit
            if (hit)
            {
                if (hitInfo.transform != target)
                    return true;
            }

            return false;
        }

        // Engages you if you hit something with your melee weapon within a given range ( not engaged if you miss the target )
        void HandlePlayerOnHitSomething(Weapon weapon)
        {

            if (state == State.Engaged || state == State.Dead)
                return;

            if(weapon.GetType() == typeof(MeleeWeapon))
            {
                float sqrDistance = (PlayerController.Instance.transform.position - transform.position).sqrMagnitude;
                float sqrNoiseRange = weapon.NoiseRange * weapon.NoiseRange;
                if (sqrDistance < sqrNoiseRange)
                    Engage();
                    
            }
        }

        // Engages you if you shoot within a given range
        void HandlePlayerOnShoot(Weapon weapon)
        {
            
            if (state == State.Engaged || state == State.Dead)
                return;

            float sqrDistance = (PlayerController.Instance.transform.position - transform.position).sqrMagnitude;
            float sqrNoiseRange = weapon.NoiseRange * weapon.NoiseRange;
            if (sqrDistance < sqrNoiseRange)
                Alert(PlayerController.Instance.transform.position);
                
        }

        #endregion
        // Reset the agent speed to max speed

        #region ANIMATIONS EVENTS
        public void ReactionCompleted()
        {
            ResetSpeed();
            reacting = false;
            fighting = false;
            attackRecharging = false;
        }

        public void DeadCompleted()
        {
            fighting = false;

            if(GetComponent<Collider>())
                GetComponent<Collider>().enabled = false;
        }

        public void AttackCompleted()
        {
            // Stop fighting
            fighting = false;

            // Need to rechearge?
            if (attackCooldown > 0)
            {
                attackRecharging = true;
                lastAttack = DateTime.UtcNow;
            }
            

        }

        public void Hit()
        {
            (fightBehaviour as IFighter).Fight(PlayerController.Instance.transform);
            
        }

       
        #endregion





    }

}
