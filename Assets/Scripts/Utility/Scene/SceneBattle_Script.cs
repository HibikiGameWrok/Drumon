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
using UnityEngine.SceneManagement;

// バトルシーンクラス
public class SceneBattle_Script : IScene_Script
{

    //private GameObject m_player = null;

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
        // もしOtosを倒したら
        if (BattleManager_Script.Get.EnemyCreature.OtsoFlag.Equals(true))
        {
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Result.Name);
            return SceneID.SCENE_RESULT;
        }

        // バトルが終了したかで判断する
        if (BattleManager_Script.Get.IsFinish.Value.Equals(true))
        {
            // 非同期処理のSceneアンロード
            TransitionManager_Script.StartTransition_UnloadScene(this.Name);

            // プレイヤーを非アクティブにしておく
            //if (m_player.activeSelf == false)
            //    m_player.gameObject.SetActive(true);
            if (m_manager.Player.activeSelf == false)
                m_manager.Player.SetActive(true);

            return SceneID.SCENE_REVISED;
        }

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
        m_manager.Audio.PlayBGM(BfxType.bgm_Battle);

        //m_player = SceneManager.GetSceneByName("Revised").GetRootGameObjects().
    }
}
