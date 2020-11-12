using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;

namespace HW
{
    public class PlayerFX : MonoBehaviour
    {
        [SerializeField]
        GameObject audioListenerPrefab;

        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        List<AudioClip> reactionClips;

        [SerializeField]
        AudioClip deathClip;

        PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            playerController.OnGotHit += HandleOnGotHit;
            playerController.OnDead += HandleOnDead;

            // Create the audio listener.
            GameObject al = GameObject.Instantiate(audioListenerPrefab);
            ConstraintSource cs = new ConstraintSource();
            cs.sourceTransform = transform;
            cs.weight = 1;
            al.GetComponent<PositionConstraint>().SetSource(0, cs);
            al.GetComponent<PositionConstraint>().constraintActive = true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnGotHit(HitInfo hitInfo)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = reactionClips[Random.Range(0, reactionClips.Count)];
            audioSource.Play();
        }

        void HandleOnDead()
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = deathClip;
            audioSource.Play();
        }
    }

}
