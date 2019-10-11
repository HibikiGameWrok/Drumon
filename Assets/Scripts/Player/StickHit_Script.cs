using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHit_Script : MonoBehaviour
{
    // 左スティック
    [SerializeField]
    private GameObject m_stickLeft;
    // 右スティック
    [SerializeField]
    private GameObject m_stickRight;

    // バイブレーション
    private OVRHapticsClip m_vibClip;
    private byte[] m_vibration;

    // Start is called before the first frame update
    void Start()
    {
        // バイブの長さ
        m_vibration = new byte[50];
        for (int i = 0; i < m_vibration.Length; i++)
        {
            // バイブの強さ
            m_vibration[i] = 128;
        }
        m_vibClip = new OVRHapticsClip(m_vibration, m_vibration.Length);
    }

    // Update is called once per frame
    void Update()
    {
        // ハンドスピナー
        //if (m_stickState == 1)
        //{
        //    m_count++;
        //}
        //if (m_count >= 30)
        //{
        //    this.m_stickLeft.transform.Rotate(new Vector3(1, 0, 0), 10);
        //    m_stickState = 2;
        //}
    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        // 弾かれ処理
        if (collision.gameObject.name == "DistanceGrabHandLeft")
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);
        }
        if (collision.gameObject.name == "DistanceGrabHandRight")
        {
            // 振動させる
            OVRHaptics.RightChannel.Preempt(m_vibClip);
        } 
    }
}
