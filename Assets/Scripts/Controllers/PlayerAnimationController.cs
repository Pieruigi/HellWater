using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerAnimationController : MonoBehaviour
    {
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
        string paramChargeAttack = "ChargeAttack";
        string paramAttackOK = "AttackOK";
        string paramAttackKO = "AttackKO";
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
            playerController.OnChargeAttack += HandleOnChargeAttack;
            playerController.OnAttack += HandleOnAttack;
            playerController.OnAttackCharged += HandleOnAttackCharged;

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
            animator.SetFloat(paramSpeed, playerController.GetCurrentSpeed() / playerController.GetMaximumSpeed());

            // If player is aiming then check rotation
            if (animator.GetBool(paramAiming))
            {
                int dir = 0;
                if (playerController.ToTargetSignedAngleRotation != 0)
                    dir = (int)Mathf.Sign(playerController.ToTargetSignedAngleRotation);
                
                if (animator.GetInteger(paramAimingDirection) != dir)
                    animator.SetInteger(paramAimingDirection, dir);
            }

            //if(chargingEvalDir != 0)
            //{
            //    chargingSpeed += chargingEvalDir * chargingEvalSpeed * Time.deltaTime;

            //    animator.speed = Mathf.Clamp(chargingSpeed, chargingSpeedMin, 1);
            //    //Debug.Log("AnimSpeed:" + animator.speed);

            //    if(chargingEvalDir < 0)
            //    {
            //        if (chargingSpeed == chargingSpeedMin)
            //            chargingEvalDir = 0;
            //    }
            //    else
            //    {
            //        if (chargingSpeed == 1)
            //            chargingEvalDir = 0;
            //    }
            //}
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

        void HandleOnShoot()
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

            if (hitInfo.PhysicalReaction == HitPhysicalReaction.Push)
                animator.SetTrigger(paramHit);

            if (playerController.IsDead())
                animator.SetBool(paramDead, true);
        }

        void HandleOnChargeAttack()
        {
            animator.SetTrigger(paramChargeAttack);
        }

        void HandleOnAttack(bool value)
        {
            if(value)
                animator.SetTrigger(paramAttackOK);
            else
                animator.SetTrigger(paramAttackKO);
        }

        void HandleOnAttackCharged(bool value)
        {
            if (value)
                animator.speed = 0.3f;
            else
                animator.speed = 1;
        }
    }

}
