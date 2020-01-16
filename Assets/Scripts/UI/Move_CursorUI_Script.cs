using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_CursorUI_Script : MonoBehaviour
{

    [SerializeField]
    private Image m_cursorUI = null;

    [SerializeField]
    private GameObject[] m_point = null;

    private int m_movePoint = 0;

    // Start is called before the first frame update
    void Start()
    {
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
                m_cursorUI.transform.position = m_point[0].transform.localPosition;
                break;
            case 1:
                m_cursorUI.transform.position = m_point[1].transform.localPosition;
                break;
            case 2:
                m_cursorUI.transform.position = m_point[2].transform.localPosition;
                break;
            case 3:
                m_cursorUI.transform.position = m_point[3].transform.localPosition;
                break;
            default:
                break;
    }

}
