/*----------------------------------------------------------*/
//  file:      AttackDrum_Scripts.cs					            |
//				 											                    |
//  brief:    攻撃用のドラムクラスのスクリプト		            | 
//              Attack Drum class  				                    |
//															                    |
//  date:	2019.10.9										            |
//															                    |
//  author: Renya Fukuyama									    |
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
    /// <param name="manager">ドラムマネージャー</param>
    public override void Initialize(DrumManager_Script manager)
    {
        // 親オブジェクトを入れる
        m_manager = manager;
    }

    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=ドラムを変更する</returns>
    public override bool Execute()
    {
        // アクティブでないなら
        if (isActive == false)
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
        isActive = false;
    }


    /// <summary>
    /// アクティブフラグのプロパティ
    /// </summary>
    public override bool isActive
    {
        // 取得する
        get { return m_isActive; }
        // 設定する
        set { m_isActive = value; }
    }


    /// <summary>
    /// 衝突を検出した時の処理
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    public void OnTriggerEnter(Collider other)
    {
        // スティックに当たったら処理をする
        if (other.tag == "Stick")
        { 
            // アクティブにする
            isActive = true;
            // このドラムを現在のドラムにする
            m_manager.ChangeDrum(GetComponent<AttackDrum_Script>());
        }
    }


    /// <summary>
    /// 衝突したオブジェクトが離れた時の処理
    /// </summary>
    /// <param name="other">当たっていたオブジェクト</param>
    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }
}
