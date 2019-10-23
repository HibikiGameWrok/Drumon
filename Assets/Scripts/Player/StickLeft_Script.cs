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

    public enum HIT_PATTERN
    {
        IN_HIT = (1 << 0),          // 内側を叩いた判定(0001)
        DOUBLE_IN_HIT = (1 << 1),   // 内側を同時に叩いた判定(0010)
        OUT_HIT = (1 << 2),         // 外側を叩いた判定(0100)
        DOUBLE_OUT_HIT = (1 << 3)   // 外側を同時に叩いた判定(1000)
    }


    // バイブレーション
    private OVRHapticsClip m_vibClip;
    private OVRHapticsClip m_doubleHitVibClip;
    private byte[] m_vibration;
    private byte[] m_doubleHitVib;

    // 右スティック
    private StickRight_Script m_rightStick;
    // 同時に叩ける時間
    private int m_doubleHitTime;

    // 叩いた場所のフラグ管理
    private Flag_Script m_hitPatternFlag;

    // 内側を叩いて同時叩き可能フラグ
    private bool m_inHitConnectFlag;
    // 外側を叩いて同時叩き可能フラグ
    private bool m_outHitConnectFlag;
    // 叩かれたかの判定フラグ
    private bool m_hitFlag;
    // 当たった数
    private int m_hitNum;

    // 回復ドラムを叩いたフラグ
    private bool m_healHitFlag;

    AudioSource audioSource;
    // 内側を叩いた音
    [SerializeField]
    private AudioClip m_inHitSE;
    // 外側を叩いた音
    [SerializeField]
    private AudioClip m_outHitSE;
    // 回復ドラムを叩いた音
    [SerializeField]
    private AudioClip m_healHitSE;

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
        m_doubleHitTime = 0;
        m_rightStick = FindObjectOfType<StickRight_Script>();

        m_hitPatternFlag = new Flag_Script();

        m_inHitConnectFlag = false;
        m_outHitConnectFlag = false;
        m_hitFlag = false;

        m_hitNum = 0;

        m_healHitFlag = false;

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
        // 内側を叩いたら
        if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.IN_HIT) == true)
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);

            // 時間を代入
            m_doubleHitTime = DOUBLE_HIT_TIME;
            // 音を鳴らす
            audioSource.PlayOneShot(m_inHitSE);

            m_inHitConnectFlag = true;

            // 右スティックが叩いた状態だったら
            if (m_rightStick.InHitConnectFlag == true)
            {
                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);

                // 時間を初期化
                m_doubleHitTime = 0;

                m_inHitConnectFlag = false;
                m_rightStick.InHitConnectFlag = false;

                // 内側を同時に叩いた判定フラグを立てる
                m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.DOUBLE_IN_HIT);
            }
        }
        // 外側を叩いたら
        else if(m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.OUT_HIT) == true)
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);

            // 時間を代入
            m_doubleHitTime = DOUBLE_HIT_TIME;
            // 音を鳴らす
            audioSource.PlayOneShot(m_outHitSE);

            m_outHitConnectFlag = true;

            // 右スティックが叩いた状態だったら
            if (m_rightStick.OutHitConnectFlag == true)
            {
                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_doubleHitVibClip);
                OVRHaptics.RightChannel.Preempt(m_doubleHitVibClip);

                // 時間を初期化
                m_doubleHitTime = 0;

                m_outHitConnectFlag = false;
                m_rightStick.OutHitConnectFlag = false;

                // 外側を同時に叩いた判定フラグを立てる
                m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.DOUBLE_OUT_HIT);
            }
        }
        // 回復ドラムを叩いたら
        else if (m_healHitFlag == true)
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);
            // 音を鳴らす
            audioSource.PlayOneShot(m_healHitSE);

            Debug.Log("Heal");
        }

        // 左スティックで叩いたら
        if (m_inHitConnectFlag == true || m_outHitConnectFlag == true)
        {
            // 時間を計る
            m_doubleHitTime--;
        }

        m_healHitFlag = false;

        // 内側を叩いた判定フラグを伏せる
        m_hitPatternFlag.OffFlag((uint)HIT_PATTERN.IN_HIT);
        // 外側を叩いた判定フラグを伏せる
        m_hitPatternFlag.OffFlag((uint)HIT_PATTERN.OUT_HIT);
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
            if (collision.gameObject.tag == "AttackInDrum")
            {
                // 内側を叩いた判定フラグを立てる
                m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.IN_HIT);
            }
            // 外側を叩いたら
            else if (collision.gameObject.tag == "AttackOutDrum")
            {
                // 外側を叩いた判定フラグを立てる
                m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.OUT_HIT);
            }
            // 回復ドラムを叩いたら
            else if (collision.gameObject.tag == "HealDrum")
            {
                m_healHitFlag = true;
            }
        }
    }

    // 当たり判定を抜けた処理
    void OnCollisionExit(Collision collision)
    {
        // カウントダウン
        m_hitNum--;
    }

    // 同時に叩ける時間のプロパティ
    public int DoubleHitTime
    {
        get { return m_doubleHitTime; }
        set { m_doubleHitTime = value; }
    }

    // 叩いた場所のフラグ管理のプロパティ
    public Flag_Script HitPatternFlag
    {
        get { return m_hitPatternFlag; }
        set { m_hitPatternFlag = value; }
    }

    // 内側を叩いて同時叩き可能フラグのプロパティ
    public bool InHitConnectFlag
    {
        get { return m_inHitConnectFlag; }
        set { m_inHitConnectFlag = value; }
    }
    // 外側を叩いて同時叩き可能フラグのプロパティ
    public bool OutHitConnectFlag
    {
        get { return m_outHitConnectFlag; }
        set { m_outHitConnectFlag = value; }
    }

    // 回復ドラムを叩いたフラグのプロパティ
    public bool HealHitFlag
    {
        get { return m_healHitFlag; }
        set { m_healHitFlag = value; }
    }
}
