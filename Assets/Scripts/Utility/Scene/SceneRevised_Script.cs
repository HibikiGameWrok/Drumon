/*----------------------------------------------------------*/
//  file:      SceneRevised_Script.cs                              |
//				 											                    |
//  brief:    探索シーンのスクリプト			                    |
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


// 探索シーンクラス
public class SceneRevised_Script : IScene_Script
{
    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        // BGMを止める
        m_manager.Audio.AttachBGMSource.Stop();
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override SceneID Execute()
    {
        // バトルシーンへ
        if (Input.GetKeyDown(KeyCode.B))
        {
            return SceneID.SCENE_BATTLE;
        }
        // リザルトシーンへ
        // return SceneID.SCENE_RESULT;
        // タイトルシーンへ
        if (Input.GetKeyDown(KeyCode.R))
        {
            return SceneID.SCENE_TITLE;
        }

        // 継続する
        return SceneID.CONTINUE;
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
        m_manager.Audio.PlayBGM(BfxType.bgm_Search);
    }
}
