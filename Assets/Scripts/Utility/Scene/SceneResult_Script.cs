/*----------------------------------------------------------*/
//  file:      SceneResult_Script.cs                               |
//				 											                    |
//  brief:    探索シーンのスクリプト			                    |
//															                    |
//  date:	2019.1.9									                |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResult_Script : IScene_Script
{

    private GameObject m_resultEvent = null;
    private ProgressResultEvent_Script m_resultEvent_Script = null;

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
        if (m_resultEvent_Script.finishEventFlag == true)
        {

            // SEを鳴らす
            m_manager.Audio.PlaySE(SfxType.teleport);
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Ending.Name);
            return SceneID.SCENE_ENDING;

        }
        // 継続する
        return SceneID.CONTINUE;
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="manager"></param>
    public override void Initialize(SceneManager_Script manager)
    {
        // 親オブジェクトを取得する
        m_manager = manager;

        // BGMを再生する
        m_manager.Audio.PlayBGM(BfxType.bgm_Title);

        m_resultEvent = GameObject.Find("ResultEvent");
        m_resultEvent_Script = m_resultEvent.GetComponent<ProgressResultEvent_Script>();
    }
}
