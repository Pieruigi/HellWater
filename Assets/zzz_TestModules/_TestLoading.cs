using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HW;

public class _TestLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Loading)
            return;


        if (Input.GetKeyDown(KeyCode.A))
        {
            if (GameManager.Instance.InGame)
                GameManager.Instance.LoadMainMenu();
            else
                GameManager.Instance.StartNewGame();
        }
    }
}
