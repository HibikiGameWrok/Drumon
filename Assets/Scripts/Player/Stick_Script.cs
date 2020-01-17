using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.XR;

public abstract class Stick_Script : MonoBehaviour
{
    // 定数
    // バイブの長さ
    protected const int VIB_LENGTH = 50;
    // 同時に叩いた時のバイブの長さ
    protected const int DOUBLE_HIT_VIB_LENGTH = 200;
    // バイブの大きさ
    protected const int VIB_SIZE = 128;
    // 同時に叩いた時のバイブの大きさ
    protected const int DOUBLE_HIT_VIB_SIZE = 255;

    public enum HIT_PATTERN
    {
        IN_HIT = (1 << 0),          // 内側を叩いた判定(0001)
        OUT_HIT = (1 << 1),         // 外側を叩いた判定(0010)
    }

    public enum HIT_DRUM
    {
        ATTACK = (1 << 0),  // 攻撃ドラムを叩いた判定(0001)
        HEAL = (1 << 1),    // 回復ドラムを叩いた判定(0010)
        SWITCH = (1 << 2),  // 選択ドラムを叩いた判定(0100)
        CAPTURE = (1 << 3)  // 捕獲ドラムを叩いた判定(1000)
    }

    // バイブレーション
    protected OVRHapticsClip m_vibClip;
    protected OVRHapticsClip m_doubleHitVibClip;
    public OVRHapticsClip DoubleHitVibClip
    {
        get { return m_doubleHitVibClip; }
    }
    protected byte[] m_vibration;
    protected byte[] m_doubleHitVib;

    // 自身のオーディオコンポーネント取得変数
    protected AudioSource audioSource;
    // 内側を叩いた音
    [SerializeField]
    protected AudioClip m_inHitSE = null;
    // 外側を叩いた音
    [SerializeField]
    protected AudioClip m_outHitSE = null;
    // 回復ドラムを叩いた音
    [SerializeField]
    protected AudioClip m_healHitSE = null;

    // スティックマネージャー
    protected StickManager_Script m_stickManager;

    // 叩いた場所のフラグ管理
    protected Flag_Script m_hitPatternFlag;
    // 叩いた場所のフラグ管理のプロパティ
    public Flag_Script HitPatternFlag
    {
        get { return m_hitPatternFlag; }
        set { m_hitPatternFlag = value; }
    }

    // 叩いたドラムのフラグ管理
    protected Flag_Script m_hitDrumFlag;
    // 叩いたドラムのフラグ管理のプロパティ
    public Flag_Script HitDrumFlag
    {
        get { return m_hitDrumFlag; }
        set { m_hitDrumFlag = value; }
    }

    // 叩かれたかの判定フラグ
    protected bool m_hitFlag;
    // 当たった数
    protected int m_hitNum;
    // 最後に当たったオブジェクトのタグ
    protected string m_lastCollisionTag = null;

    // Boxドラムの叩いたフラグ
    protected bool m_boxDrumHitFlag = false;
    // Boxドラムの叩いたフラグのプロパティ
    public bool BoxDrumHitFlag
    {
        get { return m_boxDrumHitFlag; }
        set { m_boxDrumHitFlag = value; }
    }

    // タイトルドラムの叩いたフラグ
    protected bool m_titleDrumHitFlag = false;
    // タイトルドラムの叩いたフラグのプロパティ
    public bool TitleDrumHitFlag
    {
        get { return m_titleDrumHitFlag; }
        set { m_titleDrumHitFlag = value; }
    }

    // 初期化
    public abstract void Initialize(StickManager_Script manager);

    // 当たり判定を抜けた処理
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "AttackInDrum" || collision.gameObject.tag == "AttackOutDrum" || collision.gameObject.tag == "SwitchInDrum" || collision.gameObject.tag == "SwitchOutDrum" || collision.gameObject.tag == "CaptureDrum" || collision.gameObject.tag == "BoxInDrum" || collision.gameObject.tag == "BoxOutDrum")
        {
            // カウントダウン
            m_hitNum--;
        }
    }

    // バイブの初期化
    protected void InitialVibration()
    {
        if (XRDevice.isPresent)
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
        }
    }
}
