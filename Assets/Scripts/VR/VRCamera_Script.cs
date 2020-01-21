using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCamera_Script : MonoBehaviour
{
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

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft)
               || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft))
        {
            rotAngle.y = rotAngle.y - 45f;
            rot = Quaternion.Euler(rotAngle);

            transform.localPosition = pos;
            transform.localRotation = rot;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight)
       || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight))
        {
            rotAngle.y = rotAngle.y + 45f;
            rot = Quaternion.Euler(rotAngle);

            transform.localPosition = pos;
            transform.localRotation = rot;
        }

    }

    void FixedUpdate()
    {
       
    }
}
