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
        #endregion

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            animator = GetComponent<Animator>();

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

        void HandleOnGotHit(HitInfo hitInfo)
        {
            if(hitInfo.PhysicalReaction != HitPhysicalReaction.None)
            {
                if(hitInfo.PhysicalReaction == HitPhysicalReaction.Stop || enemy.CanNotBePushed)
                    animator.SetTrigger(paramStopReaction);
                else
                    animator.SetTrigger(paramPushReaction);
            }

            if (enemy.IsDead())
                animator.SetBool(paramDead, true);

        }

        void HandleOnFight()
        {
            if (enemy.IsDead())
                return;

            int animationId = Random.Range(0, fightAnimationCount);
            animationId = 0;
            animator.SetFloat(paramFightingAnimId, (float)animationId);
            animator.SetTrigger(paramFight);
        }

    }

}
