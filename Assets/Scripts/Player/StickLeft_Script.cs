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
    private const int DOUBLE_HIT_TIME = 5;

    public enum HIT_PATTERN
    {
        IN_HIT = (1 << 0),          // 内側を叩いた判定(0001)
        DOUBLE_IN_HIT = (1 << 1),   // 内側を同時に叩いた判定(0010)
        OUT_HIT = (1 << 2),         // 外側を叩いた判定(0100)
        DOUBLE_OUT_HIT = (1 << 3)   // 外側を同時に叩いた判定(1000)
    }

    public enum HIT_DRUM
    {
        ATTACK = (1 << 0),  // 攻撃ドラムを叩いた判定(0001)
        HEAL = (1 << 1),    // 回復ドラムを叩いた判定(0010)
        SWITCH = (1 << 2),  // 選択ドラムを叩いた判定(0100)
        CAPTURE = (1 << 3)  // 捕獲ドラムを叩いた判定(1000)
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
    // 同時に叩ける時間のプロパティ
    public int DoubleHitTime
    {
        get { return m_doubleHitTime; }
        set { m_doubleHitTime = value; }
    }

    // 叩いた場所のフラグ管理
    private Flag_Script m_hitPatternFlag;
    // 叩いた場所のフラグ管理のプロパティ
    public Flag_Script HitPatternFlag
    {
        get { return m_hitPatternFlag; }
        set { m_hitPatternFlag = value; }
    }

    // 叩いたドラムのフラグ管理
    private Flag_Script m_hitDrumFlag;
    // 叩いたドラムのフラグ管理のプロパティ
    public Flag_Script HitDrumFlag
    {
        get { return m_hitDrumFlag; }
        set { m_hitDrumFlag = value; }
    }

    // 内側を叩いて同時叩き可能フラグ
    private bool m_inHitConnectFlag;
    // 内側を叩いて同時叩き可能フラグのプロパティ
    public bool InHitConnectFlag
    {
        get { return m_inHitConnectFlag; }
        set { m_inHitConnectFlag = value; }
    }
    // 外側を叩いて同時叩き可能フラグ
    private bool m_outHitConnectFlag;
    // 外側を叩いて同時叩き可能フラグのプロパティ
    public bool OutHitConnectFlag
    {
        get { return m_outHitConnectFlag; }
        set { m_outHitConnectFlag = value; }
    }
    // 叩かれたかの判定フラグ
    private bool m_hitFlag;
    // 当たった数
    private int m_hitNum;

    // 選択されているアイコン
    private int m_pickCount;
    // 選択されているアイコンのプロパティ
    public int PickCount
    {
        get { return m_pickCount; }
        set { m_pickCount = value; }
    }

    // UIの表示フラグ
    private bool m_openUIFlag;
    // UIの表示フラグのプロパティ
    public bool OpenUIFlag
    {
        get { return m_openUIFlag; }
        set { m_openUIFlag = value; }
    }

    // モンスターの変更フラグ
    private bool m_creatureChengeFlag;
    // モンスターの変更フラグのプロパティ
    public bool CreatureChengeFlag
    {
        get { return m_creatureChengeFlag; }
        set { m_creatureChengeFlag = value; }
    }

    private AudioSource audioSource;
    // 内側を叩いた音
    [SerializeField]
    private AudioClip m_inHitSE = null;
    // 外側を叩いた音
    [SerializeField]
    private AudioClip m_outHitSE = null;
    // 回復ドラムを叩いた音
    [SerializeField]
    private AudioClip m_healHitSE = null;

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
        m_hitDrumFlag = new Flag_Script();

        m_inHitConnectFlag = false;
        m_outHitConnectFlag = false;
        m_hitFlag = false;

        m_hitNum = 0;

        //m_pickCount = 0;

        m_openUIFlag = false;

        m_creatureChengeFlag = false;

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
        // 攻撃ドラムを叩いたら
        if (m_hitDrumFlag.IsFlag((uint)HIT_DRUM.ATTACK) == true)
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
            else if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.OUT_HIT) == true)
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
        }
        // 回復ドラムを叩いたら
        else if (m_hitDrumFlag.IsFlag((uint)HIT_DRUM.HEAL) == true)
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);
            // 音を鳴らす
            audioSource.PlayOneShot(m_healHitSE);
        }
        // 選択ドラムを叩いたら
        else if (m_hitDrumFlag.IsFlag((uint)HIT_DRUM.SWITCH) == true)
        {
            // 内側に当たったら
            if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.IN_HIT) == true)
            {
                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_vibClip);
                // 音を鳴らす
                audioSource.PlayOneShot(m_inHitSE);

                // UIが表示されていなければ
                if (m_openUIFlag == false)
                {
                    // UIの表示フラグを立てる
                    m_openUIFlag = true;
                }
                // UIが表示されていたら
                else
                {
                    // モンスターの変更フラグを立てる
                    m_creatureChengeFlag = true;
                    // UIの表示フラグを伏せる
                    m_openUIFlag = false;

                    // 選択カーソルの位置がBackだったら     
                    //if (m_pickCount == 6)
                    //{
                    //    // UIの表示フラグを伏せる
                    //    m_openUIFlag = false;
                    //}
                    //else
                    //{
                    //    // モンスターの変更フラグを立てる
                    //    m_creatureChengeFlag = true;
                    //    // UIの表示フラグを伏せる
                    //    m_openUIFlag = false;
                    //}
                }
            }
            // 外側に当たったら
            else if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.OUT_HIT) == true)
            {
                // 振動させる
                OVRHaptics.LeftChannel.Preempt(m_vibClip);
                // 音を鳴らす
                audioSource.PlayOneShot(m_outHitSE);

                // UIが表示されていなければ
                if (m_openUIFlag == false)
                {
                    // UIの表示フラグを立てる
                    m_openUIFlag = true;
                }
                // UIが表示されていたら
                else
                {
                    //if (m_pickCount > 0)
                    //{
                    //    m_pickCount--;
                    //}
                    //else if (m_pickCount <= 0)
                    //{
                    //    m_pickCount = 6;
                    //}
                }
            }
        }
        // 捕獲ドラムを叩いたら
        else if (m_hitDrumFlag.IsFlag((uint)HIT_DRUM.CAPTURE) == true)
        {
            // 振動させる
            OVRHaptics.LeftChannel.Preempt(m_vibClip);
            // 音を鳴らす
            audioSource.PlayOneShot(m_healHitSE);

            // 捕獲ドラムを叩いた判定フラグを伏せる
            //m_hitDrumFlag.OffFlag((uint)HIT_DRUM.CAPTURE);
        }

        // 左スティックで叩いたら
        if (m_inHitConnectFlag == true || m_outHitConnectFlag == true)
        {
            // 時間を計る
            m_doubleHitTime--;
        }

        // 内側を叩いた判定フラグを伏せる
        m_hitPatternFlag.OffFlag((uint)HIT_PATTERN.IN_HIT);
        // 外側を叩いた判定フラグを伏せる
        m_hitPatternFlag.OffFlag((uint)HIT_PATTERN.OUT_HIT);
        // 攻撃ドラムを叩いた判定フラグを伏せる
        m_hitDrumFlag.OffFlag((uint)HIT_DRUM.ATTACK);
    }

    // 当たり判定
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "AttackInDrum" || collision.gameObject.tag == "AttackOutDrum" || collision.gameObject.tag == "HealDrum" || collision.gameObject.tag == "SwitchInDrum" || collision.gameObject.tag == "SwitchOutDrum" || collision.gameObject.tag == "CaptureDrum")
        {
            // カウントアップ
            m_hitNum++;

            // まだ当たっていなければ
            if (m_hitFlag == false)
            {
                // 攻撃ドラムの内側を叩いたら
                if (collision.gameObject.tag == "AttackInDrum")
                {
                    // 内側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.IN_HIT);
                    // 攻撃ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.ATTACK);
                }
                // 攻撃ドラムの外側を叩いたら
                else if (collision.gameObject.tag == "AttackOutDrum")
                {
                    // 外側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.OUT_HIT);
                    // 攻撃ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.ATTACK);
                }
                // 回復ドラムを叩いたら
                else if (collision.gameObject.tag == "HealDrum")
                {
                    // 回復ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.HEAL);
                }
                // 選択ドラムの内側を叩いたら
                else if (collision.gameObject.tag == "SwitchInDrum")
                {
                    // 内側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.IN_HIT);
                    // 選択ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.SWITCH);
                }
                // 選択ドラムの外側を叩いたら
                else if (collision.gameObject.tag == "SwitchOutDrum")
                {
                    // 外側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.OUT_HIT);
                    // 選択ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.SWITCH);
                }
                // 捕獲ドラムを叩いたら
                else if (collision.gameObject.tag == "CaptureDrum")
                {
                    // 捕獲ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.CAPTURE);
                }
            }
        } 
    }

    // 当たり判定を抜けた処理
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "AttackInDrum" || collision.gameObject.tag == "AttackOutDrum" || collision.gameObject.tag == "HealDrum" || collision.gameObject.tag == "SwitchInDrum" || collision.gameObject.tag == "SwitchOutDrum" || collision.gameObject.tag == "CaptureDrum")
        {
            // カウントダウン
            m_hitNum--;
        }
    }
}
