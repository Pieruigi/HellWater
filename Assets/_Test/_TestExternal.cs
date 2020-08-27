using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _TestExternal : MonoBehaviour
{
    [SerializeField]
    _TextExternalExt test;
}



[System.Serializable]
public abstract class _TestExternalParent<T> 
{
    [System.Serializable]
    public abstract class IntClass
    {
        [SerializeField]
        int id;

        [SerializeField]
        T action;
    }

    [SerializeField]
    string a;

    //[SerializeField]
    //IntClass<T> intClass;

    //[SerializeField]
    //protected T state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum DoorState { Locked, Unlocked }


[System.Serializable]
public class _TextExternalExt: _TestExternalParent <DoorState>
{
    [System.Serializable]
    public class IntClassChild : IntClass
    {

    }

    [SerializeField]
    IntClassChild intClass;

    [SerializeField]
    DoorState ssss;
}


