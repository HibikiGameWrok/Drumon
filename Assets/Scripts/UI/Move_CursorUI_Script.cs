using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_CursorUI_Script : MonoBehaviour
{
    // 定数
    // 位置の調整
    private readonly Vector3 MOVE_POS = new Vector3(-0.05f, 2.07f, 2.9f);

    [SerializeField]
    private Image m_cursorUI = null;

    [SerializeField]
    private GameObject[] m_point = null;

    private int MAX_POINT;
    private int MIN_POINT = 0;

    private int m_movePoint = 0;
    public int MovePoint
    {
        get { return m_movePoint; }
    }

    private bool m_decisionFlag = false;
    public bool DecusuonFlag
    {
        get { return m_decisionFlag; }
        set { m_decisionFlag = value; }
    }

    [SerializeField]
    private GameObject m_decisionObject = null;


    void Awake()
    {
        MAX_POINT = m_point.Length;
    }

    // 決定
    public void Decision()
    {
        if (m_decisionFlag == false)
        {
            m_decisionFlag = true;
        }
    }

    // cursorを上に移動
    public void CursorUP()
    {
        m_movePoint++;
        if (m_movePoint > MAX_POINT - 1)
        {
            m_movePoint = MIN_POINT;
        }
        MoveCuresor();
    }

    // cursorを下に移動
    public void CursorDown()
    {

        m_movePoint--;
        if (m_movePoint < MIN_POINT)
        {
            m_movePoint = MAX_POINT - 1;
        }
        MoveCuresor();
    }

    // cursorを移動
    private void MoveCuresor()
    {
        switch (m_movePoint)
        {
            case 0:
                m_cursorUI.transform.position = m_point[0].transform.position;
                break;
            case 1:
                m_cursorUI.transform.position = m_point[1].transform.position;
                break;
            case 2:
                m_cursorUI.transform.position = m_point[2].transform.position;
                break;
            case 3:
                m_cursorUI.transform.position = m_point[3].transform.position;
                break;
            default:
                break;
        }
    }
}
