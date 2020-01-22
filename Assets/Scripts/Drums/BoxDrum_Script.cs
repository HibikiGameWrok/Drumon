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
    [SerializeField]
    private GameObject[] m_tradeUIC;

    // カーソルUI
    private Move_CursorUI_Script m_cursorUI;
    // カーソルUI
    private Move_CursorUI_Script m_JudgeUI;
    // メニューUI
    private StatusMenuUI_Script m_statusMenuUI;

    private bool m_switchFlag = false;
    public bool switchFlag
    {
        get { return m_switchFlag; }
    }


    void Awake()
    {
        m_cursorUI = m_tradeUIC[0].GetComponent<Move_CursorUI_Script>();
        m_JudgeUI = m_tradeUIC[1].GetComponent<Move_CursorUI_Script>();
        m_statusMenuUI = m_tradeUIC[0].GetComponent<StatusMenuUI_Script>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        // 叩かれた時の処理
        HitProcess();
    }

    // 叩かれた時の処理
    private void HitProcess()
    {
        if (m_leftStick.BoxDrumHitFlag == true)
        {
            // 左スティックで真ん中を叩く
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                if (m_cursorUI.DecusuonFlag == false)
                {
                    // 決定フラグをtrue
                    m_cursorUI.Decision();
                    // 決定UIをアクティブ
                    m_JudgeUI.GetComponent<SetChildActiveObject_Script>().OpenUI();
                }
                else if (m_cursorUI.DecusuonFlag == true && m_JudgeUI.DecusuonFlag == false)
                {
                    if (m_JudgeUI.MovePoint == 0)
                    {
                        m_switchFlag = true;
                        // 入れ替え
                        if (m_cursorUI.MovePoint < 3)
                        {
                            CreatureList_Script.Get.List.DataList[m_cursorUI.MovePoint] = CreatureList_Script.Get.OverData;
                            CreatureList_Script.Get.OverData = null;
                        }
                        else
                        {
                            CreatureList_Script.Get.OverData = null;
                        }
                    }
                    else if (m_JudgeUI.MovePoint != 0)
                    {
                        // 決定UIをアクティブ
                        m_JudgeUI.GetComponent<SetChildActiveObject_Script>().CloseUI();
                    }
                }
                // 内側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }

            // 左スティックで端を叩く
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                m_cursorUI.CursorUP();
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }

            m_leftStick.BoxDrumHitFlag = false;
        }
        else if (m_rightStick.BoxDrumHitFlag == true)
        {
            // 右スティックで真ん中を叩く
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                if (m_cursorUI.DecusuonFlag == false)
                {
                    // 決定フラグをtrue
                    m_cursorUI.Decision();
                    // 決定UIをアクティブ
                    m_JudgeUI.GetComponent<SetChildActiveObject_Script>().OpenUI();
                }
                else if (m_cursorUI.DecusuonFlag == true && m_JudgeUI.DecusuonFlag == false)
                {
                    if (m_JudgeUI.MovePoint == 0)
                    {
                        m_switchFlag = true;
                        // 入れ替え
                        if (m_cursorUI.MovePoint < 3)
                        {
                            CreatureList_Script.Get.List.DataList[m_cursorUI.MovePoint] = CreatureList_Script.Get.OverData;
                            CreatureList_Script.Get.OverData = null;
                        }
                        else
                        {
                            CreatureList_Script.Get.OverData = null;
                        }
                    }
                    else if(m_JudgeUI.MovePoint != 0)
                    {
                        // 決定UIをアクティブ
                        m_JudgeUI.GetComponent<SetChildActiveObject_Script>().CloseUI();
                    }
                }

            }

            // 内側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
        }

        // 右スティックで端を叩く
        if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
        {
            m_cursorUI.CursorDown();
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
        }

        m_rightStick.BoxDrumHitFlag = false;
    }
}
