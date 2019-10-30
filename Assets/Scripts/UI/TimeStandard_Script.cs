//
//      FileName @ TimeStandard_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16      
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStandard_Script : MonoBehaviour
{
    // 初期化定数
    private const float ZERO_TIME = 0.0f;

    // 経過時間を保持
    private float m_nowTimer = 0.0f;

    // 最大カウント値
    [SerializeField]
    private float m_maxTimer = 10.0f;
    // タイマーを取得するプロパティ
    public float NowTimer
    {
        get { return m_nowTimer; }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // タイマーを更新
        m_nowTimer += Time.deltaTime;
    }

    // タイムが設定値まで達した時にフラグを返す関数
    public bool TimerMax()
    {
        if (m_nowTimer >= m_maxTimer)
        {
            return true;
        }
        return false;
    }

    // タイマーをリセット
    public void TimerReset()
    {
        // タイマーを初期化
        m_nowTimer = ZERO_TIME;
    }
}
