/*----------------------------------------------------------*/
//  file:   SceneBattleTutorial_Script.cs                   |
//				 											|
//  brief:  バトルチュートリアルシーンのスクリプト          |
//															|
//  date:	2019.1.17                                       |
//                                                          |                               
//  author: Naoki Akiyama                                   |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBattleTutorial_Script : IScene_Script
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
        // バトルが終了したかで判断する
        if (TutorialManager_Script.Get.IsAllFinish.Value.Equals(true))
        {
            TransitionManager_Script.StartTransition(m_manager.Title.Name);

            return SceneID.SCENE_TITLE;
        }

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
        m_manager.Audio.PlayBGM(BfxType.bgm_Tutorial);
    }
}
