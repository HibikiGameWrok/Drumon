using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrum_Script : MonoBehaviour
{
    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    private int m_selectCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_leftStick.BoxDrumHitFlag == true)
        {
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                Debug.Log("OK");

                // 内側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                if (m_selectCount > 0)
                {
                    m_selectCount--;
                }

                Debug.Log(m_selectCount);

                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }

            m_leftStick.BoxDrumHitFlag = false;
        }
        else if (m_rightStick.BoxDrumHitFlag == true)
        {
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                Debug.Log("OK");

                // 内側を叩いた判定フラグを伏せる
                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                if (m_selectCount < 2)
                {
                    m_selectCount++;
                }

                Debug.Log(m_selectCount);

                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }

            m_rightStick.BoxDrumHitFlag = false;
        }
    }
}
