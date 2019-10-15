using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickLeft_Script : MonoBehaviour
{
    // 定数
    // バイブの長さ
    private const int VIB_LENGTH = 50;
    // バイブの大きさ
    private const int VIB_SIZE = 128;


    // バイブレーション
    private OVRHapticsClip m_vibClip;
    private byte[] m_vibration;

    // Start is called before the first frame update
    void Start()
    {
        // バイブの長さ
        m_vibration = new byte[VIB_LENGTH];
        for (int i = 0; i < m_vibration.Length; i++)
        {
            // バイブの大きさ
            m_vibration[i] = VIB_SIZE;
        }
        m_vibClip = new OVRHapticsClip(m_vibration, m_vibration.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            Debug.Log("Xボタンを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            Debug.Log("Yボタンを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            Debug.Log("メニューボタン（左アナログスティックの下にある）を押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            Debug.Log("左人差し指トリガーを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            Debug.Log("左中指トリガーを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
        {
            Debug.Log("左アナログスティックを上に倒した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
        {
            Debug.Log("左アナログスティックを下に倒した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            Debug.Log("左アナログスティックを左に倒した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            Debug.Log("左アナログスティックを右に倒した");
        }
    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("LeftHit");
        if (collision.gameObject.name == "Drum")
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);
        }
    }
}
