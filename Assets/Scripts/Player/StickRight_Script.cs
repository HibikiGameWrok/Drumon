using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickRight_Script : MonoBehaviour
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
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            Debug.Log("Aボタンを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            Debug.Log("Bボタンを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            Debug.Log("右人差し指トリガーを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            Debug.Log("右中指トリガーを押した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp))
        {
            Debug.Log("右アナログスティックを上に倒した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown))
        {
            Debug.Log("右アナログスティックを下に倒した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            Debug.Log("右アナログスティックを左に倒した");
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
        {
            Debug.Log("右アナログスティックを右に倒した");
        }
    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("RightHit");
        if (collision.gameObject.name == "Drum")
        {
            // 振動させる
            OVRHaptics.RightChannel.Preempt(m_vibClip);
        }
    }
}
