using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HW.UI
{
    public class InventoryFX : MonoBehaviour
    {
        

        [SerializeField]
        AudioClipData enterClip;

        [SerializeField]
        AudioClipData exitClip;

        [SerializeField]
        AudioClipData selectionClip;

        AudioSource audioSource;


        private void Awake()
        {
            audioSource = GetComponentInParent<AudioSource>();
            InventoryUI ui = GetComponent<InventoryUI>();
            ui.OnOpen += HandleOnOpen;
            ui.OnClose += HandleOnClose;
            ui.OnSelect += HandleOnSelect;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnOpen()
        {
            audioSource.clip = enterClip.Clip;
            audioSource.volume = enterClip.Volume;
            audioSource.Play();
        }

        void HandleOnClose()
        {
            audioSource.clip = exitClip.Clip;
            audioSource.volume = exitClip.Volume;
            audioSource.Play();
        }

        void HandleOnSelect(ItemUI itemUI)
        {
            audioSource.clip = selectionClip.Clip;
            audioSource.volume = selectionClip.Volume;
            audioSource.Play();
        }
    }

}
