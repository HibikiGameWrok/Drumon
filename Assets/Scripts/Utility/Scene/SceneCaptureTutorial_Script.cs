/*----------------------------------------------------------*/
//  file:   SceneCaptureTutorial_Script.cs                  |
//				 											|
//  brief:  キャプチャーチュートリアルシーンのスクリプト    |
//															|
//  date:	2019.1.9                                        |
//															|
//  author: Renya Fukuyama									|
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCaptureTutorial_Script : IScene_Script
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
        if (TutorialManager_Script.Get.IsFinish.Value.Equals(true))
        {
            TransitionManager_Script.StartTransition(m_manager.BattleTutorial.Name);

            return SceneID.SCENE_BATTLETUTORIAL;
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
