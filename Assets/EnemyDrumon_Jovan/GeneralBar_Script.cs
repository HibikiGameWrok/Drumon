using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralBar_Script : MonoBehaviour
{

    [SerializeField]
    private Image BarImage = null;

    void Awake()
    {
        SetWaitGauge(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function : SetWaitGauge
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //float _value | Set the scale of the Wait Gauge Bar, 0.0f ~ 1.0f
    //~~~~~~~~~~~~~~~~~~~~~~~~
    //Call this function to change the scale of the Wait Gauge Bar
    public void SetWaitGauge(float _value)
    {
        BarImage.transform.localScale = new Vector3(_value, 1, 1);
    }
}
