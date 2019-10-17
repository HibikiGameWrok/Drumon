/*----------------------------------------------------------*/
//  file:      HealDrum_Scripts.cs						    |
//				 											|
//  brief:    回復用のドラムクラスのスクリプト		        |
//              Heal Drum class  				            |
//															|
//  date:	2019.10.9										|
//															|
//  author: Renya Fukuyama									|
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回復用のドラムクラスの定義
public class HealDrum_Script : Drum_Script
{

    // メンバ変数


    /// <summary>
    /// デフォルト関数
    /// </summary>
    void Start()
    {
        // 何もしない
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize(DrumManager_Script manager)
    {
        // 親オブジェクトを入れる
        m_manager = manager;
        
        // アクティブにする
        Active = true;
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=ドラムを変更する</returns>
    public override bool Execute()
    {
        // アクティブでないなら
        if(Active == false)
        {
            // 変更する
            return false;
        }

        // 継続する
        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {        
        // 非アクティブにする
        Active = false;
    }


    /// <summary>
    /// アクティブフラグのプロパティ
    /// </summary>
    public override bool Active
    {
        get { return m_isActive; }

        set { m_isActive = value; }
    }

    /// <summary>
    /// 当たり判定の検出をする
    /// </summary>
    /// <param name="col">衝突した相手</param>
    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Stick")
        {
            // アクティブにする
            Active = true;
            // 回復用のドラムに変更する
            m_manager.ChangeDrum(m_manager.HealDrum);
        }
    }


    /// <summary>
    /// 当たり判定から外れた時
    /// </summary>
    /// <param name="col">衝突した相手</param>
    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }
}
