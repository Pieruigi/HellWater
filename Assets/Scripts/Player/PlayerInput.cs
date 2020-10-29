using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    public class PlayerInput
    {
        #region CONTROLLER AXIS 
        public static readonly string HorizontalAxis = "Horizontal";
        public static readonly string VerticalAxis = "Vertical";
        public static readonly string SprintAxis = "Run";
        public static readonly string AimAxis = "Aim";
        public static readonly string ReloadAxis = "Reload";
        public static readonly string ShootAxis = "Shoot";
        public static readonly string ActionAxis = "Action";
        public static readonly string BackAxis = "Back";
        public static readonly string InventoryAxis = "Inventory"; // Left button
        public static readonly string EscapeAxis = "Escape";
        public static readonly string QuestAxis = "Quest";
        #endregion

        // Some usefull mappings
        public static bool GetActionButtonDown()
        {
            return GetButtonDown(ActionAxis);
        }

        public static bool GetActionButtonUp()
        {
            return GetButtonUp(ActionAxis);
        }

        public static bool GetBackButtonDown()
        {
            return GetButtonDown(BackAxis);
        }

        public static bool GetBackButtonUp()
        {
            return GetButtonUp(BackAxis);
        }

        public static bool GetSprintButton()
        {
            return GetButton(SprintAxis);
        }

        public static bool GetInventoryButtonDown()
        {
            return GetButtonDown(InventoryAxis);
        }

        public static bool GetInventoryButtonUp()
        {
            return GetButtonUp(InventoryAxis);
        }

        public static float GetHorizontalAxisRaw()
        {
            return GetAxisRaw(HorizontalAxis);
        }

        public static float GetVerticalAxisRaw()
        {
            return GetAxisRaw(VerticalAxis);
        }


        public static float GetAxis(string axis)
        {
            // No suffix for mouse and keyboard
            string suffix = "_0";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetAxis(axis + suffix);
        }

        
        public static float GetAxisRaw(string axis)
        {
            // No suffix for mouse and keyboard
            string suffix = "_0";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetAxisRaw(axis + suffix);
        }

        public static bool GetButtonDown(string button)
        {
            // No suffix for mouse and keyboard
            string suffix = "_0";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetButtonDown(button + suffix);
        }

        public static bool GetButtonUp(string button)
        {
            // No suffix for mouse and keyboard
            string suffix = "_0";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetButtonUp(button + suffix);
        }

        public static bool GetButton(string button)
        {
            // No suffix for mouse and keyboard
            string suffix = "_0";

            // Are we using the gamepad ?
            if (JoystickManager.Instance.Connected)
            {
                // If so which type ?
                suffix = JoystickManager.Instance.Suffix;
            }

            return Input.GetButton(button + suffix);
        }
    }

}
