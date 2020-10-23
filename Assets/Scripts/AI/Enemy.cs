using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Interfaces;
using UnityEngine.AI;
using UnityEngine.Events;


namespace HW
{
    public class Enemy : MonoBehaviour, ITargetable, IHitable, IMover, IActivable
    {
        enum State { Dead, Idle, Alerted, Engaged }

        public UnityAction OnFight;
        public UnityAction<HitInfo> OnGotHit;

        [Header("Engagement ranges")]
        [SerializeField]
        float proximityRange = 2f; // You are too close ( zero or negative to disable )
        float sqrProximityRange;

        [SerializeField]
        float hearingRange = 4f; // Valid for running and melee attack when you hit enemes ( zero or negative to disable )
        float sqrHearingRange;

        [SerializeField]
        float sightRange = 8f; // They can see you ( zero or negative to disable )
        float sqrSightRange;

        [SerializeField]
        float shotHearingRange = 8f; // Enemy can hear you if you shoot within this distance ( zero or negative to disable )
        float sqrShotHearingRange;

        [Header("Behaviours")]
        [SerializeField]
        MonoBehaviour idleBehaviour;

        [SerializeField]
        MonoBehaviour fightBehaviour;

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
        float sightAngle = 30f;



        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            angularSpeedDefault = agent.angularSpeed;
            ResetSpeed();

            // Ranges
            sqrProximityRange = proximityRange * proximityRange;
            sqrHearingRange = hearingRange * hearingRange;
            sqrSightRange = sightRange * sightRange;
            sqrShotHearingRange = shotHearingRange * shotHearingRange;
           

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

            if(reacting || fighting)
            {
                if (fighting)
                {
                    Vector3 dir = PlayerController.Instance.transform.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();

                    transform.forward = Vector3.RotateTowards(transform.forward, dir, agent.angularSpeed * Time.deltaTime, 0);
                }

                return;
            }


            switch (state)
            {
                case State.Engaged:
                    if (CheckForDisengagement())
                    {
                        Alert(transform.position);
                    }
                    else
                    {
                        // Withing the fighting range
                        if ((transform.position - target.position).magnitude < (fightBehaviour as IFighter).GetFightingRange())
                        {
                            agent.velocity = Vector3.MoveTowards(agent.velocity, Vector3.zero, closeEnoughDeceleration * Time.deltaTime);
                            
                            if (!fighting)
                            {
                                fighting = true;
                                OnFight?.Invoke();
                                agent.ResetPath();
                                
                            }

                        }
                        else // Out of the fighting range
                        {

                            // Move to target
                            if(!IsOccluded(PlayerController.Instance.transform))
                                MoveTo(target.position);
                        }
                    }
                    
                    break;

                case State.Alerted:
                    if (CheckForEngagement())
                    {
                        StopCoroutine(DoAlert());
                        movingAlerted = false;
                        Engage();
                    }

                    break;
                case State.Idle:
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
                state = State.Dead;
                
         
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
            
            // If true there is already a coroutine running
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
            yield return new WaitForSeconds(waitingTimer);

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

            // Start idle mode
            (idleBehaviour as IBehaviour)?.StartBehaving();
        }

        // Returns true if player is engaged otherwise false
        bool CheckForEngagement()
        {

            if ((System.DateTime.UtcNow - lastEngagementCheckTime).TotalSeconds < engagementCheckTimer)
                return false;

            lastEngagementCheckTime = System.DateTime.UtcNow;

            // I should not be here... anyway
            if (state == State.Engaged) 
                return true;

            // Can't be eangaged by dead enemies
            if (IsDead())
                return false;

            // If player is dead disengage
            if (PlayerController.Instance.IsDead())
                return false;

            // Vector to player
            Vector3 toPlayer = PlayerController.Instance.transform.position - transform.position;
            toPlayer.y = 0; // Get rid of the y

            // Distance from player
            float sqrDistance = toPlayer.sqrMagnitude;

            // Is the player within the proximity range ?
            if (sqrProximityRange > 0 && sqrDistance < sqrProximityRange)
                return true;

            if (sqrHearingRange > 0 && sqrDistance < sqrHearingRange && PlayerController.Instance.IsRunning())
                return true;

            if (IsInFieldView(PlayerController.Instance.transform, sightRange, sightAngle) && !IsOccluded(PlayerController.Instance.transform))
                return true;

            return false;
            
        }

        bool CheckForDisengagement()
        {
            // Time to check?
            if ((System.DateTime.UtcNow - lastEngagementCheckTime).TotalSeconds < engagementCheckTimer)
                return false;

         

            // Lets check
            lastEngagementCheckTime = System.DateTime.UtcNow;

            // I should not be here... anyway
            if (state != State.Engaged)
                return true;

           

            // Can't be eangaged by dead enemies
            if (IsDead())
                return true;

            

            // If player is dead disengage
            if (PlayerController.Instance.IsDead())
                return true;



            // Check the last time the enemy saw you
            if (IsInFieldView(PlayerController.Instance.transform, sightRange, sightAngle) && !IsOccluded(PlayerController.Instance.transform))
            {
                lastPlayerOnSight = System.DateTime.UtcNow;
            }
            else
            {
                if ((System.DateTime.UtcNow - lastPlayerOnSight).TotalSeconds > playerLostMaxTime)
                {
                    return true;
                }

            }

            return false;
        }

        // Check for the target to be inside a visual cone given angle and distance 
        bool IsInFieldView(Transform target, float maxDistance, float maxAngle)
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
                if (sqrDistance < sqrHearingRange)
                    Engage();
                    
            }
        }

        // Engages you if you shoot within a given range
        void HandlePlayerOnShoot()
        {
            
            if (state == State.Engaged || state == State.Dead)
                return;

            float sqrDistance = (PlayerController.Instance.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < sqrShotHearingRange)
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
        }

        public void DeadCompleted()
        {
            fighting = false;

            if(GetComponent<Collider>())
                GetComponent<Collider>().enabled = false;
        }

        public void AttackCompleted()
        {
            fighting = false;
        }

        public void Hit()
        {
            (fightBehaviour as IFighter).Fight(PlayerController.Instance.transform);
            
        }
        #endregion





    }

}
