using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrum_Script : MonoBehaviour
{
    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    // トレードUIC
    private GameObject m_tradeUIC;
    // カーソルUI
    private Move_CursorUI_Script m_cursorUI;
    // メニューUI
    private StatusMenuUI_Script m_statusMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
        m_tradeUIC = GameObject.Find("TradeUI_Canvas");
        m_cursorUI = m_tradeUIC.GetComponent<Move_CursorUI_Script>();
        m_statusMenuUI = m_tradeUIC.GetComponent<StatusMenuUI_Script>();

        // UIの非表示
        m_statusMenuUI.CloseUI();
    }

    // Update is called once per frame
    void Update()
    {
        // 叩かれた時の処理
        HitProcess();

        if (this.gameObject.activeInHierarchy == true)
        {
            // UIの表示
            m_statusMenuUI.OpenUI();
        }
        else
        {
            // UIの非表示
            m_statusMenuUI.CloseUI();
        }
    }

    // 叩かれた時の処理
    private void HitProcess()
    {
        if (m_leftStick.BoxDrumHitFlag == true)
        {
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // 入れ替え
                if (m_cursorUI.MovePoint < 3)
                {
                    CreatureList_Script.Get.List.DataList[m_cursorUI.MovePoint] = CreatureList_Script.Get.OverData;
                }

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
                // 入れ替え
                if (m_cursorUI.MovePoint < 3)
                {
                    CreatureList_Script.Get.List.DataList[m_cursorUI.MovePoint] = CreatureList_Script.Get.OverData;
                }

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
