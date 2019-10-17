/*----------------------------------------------------------*/
//  file:      AttackDrum_Scripts.cs					    |
//				 											|
//  brief:    攻撃用のドラムクラスのスクリプト		        |
//              Attack Drum class  				            |
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

// 攻撃用のドラムクラスの定義
public class AttackDrum_Script : Drum_Script
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

        Debug.Log("Attack init");

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
        if (Active == false)
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

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Stick")
        {
            //if (gameObject.tag == "Drum_Base") 
            {
                Debug.Log("Stick hit");
                Active = true;

                m_manager.ChangeDrum(GetComponent<AttackDrum_Script>());
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }
}
