//
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
    private Slider m_hpSlider;

    // スライダー最大値
    private float m_maxPoint;
    public float MaxPoint
    {
        set { this.m_maxPoint = value; }
    }

    // スライダー最低値
    private float m_minPoint;
    public float MinPoint
    {
        set { this.m_minPoint = value; }
    }

    // 現在の値
    private float m_nowPoint;
    public float NowPoint
    {
        set { this.m_nowPoint = value; }
    }

    // アクティブ時に呼ばれる関数
    void Awake()
    {
        m_hpSlider = GetComponent<Slider>();
    }

    // 更新関数
    void Update()
    {
        SetSliderValue();
        SetNowValue();
    }

    // UIに値を設定する関数
    private void SetSliderValue()
    {
        m_hpSlider.maxValue = m_maxPoint;
        m_hpSlider.maxValue = m_minPoint;
    }

    // UIの現在の値を設定する関数
    private void SetNowValue()
    {
        m_hpSlider.value = m_nowPoint;
    }

}
