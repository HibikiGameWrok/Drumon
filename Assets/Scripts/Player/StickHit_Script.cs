using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHit_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
    }
}
