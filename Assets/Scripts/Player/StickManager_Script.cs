using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StickManager_Script : MonoBehaviour
{
    // 定数
    // 同時に叩ける時間
    protected const int DOUBLE_HIT_TIME = 2;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    //同時に叩ける時間
    private int m_doubleHitTime = 0;
    //同時に叩ける時間のプロパティ
    public int DoubleHitTime
    {
        get { return m_doubleHitTime; }
        set { m_doubleHitTime = value; }
    }

    // 内側を同時に叩いた判定フラグ
    private bool m_doubleInHitFlag = false;
    // 内側を同時に叩いた判定フラグのプロパティ
    public bool DoubleInHitFlag
    {
        get { return m_doubleInHitFlag; }
        set { m_doubleInHitFlag = value; }
    }
    // 外側を同時に叩いた判定フラグ
    private bool m_doubleOutHitFlag = false;
    // 外側を同時に叩いた判定フラグのプロパティ
    public bool DoubleOutHitFlag
    {
        get { return m_doubleOutHitFlag; }
        set { m_doubleOutHitFlag = value; }
    }

    // 選択されているアイコン
    private int m_pickCount = 0;
    // 選択されているアイコンのプロパティ
    public int PickCount
    {
        get { return m_pickCount; }
        set { m_pickCount = value; }
    }

    // UIの表示フラグ
    private bool m_openUIFlag = false;
    // UIの表示フラグのプロパティ
    public bool OpenUIFlag
    {
        get { return m_openUIFlag; }
        set { m_openUIFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();

        // 初期化
        m_leftStick.Initialize(this);
        m_rightStick.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_doubleHitTime >= 0)
        {
            m_doubleHitTime--;
        }

        // 攻撃ドラムの同時に叩いた処理
        DoubleHitAttackDrum();
        // 選択ドラムを叩いた処理
        HitSwitchDrum();
    }

    // 攻撃ドラムの同時に叩いた処理
    void DoubleHitAttackDrum()
    {
        if (XRDevice.isPresent)
        {
            if (m_leftStick.HitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.ATTACK) == true && m_rightStick.HitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.ATTACK) == true)
            {
                // 内側
                if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true && m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
                {
                    m_doubleInHitFlag = true;
                    // 振動させる
                    OVRHaptics.LeftChannel.Preempt(m_leftStick.DoubleHitVibClip);
                    OVRHaptics.RightChannel.Preempt(m_rightStick.DoubleHitVibClip);
                }
                // 外側
                else if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true && m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
                {
                    m_doubleOutHitFlag = true;
                    // 振動させる
                    OVRHaptics.LeftChannel.Preempt(m_leftStick.DoubleHitVibClip);
                    OVRHaptics.RightChannel.Preempt(m_rightStick.DoubleHitVibClip);
                }

                // 内側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
                // 内側を叩いた判定フラグを伏せる
                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
                // 外側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
                // 外側を叩いた判定フラグを伏せる
                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }
        }
    }

    // 選択ドラムを叩いた処理
    void HitSwitchDrum()
    {
        if (XRDevice.isPresent)
        {
            if (m_leftStick.HitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.SWITCH) == true || m_rightStick.HitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.SWITCH) == true)
            {
                // 内側
                if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true || m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
                {
                    // UIが表示されていなければ
                    if (m_openUIFlag == false)
                    {
                        // UIの表示フラグを立てる
                        m_openUIFlag = true;
                    }
                    // UIが表示されていたら
                    else
                    {
                        // UIの表示フラグを伏せる
                        m_openUIFlag = false;
                    }
                }
                // 外側
                else if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true || m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
                {
                    // UIが表示されていなければ
                    if (m_openUIFlag == false)
                    {
                        // UIの表示フラグを立てる
                        m_openUIFlag = true;
                    }
                }
            }
        }
    }

    // 同時に叩ける時間の代入
    public void SetDoubleHitTime()
    {
        if (m_doubleHitTime <= 0)
        {
            m_doubleHitTime = DOUBLE_HIT_TIME;
        }
    }
}
