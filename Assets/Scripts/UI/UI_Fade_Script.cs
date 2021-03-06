﻿using System;
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

    // フェードが完了した時のフラグ
    protected bool m_isCompFlag = false;
    public bool IsFadeComp
    {
        get { return m_isCompFlag; }
        set { m_isCompFlag = value; }
    }

    [SerializeField]
    protected bool m_startFadeOutFlag = false;
    [SerializeField]
    protected bool m_startFadeInFlag = false;

    // Update is called once per frame
    protected void Update()
    {
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

        if(m_startFadeOutFlag)
        {
            StartFadeOut();
        }
        if (m_startFadeInFlag)
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
            m_isFadeOut = false;
            m_startFadeOutFlag = false;
            m_isCompFlag = true;
        }
    }

    // フェードインする処理
    protected void StartFadeIn()
    {
        m_alfa -= m_fadeSpeed;
        SetAlpha();
        if (m_alfa <= 0)
        {
            m_alfa = 0;
            m_isFadeIn = false;
            m_startFadeInFlag = false;
            m_isCompFlag = true;
        }
    }

    protected void Start()
    {
        m_isCompFlag = false;
    }

    protected override void SetAlpha()  { }

    public bool AlphaMax()
    {
        bool alphaFlag = false;
        if(m_alfa >= 1)
        {
            alphaFlag = true;
        }
        else if(m_alfa <= 0)
        {
            alphaFlag = false;
        }
        return alphaFlag;
    }
}
