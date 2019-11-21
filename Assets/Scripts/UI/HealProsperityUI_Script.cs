﻿//
//      FileName @ HealProspenityUI_Scrip.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16 (Wednesday)    
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealProsperityUI_Script : MonoBehaviour
{
    // 体力用のスライダー
    private Slider m_hpSlider = null;

    [SerializeField]
    // スライダー最低値
    private float m_minPoint = 0;
    public float MinPoint
    {
        set { this.m_minPoint = value; }
    }

    [SerializeField]
    // スライダー最大値
    private float m_maxPoint = 0;
    public float MaxPoint
    {
        set { this.m_maxPoint = value; }
    }

    [SerializeField]
    // 現在の値
    private float m_nowPoint = 0;
    public float NowPoint
    {
        get { return this.m_nowPoint; }
        set { this.m_nowPoint = value; }
    }

    // アクティブ時に呼ばれる関数
    void Awake()
    {
        if (m_hpSlider == null)
        {
            m_hpSlider = GetComponent<Slider>();
        }
        m_hpSlider.minValue = this.m_minPoint;
        m_hpSlider.maxValue = this.m_maxPoint;
        m_hpSlider.value = this.m_maxPoint;
    }

    // 開始関数
    void Start()
    {
        if (m_hpSlider == null)
        {
            m_hpSlider = GetComponent<Slider>();
        }
        m_hpSlider.minValue = this.m_minPoint;
        m_hpSlider.maxValue = this.m_maxPoint;
        m_hpSlider.value = this.m_maxPoint;
    }

    // 更新関数
    void Update()
    {
        SetSliderEdgeValue();
        SetNowValue();
    }


    // UIに値を設定する関数
    private void SetSliderEdgeValue()
    {
        m_hpSlider.maxValue = this.m_maxPoint;
        m_hpSlider.minValue = this.m_minPoint;
    }
    // UIに値を設定する関数
    public void SetSliderEdgeValue(float max,float min)
    {
        m_hpSlider.maxValue = this.m_maxPoint = max;
        m_hpSlider.minValue = this.m_minPoint = min;
    }


    // UIの現在の値を設定する関数
    private void SetNowValue()
    {
        m_hpSlider.value = this.m_nowPoint;
    }
    // UIの現在の値を設定する関数
    public void SetNowValue(float now)
    {
        m_hpSlider.value = this.m_nowPoint = now;
    }
}
