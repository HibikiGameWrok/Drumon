/*----------------------------------------------------------*/
//  file:      SceneBattle_Script.cs                                 |
//				 											                    |
//  brief:    バトルシーンのスクリプト			                    |
//															                    |
//  date:	2019.12.13									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// バトルシーンクラス
public class SceneBattle_Script : IScene_Script
{

    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        m_manager.Audio.AttachBGMSource.Stop();
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override bool Execute()
    {
        return true;
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="manager">親オブジェクト</param>
    public override void Initialize(SceneManager_Script manager)
    {
        // 親オブジェクトを取得する
        m_manager = manager;

        // BGMを再生する
        m_manager.Audio.PlayBGM(BfxType.bgm_Battle);
    }
}
