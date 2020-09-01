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
        List<AudioClip> idleClips;

        [SerializeField]
        List<AudioClip> subIdleClips;

        [SerializeField]
        List<AudioClip> attackClips;

        [SerializeField]
        List<AudioClip> reactionClips;

        [SerializeField]
        List<float> idleVolumes;

        [SerializeField]
        List<float> subIdleVolumes;

        [SerializeField]
        List<float> attackVolumes;

        [SerializeField]
        List<float> reactionVolumes;

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
            int r = Random.Range(0, idleClips.Count);

            idleAudioSource.clip = idleClips[r];
            idleAudioSource.volume = idleVolumes[r];
            idleAudioSource.PlayDelayed(Random.Range(minIdleRandomTime, maxIdleRandomTime));
        }

        void CheckSubIdle()
        {
            // Idle is playing 
            if (subIdleAudioSource.isPlaying)
                return;

            // Get random clip and play delayed
            int r = Random.Range(0, subIdleClips.Count);

            subIdleAudioSource.clip = subIdleClips[r];
            subIdleAudioSource.volume = subIdleVolumes[r];

            subIdleAudioSource.PlayDelayed(Random.Range(minSubIdleRandomTime, maxSubIdleRandomTime));
        }

        void HandleOnFight()
        {
            int r = Random.Range(0, attackClips.Count);

            attackAudioSource.clip = attackClips[r];
            attackAudioSource.volume = attackVolumes[r];
            attackAudioSource.Play();
        }

        void HandleOnGotHit(HitInfo hitInfo)
        {
            if (attackAudioSource.isPlaying)
                attackAudioSource.Stop();

            int r = Random.Range(0, reactionClips.Count);

            attackAudioSource.clip = reactionClips[r];
            attackAudioSource.volume = reactionVolumes[r];
            attackAudioSource.Play();
        }
    }

}
