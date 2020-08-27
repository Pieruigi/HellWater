using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HW.Interfaces;

namespace HW.UI
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField]
        Image crosshairImage;
        
        PlayerController playerController;

        Transform target;

        float verticalOffset = 100f;

        private void Awake()
        {
            Show(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            playerController = GameObject.FindObjectOfType<PlayerController>();
            playerController.OnTargeting += HandleOnTargeting;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            if (!IsVisible())
                return;

            crosshairImage.transform.position = Camera.main.WorldToScreenPoint(target.position) + Vector3.up * verticalOffset;
        }

        void Show(bool value)
        {
            
            crosshairImage.enabled = value;
        }

        bool IsVisible()
        {
            return crosshairImage.enabled;
        }
      
        void HandleOnTargeting(Weapon weapon, Transform target)
        {
            if (target)
            {
                if (!IsVisible())
                {
                    Show(true);
                }
                this.target = target;
            }
            else
            {
                if (IsVisible())
                    Show(false);
            }
            
            
        }
    }

}
