using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoPeckTest_Script : MonoBehaviour
{

    public bool Peck = false;

    [SerializeField]
    private GameObject VFXControl_Peck = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Peck && transform.GetComponentInChildren<VFXControl_Peck_Script>() == null)
        {
            Peck = false;
            Instantiate(VFXControl_Peck, transform);
        }
    }
}
