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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Step()
        {
            // Check is grounded and which type of ground is over
            RaycastHit hit;
            float offset = 0.1f;
            int mask = LayerMask.GetMask(Constants.LayerGround);
            Ray ray = new Ray(transform.position, Vector3.down);

            if(Physics.Raycast(ray,out hit, 2 * offset, mask))
            {
                Debug.Log("HitGround:" + hit.transform);

                // If ground has been hit the play the right step clip
                AudioClipData clipData = GetClipData(hit.transform);

                // Set clip and volume
                audioSource.volume = clipData.Volume;
                audioSource.clip = clipData.Clip;

                // Play sound
                audioSource.Play();
            }
        }

        AudioClipData GetClipData(Transform ground)
        {
            AudioClipData ret = null;
            string tag = ground.tag;
            switch (tag)
            {
                case Tags.GroundConcrete:
                    ret = concreteClips[Random.Range(0, concreteClips.Count)]; 
                    break;
                case Tags.GroundDirt:
                    ret = dirtClips[Random.Range(0, dirtClips.Count)];
                    break;
                case Tags.GroundGrass:
                    ret = grassClips[Random.Range(0, grassClips.Count)];
                    break;
                default:
                    ret = concreteClips[Random.Range(0, concreteClips.Count)];
                    break;

            }

            return ret;
        }
    }

}
