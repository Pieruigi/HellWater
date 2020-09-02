using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _TestExternal : MonoBehaviour
{
    [SerializeField]
    _TextExternalExt test;
}


[System.Serializable]
public abstract class ExtClass<T>
{
    [SerializeField]
    int id;

    [SerializeField]
    T action;

    public abstract bool Check();
}

[System.Serializable]
public abstract class _TestExternalParent<T,T2> where T2: ExtClass<T>
{
    

    [SerializeField]
    string a;

    [SerializeField]
    T2 extClass;

    
    //[SerializeField]
    //protected T state;

    // Start is called before the first frame update
    void Start()
    {
        extClass.Check();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum DoorState { Locked, Unlocked }

[System.Serializable]
public class ExtClassExt: ExtClass<DoorState>
{
    
    public override bool Check()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class _TextExternalExt: _TestExternalParent <DoorState, ExtClassExt>
{

}


