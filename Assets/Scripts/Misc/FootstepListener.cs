using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    public class FootstepListener : MonoBehaviour
    {
        [SerializeField]
        List<AudioClipData> concreteClips;

        [SerializeField]
        List<AudioClipData> grassClips;

        [SerializeField]
        List<AudioClipData> dirtClips;

        [SerializeField]
        AudioSource audioSource;

        FootstepTrigger currentTrigger;

        GroundType currentGroundType;

        // Start is called before the first frame update
        void Start()
        {
            currentGroundType = LevelManager.Instance.DefaultGroundType;
        }

        // Update is called once per frame
        void Update()
        {

        }

        //public void Step()
        //{
        //    // Check is grounded and which type of ground is over
        //    RaycastHit hit;
        //    float offset = 0.1f;
        //    int mask = LayerMask.GetMask(Constants.LayerGround);
        //    Ray ray = new Ray(transform.position, Vector3.down);

        //    if(Physics.Raycast(ray,out hit, 2 * offset, mask))
        //    {
        //        Debug.Log("HitGround:" + hit.transform);

        //        // If ground has been hit the play the right step clip
        //        AudioClipData clipData = GetClipData();

        //        // Set clip and volume
        //        audioSource.volume = clipData.Volume;
        //        audioSource.clip = clipData.Clip;

        //        // Play sound
        //        audioSource.Play();
        //    }
        //}

        public void Step()
        {
            
            AudioClipData clipData = GetClipData();

            // Set clip and volume
            audioSource.volume = clipData.Volume;
            audioSource.clip = clipData.Clip;

            // Play sound
            audioSource.Play();

        }

        public void EnterFootstepTrigger(FootstepTrigger trigger)
        {
            // Set current ground type and the trigger this object is inside
            currentGroundType = trigger.GroundType;
            currentTrigger = trigger;
        }

        public void ExitFootstepTrigger(FootstepTrigger trigger)
        {
            // It may happen that you enter a trigger and leave onother trigger in the same frame, so we must be sure
            // the trigger we are leaving is the current one, oterwise we do nothing.
            if (trigger != currentTrigger)
                return;

            currentTrigger = null;
            currentGroundType = LevelManager.Instance.DefaultGroundType;
        }

        AudioClipData GetClipData()
        {
            AudioClipData ret = null;
            
            switch (currentGroundType)
            {
                case GroundType.Concrete:
                    ret = concreteClips[Random.Range(0, concreteClips.Count)];
                    break;
                case GroundType.Dirt:
                    ret = dirtClips[Random.Range(0, dirtClips.Count)];
                    break;
                case GroundType.Grass:
                    ret = grassClips[Random.Range(0, grassClips.Count)];
                    break;
            }

            return ret;
        }

        
    }

}
