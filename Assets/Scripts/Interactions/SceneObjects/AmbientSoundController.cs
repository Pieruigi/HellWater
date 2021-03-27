using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class AmbientSoundController : MonoBehaviour
    {
        [SerializeField]
        AudioClip clip;

        [SerializeField]
        float volume;

        FiniteStateMachine fsm;

        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            if(fsm.CurrentStateId == 0)
                AmbientAudioManager.Instance.SwitchAmbientSource(clip, volume, 10);
        }
    }

}
