/*----------------------------------------------------------*/
//  file:      SceneEnding_Script.cs                                |
//				 											                    |
//  brief:    エンディングシーンのスクリプト			        |
//															                    |
//  date:	2019.1.10									                |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SceneEnding_Script : IScene_Script
{
    private GameObject m_endingEvent = null;
    private EndingEvent_Script m_endingEvent_Script = null;

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
        if (m_endingEvent_Script.finishFlag == true)
        {
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Title.Name);
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

        m_endingEvent = GameObject.Find("StaffRollCanvas");
        m_endingEvent_Script = m_endingEvent.GetComponent<EndingEvent_Script>();
    }
}
