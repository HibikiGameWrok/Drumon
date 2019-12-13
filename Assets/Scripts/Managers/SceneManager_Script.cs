﻿/*----------------------------------------------------------*/
//  file:      SceneManager_Script.cs                             |
//				 											                    |
//  brief:    シーン管理のスクリプト			                    |
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


// シーン管理クラス
public class SceneManager_Script
{
    // 現在の処理
    private IScene_Script m_currentScene;
    // タイトルシーン
    private IScene_Script m_title;
    // 探索シーン
    private IScene_Script m_revised;
    // バトルシーン
    private IScene_Script m_battle;
    // オーディオ
    private AudioManager_Script m_audio;

    /// <summary>
    /// Audioプロパティ
    /// </summary>
    public AudioManager_Script Audio => m_audio;


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(AudioManager_Script audio)
    {
        // Audio
        m_audio = audio;

        // Scene
        CreateScene();

        // 最初のシーンを設定する
        m_currentScene = m_title;
        m_currentScene.Initialize(this);
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public bool Execute()
    {
        // 現在のシーンを実行する
        SceneID result = m_currentScene.Execute();

        // 戻り値で処理を分ける
        switch(result)
        {
            case SceneID.CONTINUE:
                // シーンを継続する
                break;
            case SceneID.SCENE_TITLE:
                // タイトルシーンへ
                ChangeScene(m_title);
                break;
            case SceneID.SCENE_REVISED:
                // 探索シーン
                ChangeScene(m_revised);
                break;
            case SceneID.SCENE_BATTLE:
                ChangeScene(m_battle);
                break;
        }

        // 正常に更新
        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public void Dispose()
    {
        m_currentScene.Dispose();
    }


    /// <summary>
    /// シーンを変更する
    /// </summary>
    /// <param name="nextScene">次のシーン</param>
    public void ChangeScene(IScene_Script nextScene)
    {
        // 終了処理をする
        m_currentScene.Dispose();
        // 次のシーンを設定する
        m_currentScene = nextScene;
        // 遷移する
        TransitionManager_Script.StartTransition(m_currentScene.Name);

        // 初期化する
        m_currentScene.Initialize(this);
    }


    /// <summary>
    /// シーンを生成する
    /// </summary>
    private void CreateScene()
    {
        // TitleScene
        m_title = new SceneTitle_Script();
        m_title.Name = "TitleScene";
        // Revised
        m_revised = new SceneRevised_Script();
        m_revised.Name = "Revised";
        // BattleScene
        m_battle = new SceneBattle_Script();
        m_battle.Name = "BattleScene";
    }
}
