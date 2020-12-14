using HW.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HW
{
    
    public class Picker : MonoBehaviour
    {
        [SerializeField]
        GameObject pickableObject;

        [SerializeField]
        int amount;

        [SerializeField]
        GameObject placeHolder;

        FiniteStateMachine fsm;

        int pickedState = 0;

        private void Awake()
        {
            // Set the finite state machine.
            fsm = GetComponent<FiniteStateMachine>();
            fsm.OnStateChange += HandleOnStateChange;

            // If the place holder is empty then we try to read it from the IPickable.
            if (!placeHolder)
                placeHolder = pickableObject.GetComponent<IPickable>().GetPlaceHolder();
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // If the object has already been picked then hide the place holder.
            if(fsm.CurrentStateId == pickedState)
            {
                if (placeHolder)
                    GeneralUtility.ObjectPopOut(placeHolder);
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
            if (pickableObject.GetComponent<IPickable>().GetObjectPrefab().GetComponent<Weapon>())
                return true;

            return false;
        }

        bool IsAmmo()
        {
            if (pickableObject.GetComponent<IPickable>().GetObjectPrefab().GetComponent<Ammonition>())
                return true;

            return false;
        }

        void PickWeapon()
        {
            // Get the interface.
            IPickable pickable = pickableObject.GetComponent<IPickable>();

            // Which kind of weapon is this?
            Weapon newWeapon = pickable.GetObjectPrefab().GetComponent<Weapon>();

            // Add a reference to the new weapon.
            newWeapon.gameObject.GetComponent<Referer>().SetReference(pickable);

            // Handle to the currently equipped weapon, if any.
            GameObject wOld = null;

            // Melee weapon.
            if (newWeapon.GetType() == typeof(MeleeWeapon)) 
            {
                // Get the already equipped weapon if any.                
                if(Equipment.Instance.MeleeWeapon)
                    wOld = Equipment.Instance.MeleeWeapon.gameObject;

                // Add the new weapon in the equipment.
                Equipment.Instance.AddMeleeWeapon(newWeapon as MeleeWeapon);
            }

            // Fire weapon.
            if(newWeapon.GetType() == typeof(FireWeapon))
            {
                // Get the already equipped weapon if any.
                if ((newWeapon as FireWeapon).HolsterId == FireWeaponHolsterId.Primary)
                    wOld = Equipment.Instance.PrimaryWeapon.gameObject;
                else
                    if ((newWeapon as FireWeapon).HolsterId == FireWeaponHolsterId.Secondary)
                        wOld = Equipment.Instance.SecondaryWeapon.gameObject;

                // Add the new weapon in the equipment.
                Equipment.Instance.AddFireWeapon(newWeapon as FireWeapon);
            }


                
            // Respawn the old weapon.
            //object oldPickable = wOld.GetComponent<Referer>().GetReference();
            //Spawn(oldPickable);

            
        }

        void PickAmmo(GameObject objectPrefab)
        {

        }



         if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject wOld = Equipment.Instance.MeleeWeapon ? Equipment.Instance.MeleeWeapon.gameObject : null;


        // Create the object
        GameObject wObj = GameObject.Instantiate(bat.gameObject);

        // Equip 
        Equipment.Instance.AddMeleeWeapon(wObj.GetComponent<Weapon>() as MeleeWeapon);

            // I should read what type of prefab the old weapon refer to and spawn it.
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Create the weapon object.
            GameObject wObj = GameObject.Instantiate(gun.gameObject);

    Weapon wOld = null;
            // Check the holster id.
            if (wObj.GetComponent<FireWeapon>().HolsterId == FireWeaponHolsterId.Primary)
                wOld = Equipment.Instance.PrimaryWeapon;
            else
                if (wObj.GetComponent<FireWeapon>().HolsterId == FireWeaponHolsterId.Secondary)
                    wOld = Equipment.Instance.SecondaryWeapon;

            Equipment.Instance.AddFireWeapon(wObj.GetComponent<FireWeapon>());

        }


    }

}
