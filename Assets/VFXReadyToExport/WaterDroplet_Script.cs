using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroplet_Script : MonoBehaviour
{

    [SerializeField]
    private GameObject ripple = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<WaterDroplet_Script>()) return;

        GameObject newripple = Instantiate(ripple);
        newripple.transform.position = transform.position + transform.up * 0.1f;
        Destroy(newripple, 0.3f);
        Destroy(gameObject);
    }

}
