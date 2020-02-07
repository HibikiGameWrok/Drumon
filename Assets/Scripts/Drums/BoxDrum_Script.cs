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
    private GameObject[] m_tradeUI = null;

    // カーソルUI
    private Move_CursorUI_Script m_cursorUI;
    // カーソルUI
    private Move_CursorUI_Script m_JudgeUI;

    private bool m_switchFlag = false;
    public bool switchFlag
    {
        get { return m_switchFlag; }
        set { m_switchFlag = value; }
    }

    private bool m_centerHitFlag = false;
    public bool centerHitFlag
    {
        get { return m_centerHitFlag; }
        set { m_centerHitFlag = value; }
    }

    private bool m_noUIFlag = false;
    public bool noUIFlag
    {
        get { return m_noUIFlag; }
        set { m_noUIFlag = value; }
    }

    private int m_blockID = 0;
    public int blockID
    {
        get { return m_blockID; }
        set { m_blockID = value; }
    }

    void Awake()
    {
        m_cursorUI = m_tradeUI[1].GetComponent<Move_CursorUI_Script>();
        m_JudgeUI = m_tradeUI[2].GetComponent<Move_CursorUI_Script>();
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
                if(m_switchFlag == true)
                {
                    m_centerHitFlag = true;
                }

                if (m_noUIFlag != true)
                {
                    if (m_cursorUI.DecusuonFlag == false)
                    {
                        m_cursorUI.Decision();
                        // 決定UIをアクティブ
                        m_tradeUI[2].GetComponent<SetChildActiveObject_Script>().OpenUI();
                    }
                    else
                    {
                        m_JudgeUI.Decision();
                    }

                    if (m_cursorUI.DecusuonFlag == true && m_JudgeUI.DecusuonFlag == true)
                    {
                        if (m_JudgeUI.MovePoint == 0)
                        {
                            m_centerHitFlag = true;
                            // 入れ替え
                            if (m_cursorUI.MovePoint < 3)
                            {
                                CreatureList_Script.Get.Trade(m_cursorUI.MovePoint);
                                m_blockID = m_cursorUI.MovePoint;
                            }
                        }
                        else if (m_JudgeUI.MovePoint != 0)
                        {
                            // 決定UIをアクティブ
                            m_tradeUI[2].GetComponent<SetChildActiveObject_Script>().CloseUI();
                            m_cursorUI.DecusuonFlag = false;
                            m_JudgeUI.DecusuonFlag = false;
                        }
                    }
                }
                // 内側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }

            // 左スティックで端を叩く
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                if (m_cursorUI.DecusuonFlag == false)
                {
                    m_cursorUI.CursorUP();
                }
                else
                {
                    m_JudgeUI.CursorUP();
                }
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }
            m_leftStick.BoxDrumHitFlag = false;
        }
        else if (m_rightStick.BoxDrumHitFlag == true)
        {
            // 右スティックで真ん中を叩く
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                if (m_switchFlag == true)
                {
                    m_centerHitFlag = true;
                }

                if (m_noUIFlag != true)
                {
                    if (m_cursorUI.DecusuonFlag == false)
                    {
                        m_cursorUI.Decision();
                        // 決定UIをアクティブ
                        m_tradeUI[2].GetComponent<SetChildActiveObject_Script>().OpenUI();
                    }
                    else if (m_JudgeUI.gameObject.activeInHierarchy == true)
                    {
                        m_JudgeUI.Decision();
                    }

                    if (m_cursorUI.DecusuonFlag == true && m_JudgeUI.DecusuonFlag == true)
                    {
                        if (m_JudgeUI.MovePoint == 0)
                        {
                            m_switchFlag = true;
                            // 入れ替え
                            if (m_cursorUI.MovePoint < 3)
                            {
                                CreatureList_Script.Get.Trade(m_cursorUI.MovePoint);
                                m_blockID = m_cursorUI.MovePoint;
                            }
                        }
                        else if (m_JudgeUI.MovePoint != 0)
                        {
                            // 決定UIをアクティブ
                            m_tradeUI[2].GetComponent<SetChildActiveObject_Script>().CloseUI();
                            m_cursorUI.DecusuonFlag = false;
                            m_JudgeUI.DecusuonFlag = false;
                        }
                    }
                }
                // 内側を叩いた判定フラグを伏せる
                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }
        }
        // 右スティックで端を叩く
        if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
        {
            if (m_cursorUI.DecusuonFlag == false)
            {
                m_cursorUI.CursorDown();
            }
            else 
            {
                m_JudgeUI.CursorDown();
            }
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
        }
        m_rightStick.BoxDrumHitFlag = false;
    }
}
