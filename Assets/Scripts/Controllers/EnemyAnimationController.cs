using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField]
        int fightAnimationCount;

        // This is the speed that better fit the running animation
        float locomotioMaxSpeed = GameplayUtility.GetMovementSpeedValue(SpeedClass.VeryFast);

        Enemy enemy;
        Animator animator;

        #region ANIMATOR PARAMETERS
        string paramSpeed = "Speed";
        string paramStopReaction = "StopReaction";
        string paramPushReaction = "PushReaction";
        string paramDead = "Dead";
        string paramFightingAnimId = "AttackId";
        string paramFight = "Attack";
        string paramFightMultiplier = "AttackMultiplier";
        #endregion

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            animator = GetComponent<Animator>();

            // Set the fighting animiation speed multiplier
            animator.SetFloat(paramFightMultiplier, GameplayUtility.GetAttackAnimationMultiplierValue(enemy.SpeedClass));

            // Set handles
            enemy.OnGotHit += HandleOnGotHit;
            enemy.OnFight += HandleOnFight;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            // Update locomotion
            float ratio = enemy.GetSpeed() / locomotioMaxSpeed;
            animator.SetFloat(paramSpeed, ratio);
        }

        // Handle reaction on hit
        void HandleOnGotHit(HitInfo hitInfo)
        {
            // If there is some reaction
            if(hitInfo.PhysicalReaction != HitPhysicalReaction.None)
            {
                if(hitInfo.PhysicalReaction == HitPhysicalReaction.Stop || enemy.CanNotBePushed)
                    animator.SetTrigger(paramStopReaction); // Just stop
                else
                    animator.SetTrigger(paramPushReaction); // Push
            }

            // If is dead then set param
            if (enemy.IsDead())
                animator.SetBool(paramDead, true);

        }

        // Handle fighting
        void HandleOnFight()
        {
            // If dead do nothing
            if (enemy.IsDead())
                return;

            // We might have different types of animations
            int animationId = Random.Range(0, fightAnimationCount);
            animator.SetFloat(paramFightingAnimId, (float)animationId);

            // Start fighting
            animator.SetTrigger(paramFight);
        }

    }

}
