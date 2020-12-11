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
    MeleeWeapon bat;

    [SerializeField]
    Enemy enemy;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        

        playerController = GameObject.FindObjectOfType<PlayerController>();
        //gun = playerController.GetComponentInChildren<FireWeapon>();
        //bat = playerController.GetComponentInChildren<MeleeWeapon>();

        //Equipment.Instance.AddAmmonition((int)gun.AmmonitionType, 24);
        //Equipment.Instance.AddWeapon(bat);
        //Equipment.Instance.AddWeapon(gun);

        //playerController.FireWeapon.Reload();


        //playerController.HolsterWeapon();

        
    }

    // Update is called once per frame
    void Update()
    {
        // Hit player
        if (Input.GetKeyDown(KeyCode.T))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Stop, 20);
            playerController.GetComponent<IHitable>().GetHit(hitInfo);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Push, 20);
            enemy.GetComponent<IHitable>().GetHit(hitInfo);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            HitInfo hitInfo = new HitInfo(playerController.transform.position, Vector3.forward, HitPhysicalReaction.Stop, 20);
            enemy.GetComponent<IHitable>().GetHit(hitInfo);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Equipment.Instance.MeleeWeapon != bat)
            {
                Equipment.Instance.AddMeleeWeapon(bat);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Equipment.Instance.PrimaryWeapon != gun)
            {
                Equipment.Instance.AddPrimaryFireWeapon(gun);
            }
            else
            {
                Equipment.Instance.AddAmmonition((int)gun.AmmonitionType, 10);
            }
        }
        
    }
}
