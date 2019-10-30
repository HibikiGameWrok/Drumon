using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI_Script : MonoBehaviour
{
    private GameObject m_parent;

    // 左スティック
    StickLeft_Script m_leftStick;
    // 右スティック
    StickRight_Script m_rightStick;

    // Start is called before the first frame update
    void Start()
    {
        m_parent = this.transform.parent.gameObject;

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();

        //m_parent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_leftStick.OpenUIFlag == true || m_rightStick.OpenUIFlag == true || Input.GetKeyDown(KeyCode.Q))
        //{
        //    m_parent.SetActive(true);
        //}
    }
}
