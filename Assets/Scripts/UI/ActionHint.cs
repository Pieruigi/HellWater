using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.UI
{

    public class ActionHint : MonoBehaviour
    {
       
        [SerializeField]
        Image hintImage;

        [SerializeField]
        Image backgroundImage;

       
        bool loop = false;
        ActionController activeController;

        private void Awake()
        {
            //ActionController.OnStartActing += HandleOnStartActing;
            //ActionController.OnStopActing += HandleOnStopActing;
            //ActionController.OnActionPerformed += HandleOnActionPerformed;

            Show(false);
        }
        // Start is called before the first frame update
        void Start()
        {
            ActionController[] controllers = GameObject.FindObjectsOfType<ActionController>();
            Debug.Log("Controllers.Length:" + controllers.Length);
            foreach(ActionController controller in controllers)
            {
                controller.OnStartActing += HandleOnStartActing;
                controller.OnStopActing += HandleOnStopActing;
                controller.OnActionPerformed += HandleOnActionPerformed;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!loop)
                return;

            if (activeController.GetType() == typeof(HoldActionController))
            {
                HoldActionController ac = activeController as HoldActionController;
                backgroundImage.fillAmount = ac.Charge;
            }
            else
                if (activeController.GetType() == typeof(RepeatActionController))
                {
                    RepeatActionController ac = activeController as RepeatActionController;
                    backgroundImage.fillAmount = ac.Charge;
            }
            
        }

        void HandleOnStartActing(ActionController controller)
        {

            activeController = controller;
            loop = true;

            Show(true);
        }

        void HandleOnStopActing(ActionController controller)
        {
            loop = false;
            activeController = null;
            Show(false);
        }

        void HandleOnActionPerformed(ActionController controller)
        {

        }

        void Show(bool value)
        {
            
            hintImage.enabled = value;
            backgroundImage.enabled = (value && (activeController.GetType() != typeof(ActionController)));
            backgroundImage.fillAmount = 0;           
        }
    }

}
