///
///     ProgressResultEvent_Script.cs
///
///     リザルトシーンのイベントを進行させるスクリプト
///
///     ヨシヤス　ヒビキ
///
///     2020 / 01 / 07 火曜日
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressResultEvent_Script : MonoBehaviour
{
    [SerializeField]
    private GameObject m_panelUI = null;
    [SerializeField]
    private GameObject m_textUI = null;

    // Panelコンポーネントをフェードさせるスクリプト
    private PanelUI_Fade_Script m_panelUI_Script = null;
    // Textコンポーネントをフェードさせるスクリプト
    private TextUI_Fade_Script m_textUI_Fade_Script = null;
    // Textコンポーネントの文章を変えるスクリプト
    private TextUI_Change_Script m_textUI_Change_Script = null;

    // コンポーネントが無かった時に知らせるフラグ
    private bool m_errorFlag = false;

    // イベントが終了した時に立つフラグ
    private bool m_finishEventFlag = false;
    public bool finishEventFlag
    {
        get { return m_finishEventFlag; }
    }


    // Start is called before the first frame update
    void Start()
    {
        // error処理
        if ((m_panelUI == null) || (m_textUI == null))
        {
            m_errorFlag = true;
        }

        if (m_errorFlag != true)
        {
            m_panelUI_Script = m_panelUI.GetComponent<PanelUI_Fade_Script>();
            m_textUI_Fade_Script = m_textUI.GetComponent<TextUI_Fade_Script>();
            m_textUI_Change_Script = m_textUI.GetComponent<TextUI_Change_Script>();
            StartCoroutine("EventStep");
        }
    }


    private IEnumerator EventStep()
    {
        // 1. パネルをフェードアウト
        m_panelUI_Script.IsFadeOut = true;

        yield return new WaitForSeconds(3.0f);

        // 2. テキストをフェードアウト
        m_textUI_Fade_Script.IsFadeOut = true;

        yield return new WaitForSeconds(2.0f);

        // 3. テキストをフェードイン
        m_textUI_Fade_Script.IsFadeIn = true;

        yield return new WaitForSeconds(2.0f);

        // 4.テキストの文章を変更しフェードアウト
        m_textUI_Change_Script.NextRowText();
        m_textUI_Fade_Script.IsFadeOut = true;

        yield return new WaitForSeconds(2.0f);

        // 5. テキストをフェードイン
        m_textUI_Fade_Script.IsFadeIn = true;

        yield return new WaitForSeconds(2.0f);

        // 6.テキストの文章を変更しフェードアウト
        m_textUI_Change_Script.NextRowText();
        m_textUI_Fade_Script.IsFadeOut = true;

        yield return new WaitForSeconds(2.0f);

        // 7. テキストをフェードイン
        m_textUI_Fade_Script.IsFadeIn = true;

        yield return new WaitForSeconds(2.0f);

        // 8.テキストの文章を変更しフェードアウト
        m_textUI_Change_Script.NextRowText();
        m_textUI_Fade_Script.IsFadeOut = true;

        yield return new WaitForSeconds(2.0f);

        // 9.完了フラグを立たせる
        m_finishEventFlag = true;
    }
}
