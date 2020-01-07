using System;
///
///     UI_Fade_Script.cs
///
///     UIのフェード処理をする既定クラス(継承によってUIのタイプが変わる)
///
///     ヨシヤス　ヒビキ
///
///     2020 / 01 / 07 火曜日
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fade_SetAlpha : MonoBehaviour
{
    protected abstract void SetAlpha();
}


public class UI_Fade_Script : Fade_SetAlpha
{
    // フェードする速度
    [SerializeField]
    protected float m_fadeSpeed = 0.02f;

    // パネルの色、不透明
    protected float m_red, m_green, m_blue, m_alfa;

    // フェードアウト処理
    protected bool m_isFadeOut = false;
    public bool IsFadeOut
    {
        get { return m_isFadeOut; }
        set { m_isFadeOut = value; }
    }
    // フェードイン処理
    protected bool m_isFadeIn = false;
    public bool IsFadeIn
    {
        get { return m_isFadeIn; }
        set { m_isFadeIn = value; }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isFadeOut = true;
        }

        // フェードアウトフラグによって処理
        if (m_isFadeOut == true)
        {
            StartFadeOut();
        }

        // フェードインフラグによって処理
        if (m_isFadeIn == true)
        {
            StartFadeIn();
        }
    }


    // フェードアウトする処理
    protected void StartFadeOut()
    {
        m_alfa += m_fadeSpeed;
        SetAlpha();
        if (m_alfa >= 1)
        {
            m_alfa = 1;
            IsFadeOut = false;
        }
    }

    // フェードインする処理
    protected void StartFadeIn()
    {
        m_alfa += m_fadeSpeed;
        SetAlpha();
        if (m_alfa <= 1)
        {
            m_alfa = 0;
            IsFadeIn = false;
        }
    }

    protected override void SetAlpha()
    {

    }
}
