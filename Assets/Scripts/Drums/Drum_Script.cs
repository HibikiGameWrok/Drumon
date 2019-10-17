/*----------------------------------------------------------*/
//  file:      Drum_Script.cs					            |
//				 											|
//  brief:    ドラムの基底クラスのスクリプト				|
//              Drum base class 				            |
//															|
//  date:	2019.10.9										|
//															|
//  author: Renya Fukuyama									|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Drumクラスの定義
public abstract class Drum_Script : MonoBehaviour
{
    // メンバ変数
    // ドラムマネージャーを入れる変数
    protected DrumManager_Script m_manager;
    // 現在アクティブかどうか
    protected bool m_isActive;


    /// <summary>
    /// 初期化処理 
    /// </summary>
    public abstract void Initialize(DrumManager_Script manager);


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=ドラムを変更する</returns>
    public abstract bool Execute();


    /// <summary>
    /// 終了処理
    /// </summary>
    public abstract void Dispose();


    /// <summary>
    /// アクティブフラグのプロパティ
    /// </summary>
    public abstract bool Active
    {
        // アクティブフラグを取得する
        get;
        // アクティブフラグを設定する
        set;
    }

}
