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

    // 内側を叩く判定フラグ
    private bool m_inHitFlag;
    // 外側を叩く判定フラグ
    private bool m_outHitFlag;
    // 内側を叩いた時のノーツ生成フラグ
    private bool m_inHitNotesFlag;
    // 外側を叩いた時のノーツ生成フラグ
    private bool m_outHitNotesFlag;
    // 叩かれたかの判定フラグ
    private bool m_hitFlag;
    // 当たった数
    private int m_hitNum;

    // 内側を同時に叩いたフラグ
    private bool m_doubleInHitFlag;
    // 外側を同時に叩いたフラグ
    private bool m_doubleOutHitFlag;

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
        m_rightStickState = 0;
        m_leftStick = FindObjectOfType<StickLeft_Script>();

        m_inHitFlag = false;
        m_outHitFlag = false;
        m_inHitNotesFlag = false;
        m_outHitNotesFlag = false;
        m_hitFlag = false;
        m_hitNum = 0;

        m_doubleInHitFlag = false;
        m_doubleOutHitFlag = false;

        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // 当たっていたら
        if (m_hitNum != 0)
        {
            m_hitFlag = true;
        }
        // 当たっていなければ
        else
        {
            m_hitFlag = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 内側に当たったら
        if (m_inHitFlag == true)
        {
            // 振動させる
            OVRHaptics.RightChannel.Preempt(m_vibClip);
            // 右スティックの状態を叩いた状態に変更
            m_rightStickState = 1;
            // 時間を代入
            m_leftStick.DoubleHitTime = DOUBLE_HIT_TIME;
            // 音を鳴らす
            audioSource.PlayOneShot(m_inHitSE);

            m_inHitNotesFlag = true;

            // 左スティックが叩いた状態だったら
            if (m_leftStick.LeftStickState == 1 && m_leftStick.InHitNotesFlag == true)
            {
                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);

                // 右スティックの状態を元に戻す
                m_rightStickState = 0;
                // 左スティックの状態を元に戻す
                m_leftStick.LeftStickState = 0;
                // 時間を初期化
                m_leftStick.DoubleHitTime = 0;

                m_inHitNotesFlag = false;
                m_leftStick.InHitNotesFlag = false;

                m_doubleInHitFlag = true;
            }
        }
        // 外側に当たったら
        else
        {
            if (m_outHitFlag == true)
            {
                // 振動させる
                OVRHaptics.RightChannel.Preempt(m_vibClip);
                // 右スティックの状態を叩いた状態に変更
                m_rightStickState = 1;
                // 時間を代入
                m_leftStick.DoubleHitTime = DOUBLE_HIT_TIME;
                // 音を鳴らす
                audioSource.PlayOneShot(m_outHitSE);

                m_outHitNotesFlag = true;

                // 左スティックが叩いた状態だったら
                if (m_leftStick.LeftStickState == 1 && m_leftStick.OutHitNotesFlag == true)
                {
                    // 振動させる
                    OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                    OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);

                    // 右スティックの状態を元に戻す
                    m_rightStickState = 0;
                    // 左スティックの状態を元に戻す
                    m_leftStick.LeftStickState = 0;
                    // 時間を初期化
                    m_leftStick.DoubleHitTime = 0;

                    m_outHitNotesFlag = false;
                    m_leftStick.OutHitNotesFlag = false;

                    m_doubleOutHitFlag = true;
                }
            }
        }

        // 右スティックで叩いたら
        if (m_rightStickState == 1)
        {
            // 時間を計る
            m_leftStick.DoubleHitTime--;
        }

        // 内側を叩く判定フラグを初期化
        m_inHitFlag = false;
        // 外側を叩く判定フラグを初期化
        m_outHitFlag = false;
    }

    // 当たり判定
    void OnCollisionEnter(Collision collision)
    {
        // カウントアップ
        m_hitNum++;

        // まだ当たっていなければ
        if (m_hitFlag == false)
        {
            // 内側を叩いたら
            if (collision.gameObject.name == "InDrum")
            {
                m_inHitFlag = true;
            }
            // 外側を叩いたら
            else if (collision.gameObject.name == "OutDrum")
            {
                m_outHitFlag = true;
            }
        }
    }

    // 当たり判定を抜けた処理
    void OnCollisionExit(Collision collision)
    {
        // カウントダウン
        m_hitNum--;
    }

    // 右スティックの状態のプロパティ
    public int RightStickState
    {
        get { return m_rightStickState; }
        set { m_rightStickState = value; }
    }

    public bool InHitFlag
    {
        get { return m_inHitFlag; }
        set { m_inHitFlag = value; }
    }

    public bool OutHitFlag
    {
        get { return m_outHitFlag; }
        set { m_outHitFlag = value; }
    }

    public bool InHitNotesFlag
    {
        get { return m_inHitNotesFlag; }
        set { m_inHitNotesFlag = value; }
    }
    public bool OutHitNotesFlag
    {
        get { return m_outHitNotesFlag; }
        set { m_outHitNotesFlag = value; }
    }

    public bool DoubleInHitFlag
    {
        get { return m_doubleInHitFlag; }
        set { m_doubleInHitFlag = value; }
    }
    public bool DoubleOutHitFlag
    {
        get { return m_doubleOutHitFlag; }
        set { m_doubleOutHitFlag = value; }
    }
}
