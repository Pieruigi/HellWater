using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    public class FireWeaponFX : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField]
        AudioClip shootClip;

        [SerializeField]
        float shootVolume = 1;

        [SerializeField]
        AudioClip reloadClip;

        [SerializeField]
        float reloadVolume = 1;

        [SerializeField]
        AudioClip outOfAmmoClip;

        [SerializeField]
        float outOfAmmoVolume = 1;

        [Header("Particle Systems")]
        [SerializeField]
        ParticleSystem shootPS;

        [SerializeField]
        Transform shootPoint;

        AudioSource audioSource;
        FireWeapon fireWeapon;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            fireWeapon = GetComponent<FireWeapon>();
            fireWeapon.OnShoot += HandleOnShoot;
            fireWeapon.OnReload += HandleOnReload;
            fireWeapon.OnOutOfAmmo += HandleOnOutOfAmmo;
            fireWeapon.OnReloadInterrupted += HandleOnReloadInterrupted;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnShoot()
        {
            // Play audio
            PlayClip(shootClip, shootVolume);

            // Play particle system
            if (shootPS)
            {
                ParticleSystem ps = GameObject.Instantiate(shootPS);
                ps.transform.position = shootPoint.position;
                ps.transform.rotation = shootPoint.rotation;
                ps.transform.localScale = shootPoint.localScale;
                //ps.transform.parent = shootPoint;
                
                ps.Play();
            }
        }

        void HandleOnReload()
        {
            if (IsPlaying(reloadClip))
                return;
            PlayClip(reloadClip, reloadVolume);
        }

        void HandleOnOutOfAmmo()
        {
            PlayClip(outOfAmmoClip, outOfAmmoVolume);
        }

        void HandleOnReloadInterrupted()
        {
            audioSource.Stop();
        }

        void PlayClip(AudioClip clip, float volume)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }

        bool IsPlaying(AudioClip clip)
        {
            if(audioSource.clip == null || audioSource.clip != clip || !audioSource.isPlaying)
                return false;

            return true;
        }
    }

}
