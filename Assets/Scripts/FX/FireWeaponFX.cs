using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW
{
    public class FireWeaponFX : MonoBehaviour
    {
        [SerializeField]
        AudioClip shootClip;

        [SerializeField]
        float shootVolume = 1;

        [SerializeField]
        AudioClip reloadClip;

        [SerializeField]
        float reloadVolume = 1;

        AudioSource audioSource;
        FireWeapon fireWeapon;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            fireWeapon = GetComponent<FireWeapon>();
            fireWeapon.OnShoot += HandleOnShoot;
            fireWeapon.OnReload += HandleOnReload;
            fireWeapon.OnOutOfAmmo += HandleOnOutOfAmmo;
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
            PlayClip(shootClip, shootVolume);
        }

        void HandleOnReload()
        {
            PlayClip(reloadClip, reloadVolume);
        }

        void HandleOnOutOfAmmo()
        {

        }

        void PlayClip(AudioClip clip, float volume)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

}
