using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerAnimationController : MonoBehaviour
    {
        enum Locomotion { Idle, Walk, Run }

        [SerializeField]
        int attackAnimationCount = 1;

        PlayerController playerController;
        Animator animator;

        float chargingSpeed = 1;
        float chargingSpeedMin = 0.1f;
        float chargingEvalDir = 0;
        float chargingEvalSpeed = 10;

        #region ANIMATION PARAMS
        string paramWeaponAnimationId = "WeaponAnimationId";
        string paramSpeed = "Speed";
        string paramAiming = "Aiming";
        string paramAimingDirection = "AimingDirection";
        string paramShoot = "Shoot";
        string paramReload = "Reload";
        string paramHit = "Hit";
        string paramDead = "Dead";
        string paramAttack = "Attack";
        string paramAttackId = "AttackId";
        //string paramLocomotion = "Locomotion";
        
        #endregion

        private void Awake()
        {
            // Get the player controller
            playerController = GetComponent<PlayerController>();

            // Set handles
            playerController.OnSetCurrentWeapon += HandleOnSetCurrentWeapon;
            playerController.OnResetCurrentWeapon += HandleOnResetCurrentWeapon;
            playerController.OnStartAiming += HandleOnStartAiming;
            playerController.OnStopAiming += HandleOnStopAiming;
            playerController.OnShoot += HandleOnShoot;
            playerController.OnReload += HandleOnReload;
            playerController.OnGotHit += HandleOnGotHit;
            playerController.OnAttack += HandleOnAttack;
            
            animator = GetComponentInChildren<Animator>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {

            // Update locomotion
         
             
            // Set animation speed
            animator.SetFloat(paramSpeed, playerController.GetCurrentSpeedNormalized());

            // If player is aiming then check rotation
            if (animator.GetBool(paramAiming))
            {
                int dir = 0;
                if (playerController.ToTargetSignedAngleRotation != 0)
                    dir = (int)Mathf.Sign(playerController.ToTargetSignedAngleRotation);
                
                if (animator.GetInteger(paramAimingDirection) != dir)
                    animator.SetInteger(paramAimingDirection, dir);
            }

        }


        void HandleOnSetCurrentWeapon(Weapon weapon)
        {
            animator.SetFloat(paramWeaponAnimationId, (float)weapon.AnimationId);
        }

        void HandleOnResetCurrentWeapon()
        {
            animator.SetFloat(paramWeaponAnimationId, 0);
        }

        void HandleOnStartAiming()
        {
            animator.SetBool(paramAiming, true);
        }

        void HandleOnStopAiming()
        {
            animator.SetBool(paramAiming, false);
        }

        void HandleOnShoot(Weapon weapon)
        {
            animator.SetTrigger(paramShoot);
        }

        void HandleOnReload()
        {
            animator.SetTrigger(paramReload);
        }

        void HandleOnGotHit(HitInfo hitInfo)
        {
            Debug.Log("OnHit.................");

            if (hitInfo.PhysicalReaction != HitPhysicalReaction.None)
                animator.SetTrigger(paramHit);

            if (PlayerController.Instance.IsDead())
                animator.SetBool(paramDead, true);
        }



        void HandleOnAttack(bool value)
        {
            animator.SetInteger(paramAttackId, Random.Range(0, attackAnimationCount));
            animator.SetTrigger(paramAttack);
        }

    }

}
