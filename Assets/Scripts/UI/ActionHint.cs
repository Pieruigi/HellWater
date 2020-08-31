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
        ActionController actionController;

        private void Awake()
        {
            ActionController.OnStartActing += HandleOnStartActing;
            ActionController.OnStopActing += HandleOnStopActing;
            ActionController.OnActionPerformed += HandleOnActionPerformed;

            Show(false);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!loop)
                return;

            if (actionController.GetType() == typeof(HoldActionController))
            {
                HoldActionController ac = actionController as HoldActionController;
                backgroundImage.fillAmount = ac.Charge;
            }
            else
                if (actionController.GetType() == typeof(RepeatActionController))
                {
                    RepeatActionController ac = actionController as RepeatActionController;
                    backgroundImage.fillAmount = ac.Charge;
            }
            
        }

        void HandleOnStartActing(ActionController controller)
        {
            actionController = controller;
            loop = true;

            Show(true);
        }

        void HandleOnStopActing(ActionController controller)
        {
            loop = false;
            actionController = null;
            Show(false);
        }

        void HandleOnActionPerformed(ActionController controller)
        {

        }

        void Show(bool value)
        {
            
            hintImage.enabled = value;
            backgroundImage.enabled = (value && (actionController.GetType() != typeof(ActionController)));
            backgroundImage.fillAmount = 0;           
        }
    }

}
