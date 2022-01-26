using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW;
using HW.Interfaces;

public class _TestFighting : MonoBehaviour
{
    [SerializeField]
    FireWeapon gun;

    [SerializeField]
    MeleeWeapon bat;

    [SerializeField]
    Enemy enemy;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        

        playerController = GameObject.FindObjectOfType<PlayerController>();
        gun = playerController.GetComponentInChildren<FireWeapon>();
        bat = playerController.GetComponentInChildren<MeleeWeapon>();

        if (gun)
        {
            playerController.EquipWeapon(gun);
            gun.AddAmmo(18);
            gun.Reload();
        }
            

        if (bat)
        {
            playerController.EquipWeapon(bat);
        }

        playerController.HolsterWeapon();

        
    }

    // Update is called once per frame
    void Update()
    {
        // Hit player
        if (Input.GetKeyDown(KeyCode.T))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Push, 20, false);
            playerController.GetComponent<IHitable>().GetHit(hitInfo);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            playerController.HolsterWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            playerController.EquipWeapon(bat);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            playerController.EquipWeapon(gun);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Push, 20, false);
            enemy.GetComponent<IHitable>().GetHit(hitInfo);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Stop, 20, false);
            enemy.GetComponent<IHitable>().GetHit(hitInfo);
        }
    }
}
