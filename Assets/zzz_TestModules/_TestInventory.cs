using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW.Collections;
using HW;

public class _TestInventory : MonoBehaviour
{
    [SerializeField]
    List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (Item item in items)
        //    Inventory.Instance.Add(item);   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            Inventory.Instance.Add(items[0]);
    }
}
