using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW;
using HW.Interfaces;
using HW.Collections;

public class _TestFighting : MonoBehaviour
{
    [SerializeField]
    FireWeapon gun;

    [SerializeField]
    Item gunItem;

    [SerializeField]
    Item gunAmmo;

    [SerializeField]
    MeleeWeapon bat;

    [SerializeField]
    Item batItem;

    [SerializeField]
    Enemy enemy;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        

        playerController = GameObject.FindObjectOfType<PlayerController>();
        gun = playerController.GetComponentInChildren<FireWeapon>();
        bat = playerController.GetComponentInChildren<MeleeWeapon>();

        Equipment.Instance.Add(gunAmmo, 24);
        Equipment.Instance.Add(batItem);
        Equipment.Instance.Add(gunItem);
        

        playerController.EquipWeapon(batItem);
        playerController.EquipWeapon(gunItem);

        //playerController.FireWeapon.Reload();


        //playerController.HolsterWeapon();

        
    }

    // Update is called once per frame
    void Update()
    {
        // Hit player
        if (Input.GetKeyDown(KeyCode.T))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Stop, 20, false);
            playerController.GetComponent<IHitable>().GetHit(hitInfo);
        }

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
