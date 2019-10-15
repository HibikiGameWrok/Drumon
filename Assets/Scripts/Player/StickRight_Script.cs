using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickRight_Script : MonoBehaviour
{
    // 定数
    // バイブの長さ
    private const int VIB_LENGTH = 50;
    // 同時に叩いた時のバイブの長さ
    private const int DOUBLE_HIT_VIB_LENGTH = 200;
    // バイブの大きさ
    private const int VIB_SIZE = 128;
    // 同時に叩いた時のバイブの大きさ
    private const int DOUBLE_HIT_VIB_SIZE = 255;
    // 同時に叩ける時間
    private const int DOUBLE_HIT_TIME = 5;


    // バイブレーション
    private OVRHapticsClip m_vibClip;
    private OVRHapticsClip m_doubleHitVibClip;
    private byte[] m_vibration;
    private byte[] m_doubleHitVib;
    // 右スティックの状態
    private int m_rightStickState;
    // 左スティック
    private StickLeft_Script m_leftStick;

    // Start is called before the first frame update
    void Start()
    {
        // バイブの長さ
        m_vibration = new byte[VIB_LENGTH];
        // 同時に叩いた時のバイブの長さ
        m_doubleHitVib = new byte[DOUBLE_HIT_VIB_LENGTH];
        for (int i = 0; i < m_vibration.Length; i++)
        {
            // バイブの大きさ
            m_vibration[i] = VIB_SIZE;
        }
        for (int i = 0; i < m_doubleHitVib.Length; i++)
        {
            // 同時に叩いた時のバイブの大きさ
            m_doubleHitVib[i] = DOUBLE_HIT_VIB_SIZE;
        }
        m_vibClip = new OVRHapticsClip(m_vibration, m_vibration.Length);
        m_doubleHitVibClip = new OVRHapticsClip(m_doubleHitVib, m_doubleHitVib.Length);

        // 初期化
        m_rightStickState = 0;
        m_leftStick = FindObjectOfType<StickLeft_Script>();
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

        // 右スティックで叩いたら
        if (m_rightStickState == 1)
        {
            // 時間を計る
            m_leftStick.DoubleHitTime--;
        }
        // 時間が0になったら
        if (m_leftStick.DoubleHitTime < 0)
        {
            // 右スティックの状態を元に戻す
            m_rightStickState = 0;
            // 時間を初期化
            m_leftStick.DoubleHitTime = 0;
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
            // 右スティックの状態を叩いた状態に変更
            m_rightStickState = 1;
            m_leftStick.DoubleHitTime = DOUBLE_HIT_TIME;

            // 左スティックが叩いた状態だったら
            if (m_leftStick.LeftStickState == 1)
            {
                Debug.Log("doubleHit");

                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);

                // 右スティックの状態を元に戻す
                m_rightStickState = 0;
                // 左スティックの状態を元に戻す
                m_leftStick.LeftStickState = 0;
                // 時間を初期化
                m_leftStick.DoubleHitTime = 0;
            }
        }
    }

    // 右スティックの状態のプロパティ
    public int RightStickState
    {
        get { return m_rightStickState; }
        set { m_rightStickState = value; }
    }
}
