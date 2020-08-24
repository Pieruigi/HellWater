using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    public class MeleeWeaponFX : MonoBehaviour
    {
        [SerializeField]
        AudioClip hitClip;

        [SerializeField]
        float hitVolume = 1;

        [SerializeField]
        AudioClip missClip;

        [SerializeField]
        float missVolume = 1;

        AudioSource audioSource;
        MeleeWeapon weapon;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            weapon = GetComponent<MeleeWeapon>();
            weapon.OnHit += HandleOnHit;
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnHit(bool hitSomething)
        {
            if(hitSomething)
                PlayClip(hitClip, hitVolume);
            else
                PlayClip(missClip, missVolume);
        }

        void PlayClip(AudioClip clip, float volume)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

}
