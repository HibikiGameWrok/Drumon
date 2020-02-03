using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildActiveObject_Script : MonoBehaviour
{
    private bool m_activeNowFlag = false;
    public bool activeNowFlag
    {
        get { return m_activeNowFlag; }
    }

    void Start()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf == false)
            {
                m_activeNowFlag = false;
            }
            else if (child.gameObject.activeSelf == true)
            {
                m_activeNowFlag = true;
            }
        }
    }

    // UIの表示
    public void OpenUI()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf == false)
            {
                child.gameObject.SetActive(true);
                m_activeNowFlag = true;
            }
        }
    }
    // UIの非表示
    public void CloseUI()
    {
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                child.gameObject.SetActive(false);
                m_activeNowFlag = false;
            }
        }
    }
}
