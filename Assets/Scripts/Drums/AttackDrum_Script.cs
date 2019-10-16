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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 攻撃用のドラムクラスの定義
public class AttackDrum_Script : Drum_Script
{
    // メンバ変数

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
        if (Active == false)
        {
            // 変更する
            return false;
        }

        if (Input.GetKeyDown(KeyCode.S))
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
        // 親オブジェクトを解放する
        m_manager = null;

        // 非アクティブにする
        Active = false;
    }
}
