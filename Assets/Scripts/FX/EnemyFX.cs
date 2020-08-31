using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using HW.Interfaces;

namespace HW
{
    public class EnemyFX : MonoBehaviour
    {
        [SerializeField]
        AudioSource idleAudioSource;

        [SerializeField]
        AudioSource subIdleAudioSource;

        [SerializeField]
        AudioSource attackAudioSource;

        [SerializeField]
        AudioSource reactionAudioSource;

        [SerializeField]
        List<AudioClip> idleClips;

        [SerializeField]
        List<AudioClip> subIdleClips;

        [SerializeField]
        List<AudioClip> attackClips;

        [SerializeField]
        List<AudioClip> reactionClips;

        Enemy enemy;

        float minIdleRandomTime = 0.5f;
        float maxIdleRandomTime = 0.512f;

        float minSubIdleRandomTime = 3;
        float maxSubIdleRandomTime = 7;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            enemy.OnFight += HandleOnFight;
            enemy.OnGotHit += HandleOnGotHit;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (enemy.IsDead())
            {
                idleAudioSource.Stop();
                subIdleAudioSource.Stop();
                return;
            }

            CheckIdle();
            CheckSubIdle();
        }

        void CheckIdle()
        {
            // Idle is playing 
            if (idleAudioSource.isPlaying)
                return;

            // Get random clip and play delayed
            idleAudioSource.clip = idleClips[Random.Range(0, idleClips.Count)];
            idleAudioSource.PlayDelayed(Random.Range(minIdleRandomTime, maxIdleRandomTime));
        }

        void CheckSubIdle()
        {
            // Idle is playing 
            if (subIdleAudioSource.isPlaying)
                return;

            // Get random clip and play delayed
            subIdleAudioSource.clip = subIdleClips[Random.Range(0, subIdleClips.Count)];
            subIdleAudioSource.PlayDelayed(Random.Range(minSubIdleRandomTime, maxSubIdleRandomTime));
        }

        void HandleOnFight()
        {
            attackAudioSource.clip = attackClips[Random.Range(0, attackClips.Count)];
            attackAudioSource.Play();
        }

        void HandleOnGotHit(HitInfo hitInfo)
        {
            reactionAudioSource.clip = reactionClips[Random.Range(0, reactionClips.Count)];
            reactionAudioSource.Play();
        }
    }

}
