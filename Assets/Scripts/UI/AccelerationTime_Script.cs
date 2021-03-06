﻿//
//      FileName @ TimeStandard_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16      
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationTime_Script : MonoBehaviour
{
    // 初期化定数
    private const float ZERO_TIME = 0.0f;

    // 経過時間を保持
    private float m_nowTimer = 0.0f;
    // タイマーを取得するプロパティ
    public float NowTimer
    {
        get { return m_nowTimer; }
    }

    // 最大カウント値
    private float m_maxTimer = 0.0f;
    // 最大値を設定するプロパティ
    public float MaxTimer
    {
        get { return m_maxTimer; }
        set { m_maxTimer = value; }
    }

    // 停止フラグ
    private bool m_stopFlag = false;
    public bool StopFlag
    {
        get { return m_stopFlag;  }
        set { m_stopFlag = value; }
    }

    // Update is called once per frame
    void Update()
    {
        TimerReset();
        CountUP();
    }

    private void CountUP()
    {
        if (m_stopFlag != true)
        {
            // タイマーを更新
            m_nowTimer += Time.deltaTime;
        }
    }

    public void ChengeTime()
    {
        if(m_maxTimer != MaxTimer)
        {
            TimerReset();
        }
    }

    // タイムが設定値まで達した時にフラグを返す関数
    public bool TimerMax()
    {
        if (m_nowTimer >= m_maxTimer)
        {
            return true;  // 最大値に達した
        }
        return false;
    }

    

    // タイマーをリセット
    public void TimerReset()
    {
        if (TimerMax() == true)
        {
            // タイマーを初期化
            m_nowTimer = ZERO_TIME;
        }
    }
}
