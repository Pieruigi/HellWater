using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class EnemyAnimationController : MonoBehaviour
    {
        
        float locomotioMaxSpeed = GameplayUtility.GetMovementSpeedValue(SpeedClass.VeryFast);

        Enemy enemy;
        Animator animator;

        #region ANIMATOR PARAMETERS
        string paramSpeed = "Speed";
        string paramSoftReact = "SoftReact";
        string paramHardReact = "HardReact";
        #endregion

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            animator = GetComponent<Animator>();

            enemy.OnHardHit += HandleOnHardHit;
            enemy.OnSoftHit += HandleOnSoftHit;
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

            // Adjust global speed 
            //if(enemy.GetCurrentSpeed() > 0)
            //{
            //    animator.speed = enemy.GetSpeed() / locomotionBaseSpeed;
            //}
            //else
            //{
            //    animator.speed = 1;
            //}
        }

        void HandleOnHardHit()
        {
            animator.SetTrigger(paramHardReact);
        }

        void HandleOnSoftHit()
        {
            animator.SetTrigger(paramSoftReact);
        }
    }

}
