using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickRight_Script : Stick_Script
{
    // 初期化
    public override void Initialize(StickManager_Script manager)
    {
        // バイブの初期化
        InitialVibration();

        m_hitPatternFlag = new Flag_Script();
        m_hitDrumFlag = new Flag_Script();

        m_hitFlag = false;
        m_hitNum = 0;

        audioSource = GetComponent<AudioSource>();

        m_stickManager = manager;
    }

    // Start is called before the first frame update
    void Start()
    {
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
            // 内側に当たったら
            if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.IN_HIT) == true)
            {
            }
            // 外側に当たったら
            else if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.OUT_HIT) == true)
            {
            }
        }
        // 選択ドラムを叩いたら
        else if (m_hitDrumFlag.IsFlag((uint)HIT_DRUM.SWITCH) == true)
        {
            // 内側に当たったら
            if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.IN_HIT) == true)
            {

            }
            // 外側に当たったら
            else if (m_hitPatternFlag.IsFlag((uint)HIT_PATTERN.OUT_HIT) == true)
            {
            }
        }
        // 捕獲ドラムを叩いたら
        else if (m_hitDrumFlag.IsFlag((uint)HIT_DRUM.CAPTURE) == true)
        {
        }
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

                    // 振動させる
                    OVRHaptics.RightChannel.Preempt(m_vibClip);
                    // 音を鳴らす
                    audioSource.PlayOneShot(m_inHitSE);

                    // 同時に叩ける時間の代入
                    m_stickManager.SetDoubleHitTime();
                }
                // 攻撃ドラムの外側を叩いたら
                else if (collision.gameObject.tag == "AttackOutDrum")
                {
                    // 外側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.OUT_HIT);
                    // 攻撃ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.ATTACK);

                    // 振動させる
                    OVRHaptics.RightChannel.Preempt(m_vibClip);
                    // 音を鳴らす
                    audioSource.PlayOneShot(m_outHitSE);

                    // 同時に叩ける時間の代入
                    m_stickManager.SetDoubleHitTime();
                }
                // 選択ドラムの内側を叩いたら
                else if (collision.gameObject.tag == "SwitchInDrum")
                {
                    // 内側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.IN_HIT);
                    // 選択ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.SWITCH);

                    // 振動させる
                    OVRHaptics.RightChannel.Preempt(m_vibClip);
                    // 音を鳴らす
                    audioSource.PlayOneShot(m_inHitSE);
                }
                // 選択ドラムの外側を叩いたら
                else if (collision.gameObject.tag == "SwitchOutDrum")
                {
                    // 外側を叩いた判定フラグを立てる
                    m_hitPatternFlag.OnFlag((uint)HIT_PATTERN.OUT_HIT);
                    // 選択ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.SWITCH);

                    // 振動させる
                    OVRHaptics.RightChannel.Preempt(m_vibClip);
                    // 音を鳴らす
                    audioSource.PlayOneShot(m_outHitSE);
                }
                // 捕獲ドラムを叩いたら
                else if (collision.gameObject.tag == "CaptureDrum")
                {
                    // 捕獲ドラムを叩いた判定フラグを立てる
                    m_hitDrumFlag.OnFlag((uint)HIT_DRUM.CAPTURE);

                    // 振動させる
                    OVRHaptics.RightChannel.Preempt(m_vibClip);
                    // 音を鳴らす
                    audioSource.PlayOneShot(m_healHitSE);
                }

                m_hitFlag = true;
            }
        }
    }
}
