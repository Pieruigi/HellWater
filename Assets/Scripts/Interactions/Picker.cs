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
            if(fsm.CurrentStateId != (int)PickableState.Picked && fsm.CurrentStateId > 0)
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
            if (fsm.CurrentStateId == (int)PickableState.Picked)
            {
                Debug.Log("Pick:" + pickable.name);

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
            GameObject newWeapon = GameObject.Instantiate(pickable.GetObjectPrefab());

            // Add a reference to the new weapon.
            newWeapon.AddComponent<Referer>().SetReference(pickable);
            
            // Handle to the currently equipped weapon, if any.
            Weapon wOld = null;

            // Melee weapon.
            if (newWeapon.GetComponent<MeleeWeapon>()) 
            {
                // Get the already equipped weapon if any.                
                wOld = Equipment.Instance.MeleeWeapon;

                // Add the new weapon in the equipment.
                Equipment.Instance.AddMeleeWeapon(newWeapon.GetComponent<MeleeWeapon>());
            }

            // Fire weapon.
            if(newWeapon.GetComponent<FireWeapon>())
            {
                // Get the already equipped weapon if any.
                if (newWeapon.GetComponent<FireWeapon>().HolsterId == FireWeaponHolsterId.Primary)
                    wOld = Equipment.Instance.PrimaryWeapon;
                else
                    if (newWeapon.GetComponent<FireWeapon>().HolsterId == FireWeaponHolsterId.Secondary)
                        wOld = Equipment.Instance.SecondaryWeapon;

                // Add the new weapon in the equipment.
                Equipment.Instance.AddFireWeapon(newWeapon.GetComponent<FireWeapon>());
            }

            // Spawn the old pickable if any.
            // We need to get the reference of the object we want to spawn and attach it to this picker.
            Pickable oldPickable = null;
            Debug.Log("WOld:" + wOld);
            if (wOld)
            {
                // The reference to the pickable.
                oldPickable = (Pickable)wOld.GetComponent<Referer>().GetReference();

                // Set the picker.
                pickable = oldPickable;

                // Reset the finite state machine.
                fsm.ForceState((int)PickableState.NotPicked, false, false);

            }

            Debug.Log("oldPickable:" + oldPickable);

            // Manage place holders.
            StartCoroutine(ManagePlaceHolders(oldPickable));
            
        }

        void PickAmmo()
        {

        }

        IEnumerator ManagePlaceHolders(Pickable old)
        {
            // Destroy the current place holder.
            if (placeHolder)
                GeneralUtility.ObjectPopOut(placeHolder);
                

            // If we are switching from different pickables then we show up the new place holder.
            if (old)
            {
                yield return new WaitForSeconds(Constants.PopInOutTime);

                placeHolder = GeneralUtility.ObjectPopIn(old.GetPlaceHolderPrefab(), placeHolderRoot, Vector3.zero, Vector3.zero, Vector3.one);
            }
            


            
        }


    }

}
