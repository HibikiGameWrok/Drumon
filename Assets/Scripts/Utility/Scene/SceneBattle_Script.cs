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
    // バトルマネージャー
    private BattleManager_Script m_battleManager;

    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {
        m_manager.Audio.AttachBGMSource.Stop();

        m_battleManager = null;
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public override SceneID Execute()
    {
        if((BattleManager_Script.Get != null)&& (m_battleManager == null))
            m_battleManager = BattleManager_Script.Get;

        // バトルが終了したかで判断する
        if (Input.GetKeyDown(KeyCode.G) || m_battleManager.IsFinish.Value == true)
        {
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
    }
}
