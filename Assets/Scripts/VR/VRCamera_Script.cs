﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCamera_Script : MonoBehaviour
{
    float angle = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        Quaternion rot = transform.localRotation;

        Vector3 rotAngle = rot.eulerAngles;

        //if(OVRInput.GetDown(OVRInput.Button.One))
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown)
            || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown))
        {
            rotAngle.y = rotAngle.y + 180f;
            rot = Quaternion.Euler(rotAngle);

            transform.localPosition = pos;
            transform.localRotation = rot;
        }

    }

    void FixedUpdate()
    {
       
    }
}
