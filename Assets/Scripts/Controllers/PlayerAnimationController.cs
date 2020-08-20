using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerAnimationController : MonoBehaviour
    {
        PlayerController playerController;
        Animator animator;

        #region ANIMATION PARAMS
        string paramWeaponAnimationId = "WeaponAnimationId";
        string paramSpeed = "Speed";
        string paramAiming = "Aiming";
        string paramAimingDirection = "AimingDirection";
        string paramShoot = "Shoot";
        string paramReload = "Reload";
        string paramHit = "Hit";
        string paramDie = "Die";
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
            playerController.OnHit += HandleOnHit;
            playerController.OnChargeAttack += HandleOnChargeAttack;
            playerController.OnAttack += HandleOnAttack;

            animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateLocomotion();

            // If player is aiming then check rotation
            if(animator.GetBool(paramAiming))
            {
                int dir = 0;
                if (playerController.ToTargetSignedAngleRotation != 0)
                    dir = (int)Mathf.Sign(playerController.ToTargetSignedAngleRotation);

                if (animator.GetInteger(paramAimingDirection) != dir)
                    animator.SetInteger(paramAimingDirection, dir);
            }
        }

        void UpdateLocomotion()
        {
            animator.SetFloat(paramSpeed, playerController.GetCurrentSpeed() / playerController.GetMaximumSpeed());
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

        void HandleOnHit(HitInfo hitInfo)
        {
            
            if (hitInfo.PhysicalReaction == HitPhysicalReaction.Push)
                animator.SetTrigger(paramHit);

            if (playerController.IsDead())
                animator.SetTrigger(paramDie);
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
    }

}
