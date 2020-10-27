using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class LightDistanceTrigger : MonoBehaviour
    {
        [SerializeField]
        float distance = 10f;

        float sqrDistance;

        Transform target;

        bool inside = true;
        Light _light;
        float defaultLightIntensity;
        float time = 1f;

        int ltId;

        private void Awake()
        {
            sqrDistance = distance * distance;
            _light = GetComponent<Light>();
            defaultLightIntensity = _light.intensity;
        }

        // Start is called before the first frame update
        void Start()
        {


            target = PlayerController.Instance.transform;
            if ((transform.position - target.position).sqrMagnitude < sqrDistance)
                SetInside(true);
            else
                SetInside(false);
        }

        // Update is called once per frame
        void Update()
        {
            float sqrCurrentDistance = (transform.position - target.position).sqrMagnitude;
            if (sqrCurrentDistance < sqrDistance)
            {
                if(!inside)
                    SetInside(true);
            }
            else
            {
                if(inside)
                    SetInside(false);
            }
                
            
        }

        void SetInside(bool value)
        {
            inside = value;
            float currentIntensity = _light.intensity;
            //if (LeanTween.isTweening(ltId))
            //    LeanTween.removeTween(ltId);

            if(value)
            {
                ltId = LeanTween.value(currentIntensity, defaultLightIntensity, time).setOnUpdate(HandleOnUpdate).id;
            }
            else
            {
                ltId = LeanTween.value(currentIntensity, 0, time).setOnUpdate(HandleOnUpdate).id;
            }
        }

        void HandleOnUpdate(float value)
        {
            _light.intensity = value;
        }
    }

}
