using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class MarauderAnimationController : MonoBehaviour
    {
        // This is the speed that better fit the running animation
        float locomotionMaxSpeed = GameplayUtility.GetMovementSpeedValue(SpeedClass.VeryFast);

        Enemy enemy;
        Animator animator;

        #region ANIMATOR PARAMETERS
        string paramSpeed = "Speed";
        string paramAim = "Aim";
        #endregion

        private void Awake()
        {
            // Get the enemy and the animator attached to this game object.
            enemy = GetComponent<Enemy>();
            animator = GetComponent<Animator>();

            // Set handles
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
            float ratio = enemy.GetSpeed() / locomotionMaxSpeed;
            animator.SetFloat(paramSpeed, ratio);
        }

        // Handle fighting
        void HandleOnFight()
        {
            // If dead do nothing
            if (enemy.IsDead())
                return;

            // The marauder and the other human enemies simply target you and you surrender.
            animator.SetTrigger(paramAim);

            
        }

    }

}
