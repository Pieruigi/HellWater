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
        Weapon weapon;
        CapsuleCollider targetCollider;

        float cameraSizeDefault;

        private void Awake()
        {
            cameraSizeDefault = Camera.main.orthographicSize;
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

            crosshairImage.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.forward*targetCollider.height/2f);

            // Set color
            SetColor();
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
                this.weapon = weapon;
                if (target)
                    targetCollider = target.GetComponent<Collider>() as CapsuleCollider;
                
            }
            else
            {
                if (IsVisible())
                    Show(false);
            }
            
            
        }

        void SetColor()
        {
            float distance = (target.position - playerController.transform.position).magnitude;

            FireWeapon fw = weapon as FireWeapon;
            if(fw.TooCloseDistance > distance)
            {
                crosshairImage.color = Color.yellow;
                return;
            }

            if(distance > fw.Range && distance < fw.TooFarDistance)
            {
                crosshairImage.color = Color.yellow;
                return;
            }

            if(distance > fw.TooFarDistance)
            {
                crosshairImage.color = Color.red;
                return;
            }

            crosshairImage.color = Color.green;
        }
    }

}
