/*----------------------------------------------------------*/
//  file:      Drum_Script.cs					                        |
//				 															    |
//  brief:    ドラムの基底クラスのスクリプト				    |
//              Drum base class 				                        |
//																				|
//  date:	2019.10.9												    |
//																				|
//  author: Renya Fukuyama										|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Drumクラスの定義
public class Drum_Script
{
    // メンバ変数
    // ドラムマネージャーを入れる変数
    protected DrumManager_Script m_manager;
    // 現在アクティブかどうか
    private bool m_isActive;

    /// <summary>
    /// 初期化処理 
    /// </summary>
    public virtual void Initialize(DrumManager_Script manager)
    {
        // 親オブジェクトを入れる
        m_manager = manager;
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=ドラムを変更する</returns>
    public virtual bool Execute()
    {
        if(m_isActive == false)
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
    public virtual void Dispose()
    {
        // 親オブジェクトを解放する
        m_manager = null;
    }


    /// <summary>
    /// アクティブフラグのプロパティ
    /// </summary>
    public bool Active
    {
        // アクティブフラグを取得する
        get { return m_isActive; }
        // アクティブフラグを設定する
        set { m_isActive = value; }
    }

    /// <summary>
    /// 当たり判定の検出をする
    /// </summary>
    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "")
            m_isActive = true;
    }
}
