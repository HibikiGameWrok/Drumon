using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackAbilityNameUI_Script : MonoBehaviour
{
    // TextUIコンポーネントを保持する変数
    private Text m_textUI = null;

    // TextUIに表示する文字列 
    private string m_attackName = "";

    // 後で消す変数
    private float m_count = 0;
    private bool m_drawTextFlag = false;

    public string AttackName
    {
        set { m_attackName = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        m_textUI = this.GetComponent<Text>();
        m_textUI.text = m_attackName;
    }

    void Update()
    {
        if (m_drawTextFlag == true)
        {
            m_count += Time.deltaTime;
            if (m_count >= 2.0f)
            {
                m_textUI.text = "";
                m_drawTextFlag = false;
                m_count = 0;
            }
        } 
    }

    public void DrawStringAttackName(string attackName)
    {
        if ((attackName != null) || (attackName != "None"))
        {
            m_textUI.text = attackName;
            m_drawTextFlag = true;
        }
    }


}
