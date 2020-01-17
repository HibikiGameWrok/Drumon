using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrum_Script : MonoBehaviour
{
    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    // カーソルUI
    private Move_CursorUI_Script m_cursorUI;

    // Start is called before the first frame update
    void Start()
    {
        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
        m_cursorUI = GameObject.FindGameObjectWithTag("TradeUI").GetComponent<Move_CursorUI_Script>();

        this.gameObject.SetActive(false);
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
                m_cursorUI.CursorUP();

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
                m_cursorUI.CursorDown();

                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }

            m_rightStick.BoxDrumHitFlag = false;
        }
    }
}
