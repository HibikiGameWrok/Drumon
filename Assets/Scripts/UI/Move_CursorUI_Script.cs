﻿using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CursorUP();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            CursorDown();
        }
        Debug.Log(m_cursorUI.transform.position);
    }

    // cursorを上に移動
    public void CursorUP()
    {
        m_movePoint++;
        if (m_movePoint > 3)
        {
            m_movePoint = 0;
        }
        MoveCuresor();
    }

    // cursorを下に移動
    public void CursorDown()
    {
        m_movePoint--;
        if(m_movePoint < 0)
        {
            m_movePoint = 3;
        }
        MoveCuresor();
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
