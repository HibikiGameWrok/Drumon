using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickLeft_Script : MonoBehaviour
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
    private const int DOUBLE_HIT_TIME = 3;


    // バイブレーション
    private OVRHapticsClip m_vibClip;
    private OVRHapticsClip m_doubleHitVibClip;
    private byte[] m_vibration;
    private byte[] m_doubleHitVib;
    // 左スティックの状態
    private int m_leftStickState;
    // 右スティック
    private StickRight_Script m_rightStick;
    // 同時に叩ける時間
    private int m_doubleHitTime;

    // 内側を叩く判定フラグ
    private bool m_inHitFlag;
    // 外側を叩く判定フラグ
    private bool m_outHitFlag;

    AudioSource audioSource;
    // 内側を叩いた音
    [SerializeField]
    private AudioClip m_inHitSE;
    // 外側を叩いた音
    [SerializeField]
    private AudioClip m_outHitSE;

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
        m_leftStickState = 0;
        m_doubleHitTime = 0;
        m_rightStick = FindObjectOfType<StickRight_Script>();

        m_inHitFlag = false;
        m_outHitFlag = false;

        audioSource = GetComponent<AudioSource>();
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

        // 内側に当たったら
        if (m_inHitFlag == true)
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);
            // 左スティックの状態を叩いた状態に変更
            m_leftStickState = 1;
            // 時間を代入
            m_doubleHitTime = DOUBLE_HIT_TIME;
            // 音を鳴らす
            audioSource.PlayOneShot(m_inHitSE);

            // 右スティックが叩いた状態だったら
            if (m_rightStick.RightStickState == 1)
            {
                Debug.Log("doubleHit");

                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);
                // 左スティックの状態を元に戻す
                m_leftStickState = 0;
                // 右スティックの状態を元に戻す
                m_rightStick.RightStickState = 0;
                // 時間を初期化
                m_doubleHitTime = 0;
            }
        }
        // 外側に当たったら
        else
        {
            if (m_outHitFlag == true)
            {
                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_vibClip);
                // 左スティックの状態を叩いた状態に変更
                m_leftStickState = 1;
                // 時間を代入
                m_doubleHitTime = DOUBLE_HIT_TIME;
                // 音を鳴らす
                audioSource.PlayOneShot(m_outHitSE);

                // 右スティックが叩いた状態だったら
                if (m_rightStick.RightStickState == 1)
                {
                    Debug.Log("doubleHit");

                    // 振動させる
                    OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                    OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);

                    // 左スティックの状態を元に戻す
                    m_leftStickState = 0;
                    // 右スティックの状態を元に戻す
                    m_rightStick.RightStickState = 0;
                    // 時間を初期化
                    m_doubleHitTime = 0;
                }
            }
        }

        // 内側を叩く判定フラグを初期化
        m_inHitFlag = false;
        // 外側を叩く判定フラグを初期化
        m_outHitFlag = false;

        // 左スティックで叩いたら
        if (m_leftStickState == 1)
        {
            // 時間を計る
            m_doubleHitTime--;
        }
        // 時間が0になったら
        if (m_doubleHitTime < 0)
        {
            // 左スティックの状態を元に戻す
            m_leftStickState = 0;
            // 時間を初期化
            m_doubleHitTime = 0;
        }
    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("LeftHit");

        if (collision.gameObject.name == "InDrum")
        {
            Debug.Log("AttackInDrum");

            m_inHitFlag = true;
        }

        if (collision.gameObject.name == "OutDrum")
        {
            Debug.Log("AttackOutDrum");

            m_outHitFlag = true;
        }
    }

    // 左スティックの状態のプロパティ
    public int LeftStickState
    {
        get { return m_leftStickState; }
        set { m_leftStickState = value; }
    }
    // 同時に叩ける時間のプロパティ
    public int DoubleHitTime
    {
        get { return m_doubleHitTime; }
        set { m_doubleHitTime = value; }
    }
}
