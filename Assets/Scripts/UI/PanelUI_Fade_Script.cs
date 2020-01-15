///
///     PanelUI_Fade_Script.cs
///
///     パネルコンポーネントでフェードをするスクリプト
///
///     ヨシヤス　ヒビキ
///
///     2020 / 01 / 07 火曜日
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUI_Fade_Script : UI_Fade_Script
{
    // コンポーネントを保持する変数
    private Image m_imagePanel = null;

    private void Start()
    {
        if (this.GetComponent<Image>() != null)
        {
            m_imagePanel = this.GetComponent<Image>();
            m_red = m_imagePanel.color.r;
            m_green = m_imagePanel.color.g;
            m_blue = m_imagePanel.color.b;
            m_alfa = m_imagePanel.color.a;
        }
    }


    // α値を設定
    protected override void SetAlpha()
    {
        m_imagePanel.color = new Color(m_red,m_green,m_blue,m_alfa);
    }
}
