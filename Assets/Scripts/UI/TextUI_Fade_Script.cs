///
///     TextUI_Fade_Script.cs
///
///     テキストUIをフェードするスクリプト
///
///     ヨシヤス　ヒビキ
///
///     2020 / 01 / 07 火曜日
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI_Fade_Script : UI_Fade_Script
{
    private Text m_text = null;

    private void Start()
    {
        if (this.GetComponent<Text>() != null)
        {
            m_text = this.GetComponent<Text>();
            m_red = m_text.color.r;
            m_green = m_text.color.g;
            m_blue = m_text.color.b;
            m_alfa = m_text.color.a;
        }
    }

    // α値を設定
    protected override void SetAlpha()
    {
        m_text.color = new Color(m_red, m_green, m_blue, m_alfa);
    }
}
