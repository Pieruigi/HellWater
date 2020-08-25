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
        public UnityAction OnAttack;
        public UnityAction OnSoftHit;
        public UnityAction OnHardHit;

        [Header("Engagement ranges")]
        [SerializeField]
        float awarenessRange = 2f; // You are too close ( zero or negative to disable )
        float sqrAwarenessRange;

        [SerializeField]
        float hearingRange = 4f; // Valid for running and melee attack when you hit enemes ( zero or negative to disable )
        float sqrHearingRange;

        [SerializeField]
        float sightingRange = 8f; // They can see you ( zero or negative to disable )
        float sqrSightingRange;

        [SerializeField]
        float shotHearingRange = 8f; // Enemy can hear you if you shoot within this distance ( zero or negative to disable )

        [SerializeField]
        MonoBehaviour idleBehaviour;

        [SerializeField]
        MonoBehaviour fightBehaviour;

        [SerializeField]
        SpeedClass speedClass = SpeedClass.Average;

        #region FIGHTING
        // The target this enemy is looking for ( can be both the player and any NPC )
        Transform target;
        bool isDead = false;
        float attackRange = 1.25f;
        bool reacting = false;
        float pushDistance = .75f;
        float pushSpeed = 4;
        #endregion

        #region MOVEMENT
        public event UnityAction<IMover> OnDestinationReached;
        bool hasDestination = false;
        float closeEnoughDeceleration = 20f;
        float angularSpeedDefault;
        #endregion

        // The maximum speed the ai can even reach
        
        
        bool engaged = false;
        System.DateTime lastEngagementCheckTime;
        float EngagementCheckTimer = 0.2f;
        
        
        NavMeshAgent agent;
        GameObject player;
        bool active = false;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            angularSpeedDefault = agent.angularSpeed;
            ResetSpeed();

            // Ranges
            sqrAwarenessRange = awarenessRange * awarenessRange;
            sqrHearingRange = hearingRange * hearingRange;
            sqrSightingRange = sightingRange * sightingRange;
        }


        // Start is called before the first frame update
        void Start()
        {
            Activate(true);

            player = GameObject.FindGameObjectWithTag(Tags.Player);
            target = player.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!active || isDead)
                return;

            if(reacting)
            {
                return;
            }

            if ((System.DateTime.UtcNow - lastEngagementCheckTime).TotalSeconds > EngagementCheckTimer)
            {
                lastEngagementCheckTime = System.DateTime.UtcNow;
                CheckForEngagement();
            }

            if (engaged)
            {
                if ((transform.position - target.position).magnitude < attackRange)
                {
                    agent.velocity = Vector3.MoveTowards(agent.velocity, Vector3.zero, closeEnoughDeceleration * Time.deltaTime);

                    (fightBehaviour as IFighter)?.Fight();
                }
                else
                {
                    // Move to target
                    MoveTo(target.position);
                }
                
            }
            else
            {

            }
            
            

            // Check if destination has been reached
            if(hasDestination && agent.remainingDistance == 0)
            {
                hasDestination = false;
                OnDestinationReached?.Invoke(this);
            }
                
        }

        #region INTERFACES IMPLEMENTATION
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

        public void Hit(HitInfo hitInfo)
        {
            Debug.Log("Receiving HitInfo:" + hitInfo);

            if (!engaged)
                Engage();

            if(hitInfo.PhysicalReaction != HitPhysicalReaction.None)
            {
                // Set true in order to avoid get target destination
                reacting = true;

                // Reset
                agent.ResetPath();

                
                if (hitInfo.PhysicalReaction == HitPhysicalReaction.Stop)
                {
                    
                    OnSoftHit?.Invoke();
                }
                else
                {
                    // Set push speed
                    agent.speed = pushSpeed;

                    // Move back
                    agent.SetDestination(transform.position - transform.forward * pushDistance);

                    // Avoid enemy to rotate 
                    agent.angularSpeed = 0;

                    OnHardHit?.Invoke();
                }
                    
            }
            
        }

        public void MoveTo(Vector3 destination)
        {
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

        #region PRIVATE
        void Reset()
        {
            engaged = false;
            (idleBehaviour as IBehaviour).StopBehaving();
            ResetSpeed();
        }

        void ResetSpeed()
        {
            agent.speed = GameplayUtility.GetMovementSpeedValue(speedClass);
            agent.angularSpeed = angularSpeedDefault;
        }


        void Engage()
        {
            engaged = true;
            Debug.Log("Engaged");

            // Stop idle mode
            (idleBehaviour as IBehaviour).StopBehaving();

            // Reset speed
            ResetSpeed();
        }

        // Returns true if player is engaged otherwise false
        void CheckForEngagement()
        {
            if (!engaged)
            {
                // Get the largest awareness range
                float range = GetAwarenessLargestRange();

                // Vector to the player
                Vector3 toPlayer = player.transform.position - transform.position;
                toPlayer.y = 0; // Get rid of the y

                
                float sqrDistance = toPlayer.sqrMagnitude;

                if (sqrAwarenessRange > 0 && sqrDistance < sqrAwarenessRange)
                    Engage();

                if (engaged)
                    Debug.Log("Player engaged at distance:" + toPlayer.magnitude);
            }
            else // Already engaged
            {

            }

           
        }

   

        // Get the largest range between awareness, hearing and sighting ranges
        float GetAwarenessLargestRange()
        {
            float r = awarenessRange;
            if (hearingRange > r)
                r = hearingRange;
            if (sightingRange > r)
                r = sightingRange;
            return r;
        }

        void SeekTarget()
        {
            MoveTo(target.position);
            
        }

        #endregion
        // Reset the agent speed to max speed

        #region ANIMATIONS EVENTS
        public void ReactionCompleted()
        {
            ResetSpeed();
            reacting = false;
            
        }
        #endregion





    }

}
