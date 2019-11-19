using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingletonObject : SingletonBase_Script<TestSingletonObject>
{
 
    protected override void Awake()
    {
        base.CheckInstance();

  
    }
    
    // Start is called before the first frame update
    void Start()
    {
       DontDestroyOnLoad(gameObject);
    }
}
