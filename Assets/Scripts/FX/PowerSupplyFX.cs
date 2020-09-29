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
        FiniteStateMachine fsm;

        [SerializeField]
        ActionController actionController;

        [SerializeField]
        ActionController tankActionController;

        [SerializeField]
        AudioSource tankAudioSource;

        int playOnStateId = 0;

        private void Awake()
        {
            fsm.OnStateChange += HandleOnStateChange;
            actionController.OnActionStart += HandleOnActionStart;
            actionController.OnActionStop += HandleOnActionStop;

            tankActionController.OnActionStart += HandleOnActionStart;
            tankActionController.OnActionStop += HandleOnActionStop;

        }

        // Start is called before the first frame update
        void Start()
        {
            if(fsm.CurrentStateId == playOnStateId) 
            {
                
                StartFX();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm, int oldState)
        {
            if (fsm.CurrentStateId == playOnStateId)
            {
                StartFX();
            }
           
        }

        void StartFX()
        {
            // Play particle system
            particle.Play();

            // Play audio loop 
            workingAudioSource.Play();
        }

        void HandleOnActionStart(ActionController ctrl)
        {
            if (ctrl == actionController)
                startingAudioSource.Play();
            else
                tankAudioSource.PlayDelayed(0.5f);

        }

        void HandleOnActionStop(ActionController ctrl)
        {
            if (ctrl == actionController)
                startingAudioSource.Stop();
            else
                tankAudioSource.Stop();
        }

    }

}
