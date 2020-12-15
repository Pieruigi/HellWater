using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;

namespace HW
{
    
    public class Picker : MonoBehaviour
    {
        [SerializeField]
        Pickable pickable;

        [SerializeField]
        int amount;

        [SerializeField]
        Transform placeHolderRoot;

        GameObject placeHolder;

        FiniteStateMachine fsm;

        int pickedState = 0;

        private void Awake()
        {
            // Get the fsm attached to this object.
            fsm = GetComponent<FiniteStateMachine>();

            // Set the handle.
            fsm.OnStateChange += HandleOnStateChange;
        }

        // Start is called before the first frame update
        void Start()
        {
            // If the object has not been picked then show the place holder.
            if(fsm.CurrentStateId != pickedState)
            {
                GameObject prefab = pickable.GetPlaceHolderPrefab();
                if (prefab)
                    placeHolder = GeneralUtility.ObjectPopIn(prefab, placeHolderRoot, Vector3.zero, Vector3.zero, Vector3.one);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnStateChange(FiniteStateMachine fsm)
        {
            // State changed to picked.
            if (fsm.CurrentStateId == pickedState)
            {
                // Set the place holder invisible.
                if(placeHolder)
                    GeneralUtility.ObjectPopOut(placeHolder);

                // Pick the object.
                Pick();
            }
                
        }


        void Pick()
        {
            if (IsWeapon())
            {
                PickWeapon();
                return;
            }

            if (IsAmmo())
            {
                PickAmmo();
                return;
            }
            
        }

        bool IsWeapon()
        {
            if (pickable.GetObjectPrefab().GetComponent<Weapon>())
                return true;

            return false;
        }

        bool IsAmmo()
        {
            if (pickable.GetObjectPrefab().GetComponent<Ammonition>())
                return true;

            return false;
        }

        void PickWeapon()
        {
            
            // Which kind of weapon is this?
            Weapon newWeapon = pickable.GetObjectPrefab().GetComponent<Weapon>();

            // Add a reference to the new weapon.
            newWeapon.gameObject.AddComponent<Referer>().SetReference(pickable);
            
            // Handle to the currently equipped weapon, if any.
            Weapon wOld = null;

            // Melee weapon.
            if (newWeapon.GetType() == typeof(MeleeWeapon)) 
            {
                // Get the already equipped weapon if any.                
                wOld = Equipment.Instance.MeleeWeapon;

                // Add the new weapon in the equipment.
                Equipment.Instance.AddMeleeWeapon(newWeapon as MeleeWeapon);
            }

            // Fire weapon.
            if(newWeapon.GetType() == typeof(FireWeapon))
            {
                // Get the already equipped weapon if any.
                if ((newWeapon as FireWeapon).HolsterId == FireWeaponHolsterId.Primary)
                    wOld = Equipment.Instance.PrimaryWeapon;
                else
                    if ((newWeapon as FireWeapon).HolsterId == FireWeaponHolsterId.Secondary)
                        wOld = Equipment.Instance.SecondaryWeapon;

                // Add the new weapon in the equipment.
                Equipment.Instance.AddFireWeapon(newWeapon as FireWeapon);
            }


                
            // Respawn the old weapon.
            //object oldPickable = wOld.GetComponent<Referer>().GetReference();
            //Spawn(oldPickable);

            
        }

        void PickAmmo()
        {

        }




    }

}
