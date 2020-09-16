using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class DoorFX : MonoBehaviour
    {
        [SerializeField]
        AudioClipData opendClip;

        [SerializeField]
        AudioClipData closeClip;

        [SerializeField]
        AudioClipData stillLockedClip;

        [SerializeField]
        AudioClipData unlockClip;

        DoorController ctrl;
        AudioSource source;

        private void Awake()
        {
            ctrl = GetComponent<DoorController>();

            ctrl.OnClose += HandleOnClose;
            ctrl.OnOpen += HandleOnOpen;
            ctrl.OnStillLocked += HandleOnStillLocked;
            ctrl.OnUnlock += HandleOnUnlock;

            source = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnClose(DoorController controller)
        {
            PlayClip(closeClip);
        }

        void HandleOnOpen(DoorController controller)
        {
            PlayClip(opendClip);
        }

        void HandleOnStillLocked(DoorController controller)
        {
            PlayClip(stillLockedClip);
        }

        void HandleOnUnlock(DoorController controller)
        {
            PlayClip(unlockClip);
        }

        void PlayClip(AudioClipData data)
        {
            if (data.Clip == null)
                return;

            source.volume = data.Volume;
            source.clip = data.Clip;
            source.Play();
        }
    }

}
