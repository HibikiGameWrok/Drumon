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

    private int m_movePoint = 0;
    public int MovePoint
    {
        get { return m_movePoint; }
    }

    private bool m_decisionFlag = false;
    public bool DecusuonFlag
    {
        get { return m_decisionFlag; }
    }

    [SerializeField]
    private GameObject m_decisionObject = null;


    // 決定
    public void Decision()
    {
        if (m_decisionFlag == false)
        {
            m_decisionFlag = true;
            if (m_decisionObject != null || m_decisionObject.activeSelf == false)
            {
                m_decisionObject.SetActive(true);
            }        
        }
        else
        {
            if (m_decisionObject != null || m_decisionObject.activeSelf == true)
            {
                m_decisionObject.GetComponent<Move_CursorUI_Script>().Decision();
            }
        }
    }

    // cursorを上に移動
    public void CursorUP()
    {
        if (m_decisionFlag != true)
        {
            m_movePoint++;
            if (m_movePoint > 3)
            {
                m_movePoint = 0;
            }
            MoveCuresor();
        }
        else
        {
            if (m_decisionObject != null || m_decisionObject.activeSelf == true)
            {
                m_decisionObject.GetComponent<Move_CursorUI_Script>().CursorUP();
            }
        }
    }

    // cursorを下に移動
    public void CursorDown()
    {
        if (m_decisionFlag != true)
        {
            m_movePoint--;
            if (m_movePoint < 0)
            {
                m_movePoint = 3;
            }
            MoveCuresor();
        }
        else
        {
            if (m_decisionObject != null || m_decisionObject.activeSelf == true)
            {
                m_decisionObject.GetComponent<Move_CursorUI_Script>().CursorDown();
            }
        }
    }

    // cursorを移動
    private void MoveCuresor()
    {
        switch (m_movePoint)
        {
            case 0:
                m_cursorUI.transform.position = m_point[0].transform.localPosition + MOVE_POS;
                break;
            case 1:
                m_cursorUI.transform.position = m_point[1].transform.localPosition + MOVE_POS;
                break;
            case 2:
                m_cursorUI.transform.position = m_point[2].transform.localPosition + MOVE_POS;
                break;
            case 3:
                m_cursorUI.transform.position = m_point[3].transform.localPosition + MOVE_POS;
                break;
            default:
                break;
        }
    }
}
