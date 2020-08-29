using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HW.UI
{

    public class ActionHint : MonoBehaviour
    {
        enum ActionType { Press, Hold, Repeat };
        
        [SerializeField]
        Image hintImage;

        ActionType actionType;

        private void Awake()
        {
            ActionController.OnStartActing += HandleOnStartActing;
            ActionController.OnStopActing += HandleOnStopActing;

            Show(false);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStartActing(ActionController controller)
        {
            if (controller.GetType() == typeof(ActionController))
                actionType = ActionType.Press;
            else if (controller.GetType() == typeof(HoldActionController))
                actionType = ActionType.Hold;
            Show(true);
        }

        void HandleOnStopActing(ActionController controller)
        {
            Show(false);
        }

        void Show(bool value)
        {
            hintImage.enabled = value;
        }
    }

}
