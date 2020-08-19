using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW;

public class _TestFighting : MonoBehaviour
{
    [SerializeField]
    FireWeapon fireWeapon;

    [SerializeField]
    MeleeWeapon meleeWeapon;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        fireWeapon = playerController.GetComponentInChildren<FireWeapon>();
        meleeWeapon = playerController.GetComponentInChildren<MeleeWeapon>();

        if (fireWeapon)
        {
            playerController.EquipWeapon(fireWeapon);
            fireWeapon.SetVisible(false);
        }
            

        if (meleeWeapon)
        {
            playerController.EquipWeapon(meleeWeapon);
            meleeWeapon.SetVisible(false);
        }
            

        fireWeapon.AddAmmo(100);
        fireWeapon.Reload();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
