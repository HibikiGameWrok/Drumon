﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaitTimeUI_Script : MonoBehaviour
{
    // 体力用のスライダー
    private Slider m_hpSlider = null;

    [SerializeField]
    // スライダー最低値
    private float m_minPoint = 0;
    public float MinPoint
    {
        get { return this.m_minPoint; }
        set { this.m_minPoint = value; }
    }

    [SerializeField]
    // スライダー最大値
    private float m_maxPoint = 0;
    public float MaxPoint
    {
        get { return this.m_maxPoint; }
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

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Slider>() != null)
        {
            m_hpSlider = GetComponent<Slider>();
        }
        m_hpSlider.minValue = this.m_minPoint;
        m_hpSlider.maxValue = this.m_maxPoint;
        m_hpSlider.value = this.m_nowPoint;
    }

    // Update is called once per frame
    void Update()
    {
        // UIに値を設定する関数
        SetSliderEdgeValue();
        // UIの現在の値を設定する関数
        SetNowValue();
    }

    // UIに値を設定する関数
    private void SetSliderEdgeValue()
    {
        m_hpSlider.maxValue = this.m_maxPoint;
        m_hpSlider.minValue = this.m_minPoint;
    }

    // UIに値を設定する関数(読み込み専用)
    public void SetSliderEdgeValue(float max, float min)
    {
        m_hpSlider.maxValue = this.m_maxPoint = max;
        m_hpSlider.minValue = this.m_minPoint = min;
    }


    // UIの現在の値を設定する関数
    private void SetNowValue()
    {
        m_hpSlider.value = this.m_nowPoint;
    }
    // UIの現在の値を設定する関数(読み込み専用)
    public void SetNowValue(float now)
    {
        m_hpSlider.value = this.m_nowPoint = now;
    }
}
