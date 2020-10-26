using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HW
{
    /**
     * Multiple gamepad support.
     * */
    public class JoystickManager : MonoBehaviour
    {
       
        public static JoystickManager Instance { get; private set; }

        float checkTime = 1;
        DateTime lastCheck;

        // Is gamepad connetced?
        bool connected = false;
        public bool Connected
        {
            get { return connected; }
        }
        string suffix = "";
        public string Suffix
        {
            get { return suffix; }
        }

        // Has trigger or buttons ( L2, R2 for playstation; LT, RT for xbox )
        bool hasTriggers;
        public bool HasTriggers
        {
            get { return hasTriggers; }
        }

        string internalName;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if((DateTime.UtcNow - lastCheck).TotalSeconds > checkTime)
            {
                lastCheck = DateTime.UtcNow;
                CheckJoystick();
            }
        }

       
        void SetSuffix()
        {
            if (!connected)
            {
                suffix = "";
                return;
            }
                

            if (internalName == null || "".Equals(internalName))
            {
                suffix = "";
                return;
            }
                

            // XBox series
            if (internalName.ToLower().Contains("xbox "))
            {
                suffix = "_1";
                return;
            }

            //if (internalName.ToLower().Contains("playstation"))
            //{
            //    suffix = "_3";
            //    return;
            //}

            // Alfred's gamepad ( XTR94270 ) 
            if (internalName.ToLower().Contains("generic") && internalName.ToLower().Contains("usb") && internalName.ToLower().Contains("joystick"))
            {
                suffix = "_2";
                return;
            }
                

            // Not recognised, we return xbox controller as default
            suffix = "_1"; 
        }

        void SetTriggers()
        {
            if (!connected)
            {
                hasTriggers = false;
                return;
            }


            if (internalName == null || "".Equals(internalName))
            {
                hasTriggers = false;
                return;
            }


            // XBox series
            if (internalName.ToLower().Contains("xbox "))
            {
                hasTriggers = true;
                return;
            }

            // Alfred's gamepad ( XTR94270 ) 
            if (internalName.ToLower().Contains("generic") && internalName.ToLower().Contains("usb") && internalName.ToLower().Contains("joystick"))
            {
                hasTriggers = false;
                return;
            }
        }

        void CheckJoystick()
        {
            // No joystick initialized
            bool init = !connected;

            connected = (Input.GetJoystickNames().Length > 0 && !"".Equals(Input.GetJoystickNames()[0].Trim()));

            if(connected && init)
            {
                Init();
            }
        }

        

        void SetInternalName()
        {
            internalName = Input.GetJoystickNames()[0];
            Debug.Log("JoystickName:" + internalName);
        }

        void Init()
        {
            SetInternalName();
            SetSuffix();
            SetTriggers();
        }
    }

}
