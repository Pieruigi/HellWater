using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW;

public class _TestInventory : MonoBehaviour
{
    [SerializeField]
    List<Item> items;

    int addStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (Item item in items)
        //    Inventory.Instance.Add(item);   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Inventory.Instance.Add(items[0]);
            Inventory.Instance.Add(items[1]);
            Inventory.Instance.Add(items[2]);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Inventory.Instance.Remove(items[0]);
        }


    }
}
