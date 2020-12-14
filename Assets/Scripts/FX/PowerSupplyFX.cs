using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PowerSupplyFX : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem particle;
      
        [SerializeField]
        AudioSource workingAudioSource;

        [SerializeField]
        AudioSource startingAudioSource;

        [SerializeField]
        FiniteStateMachine starterFsm;

        [SerializeField]
        ActionController actionController;

        [SerializeField]
        ActionController tankActionController;

        [SerializeField]
        AudioSource tankAudioSource;

        [SerializeField]
        FiniteStateMachine tankFsm;

        int playOnStateId = 0;
        float workingDelay = 0;
        float startingDelay = 0.5f;

        private void Awake()
        {
            starterFsm.OnStateChange += HandleOnStateChange;
            tankFsm.OnStateChange += HandleOnStateChange;
            actionController.OnActionPerformed += HandleOnActionPerformed;
            //tankActionController.OnActionPerformed += HandleOnActionPerformed;

        }

        // Start is called before the first frame update
        void Start()
        {
            if(starterFsm.CurrentStateId == playOnStateId) 
            {
                StartFX();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm == starterFsm && fsm.CurrentStateId == playOnStateId)
            {
                workingDelay = 3f;

                StartFX();
            }

            if(fsm == tankFsm && fsm.PreviousStateId == 1 && fsm.CurrentStateId == 0)
            {
                tankAudioSource.Play();
            }
           
        }

        void HandleOnActionPerformed(ActionController ctrl)
        {
            if(ctrl == actionController)
            {
                startingAudioSource.PlayDelayed(startingDelay);
            }
                        
        }

        void StartFX()
        {
            if(workingDelay > 0)
            {
                // We add some delay 
                ParticleSystem.MainModule main = particle.main;
                main.startDelay = 3f;

                // Play sound delayed
                workingAudioSource.PlayDelayed(workingDelay);
            }
            else
            {
                // Play audio loop 
                workingAudioSource.Play();
            }

            // Play particle system
            particle.Play();
        }

        //void HandleOnActionStart(ActionController ctrl)
        //{
        //    if (ctrl == actionController)
        //        startingAudioSource.Play();
        //    else
        //        tankAudioSource.PlayDelayed(0.5f);

        //}

        //void HandleOnActionStop(ActionController ctrl)
        //{
        //    if (ctrl == actionController)
        //        startingAudioSource.Stop();
        //    else
        //        tankAudioSource.Stop();
        //}

    }

}
