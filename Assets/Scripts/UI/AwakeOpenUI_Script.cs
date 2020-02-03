using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeOpenUI_Script : MonoBehaviour
{
    bool m_fadeFlag = false;
    public bool fadeFlag
    {
        set { m_fadeFlag = value; }
    }
    float m_time = 0;


    void Start()
    {
        m_fadeFlag = false;
        m_time = 0;
    }

    void Update()
    {
        if(m_fadeFlag == true)
        {
            m_time += Time.deltaTime;
        }
        if(m_time > 3.0f)
        {
            this.GetComponent<SetChildActiveObject_Script>().CloseUI();
        }
    }
}
