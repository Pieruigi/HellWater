using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class EnemyAnimationController : MonoBehaviour
    {
        // This is the speed that better fit the running animation
        float locomotioMaxSpeed = GameplayUtility.GetMovementSpeedValue(SpeedClass.VeryFast);

        Enemy enemy;
        Animator animator;

        #region ANIMATOR PARAMETERS
        string paramSpeed = "Speed";
        string paramStopReaction = "StopReaction";
        string paramPushReaction = "PushReaction";
        string paramDead = "Dead";
        #endregion

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            animator = GetComponent<Animator>();

            enemy.OnGotHit += HandleOnGotHit;
            
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

    }

}
