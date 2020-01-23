/*----------------------------------------------------------*/
//  file:      SceneTitle_Script.cs                                   |
//				 											                    |
//  brief:    タイトルシーンのスクリプト			                |
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


// タイトルシーンクラス
public class SceneTitle_Script : IScene_Script
{
    // タイトルドラム
    private TitleDrum_Script m_titleDrum = null;

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
        // Spaceキーまたはゲームプレイを選択
        if (Input.GetKeyDown(KeyCode.Space) ||
            (m_titleDrum.SelectCount == 0 && m_titleDrum.Decision == true))
        {
            // SEを鳴らす
            m_manager.Audio.PlaySE(SfxType.WaterGun);
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.Revised.Name);

            return SceneID.SCENE_REVISED;
        }
        else if ((m_titleDrum.SelectCount == 1 && m_titleDrum.Decision == true) || Input.GetKeyDown(KeyCode.T))
        {
            // SEを鳴らす
            m_manager.Audio.PlaySE(SfxType.WaterGun);
            // 非同期処理のSceneロード
            TransitionManager_Script.StartTransition(m_manager.CaptureTutorial.Name);

            return SceneID.SCENE_CAPTURETUTORIAL;
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
        m_manager.Audio.PlayBGM(BfxType.bgm_Title);

        m_titleDrum = GameObject.Find("TitleDrum").GetComponent<TitleDrum_Script>();
    }
}
