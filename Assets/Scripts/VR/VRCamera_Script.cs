/*----------------------------------------------------------*/
//  file:      VRCamera_Script.cs                                   |
//				 											                    |
//  brief:    カメラ操作のスクリプト			                    |
//															                    |
//  date:	2019.1.21									                |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// カメラ操作クラス
public class VRCamera_Script : MonoBehaviour
{
    // 回転する角度の定数
    private static readonly float RotationY = 45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Positionを取得する
        Vector3 pos = transform.localPosition;
        // Rotationを取得する
        Quaternion rot = transform.localRotation;

        // オイラー角に変換する
        Vector3 rotAngle = rot.eulerAngles;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft)
               || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft))
        {
            rotAngle.y = rotAngle.y - RotationY;
            rot = Quaternion.Euler(rotAngle);

            transform.localPosition = pos;
            transform.localRotation = rot;
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight)
               || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight))
        {
            rotAngle.y = rotAngle.y + RotationY;
            rot = Quaternion.Euler(rotAngle);

            transform.localPosition = pos;
            transform.localRotation = rot;
        }

    }
}
