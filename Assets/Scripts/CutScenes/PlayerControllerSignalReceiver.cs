using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW.CutScene
{
    public class PlayerControllerSignalReceiver : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DisableController()
        {
            PlayerController.Instance.SetDisabled(true);
        }

        public void EnableController()
        {
            PlayerController.Instance.SetDisabled(false);
        }
    }

}
