using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class CameraOffsetSwitcher : MonoBehaviour
    {
        [SerializeField]
        Vector3 backOffset;

        [SerializeField]
        Vector3 frontOffset;

        LerpTrigger lerpTrigger;

        private void Awake()
        {
            lerpTrigger = GetComponent<LerpTrigger>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (lerpTrigger.Inside)
            {
                Vector3 offset = Vector3.Lerp(backOffset, frontOffset, lerpTrigger.LerpFactor);
                PlayerCamera.Instance.OffsetFromPlayer = offset;
            }
        }
    }

}
