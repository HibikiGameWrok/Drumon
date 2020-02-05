using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualAdjustMentUI_Script : MonoBehaviour
{
    private bool m_switchFlag = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (m_switchFlag == true)
            {
                this.GetComponent<SetChildActiveObject_Script>().OpenUI();
                m_switchFlag = false;
            }
            else 
            {
                this.GetComponent<SetChildActiveObject_Script>().CloseUI();
                m_switchFlag = true;
            }
        }
    }
}
