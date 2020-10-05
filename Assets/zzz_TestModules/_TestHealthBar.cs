using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.UI;
using HW;

public class _TestHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerController.Instance.GetComponent<Health>().Damage(20);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerController.Instance.GetComponent<Health>().Heal(10);
        }
    }
}
