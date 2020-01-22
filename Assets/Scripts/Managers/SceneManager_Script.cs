/*----------------------------------------------------------*/
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
using UnityEngine.SceneManagement;
using UniRx;

// シーン管理クラス
public class SceneManager_Script : SingletonBase_Script<SceneManager_Script>
{
    // 現在の処理
    private IScene_Script m_currentScene;
    // タイトルシーン
    private IScene_Script m_title;
    // 探索シーン
    private IScene_Script m_revised;
    // バトルシーン
    private IScene_Script m_battle;
    // チュートリアルシーン
    private IScene_Script m_captureTutorial;
    private IScene_Script m_battleTutorial;
    // リザルトシーン
    private IScene_Script m_result;
    // エンディングシーン
    private IScene_Script m_ending;
    // オーディオ
    private AudioManager_Script m_audio;

    /// <summary>
    /// Audioプロパティ
    /// </summary>
    public AudioManager_Script Audio => m_audio;


    /// <summary>
    /// シーンのプロパティ
    /// </summary>
    public SceneTitle_Script Title => (SceneTitle_Script)m_title;
    public SceneRevised_Script Revised => (SceneRevised_Script)m_revised;
    public SceneBattle_Script Battle => (SceneBattle_Script)m_battle;
    public SceneResult_Script Result => (SceneResult_Script)m_result;
    public SceneCaptureTutorial_Script CaptureTutorial => (SceneCaptureTutorial_Script)m_captureTutorial;
    public SceneBattleTutorial_Script BattleTutorial => (SceneBattleTutorial_Script)m_battleTutorial;
    public SceneEnding_Script Ending => (SceneEnding_Script)m_ending;


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(AudioManager_Script audio)
    {
        // 既にあるなら処理しない
        if (CheckInstance() == false)
            return;

        // Audio
        m_audio = audio;

        // Scene
        CreateScene();

        // 最初のシーンを設定する
        SetFirstScene();
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns></returns>
    public bool Execute()
    {
        if (SceneManager.GetActiveScene().name != m_currentScene.Name)
           return false; 

        // 現在のシーンを実行する
        SceneID result = m_currentScene.Execute();

        // 戻り値で処理を分ける
        switch (result)
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
                // バトルシーンへ
                ChangeScene(m_battle,LoadSceneMode.Additive);
                break;
            case SceneID.SCENE_RESULT:
                // リザルトシーンへ
                ChangeScene(m_result);
                break;
            case SceneID.SCENE_CAPTURETUTORIAL:
                // キャプチャーチュートリアル
                ChangeScene(m_captureTutorial);
                break;
            case SceneID.SCENE_BATTLETUTORIAL:
                // バトルチュートリアル
                ChangeScene(m_battleTutorial);
                break;
            case SceneID.SCENE_ENDING:
                // エンディングシーン
                ChangeScene(m_ending);
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
        // 現在のシーンの終了処理をする
        m_currentScene.Dispose();
    }


    /// <summary>
    /// シーンを変更する
    /// </summary>
    /// <param name="nextScene">次のシーン名</param>
    /// <param name="mode">ロードシーンモード</param>
    public async void ChangeScene(IScene_Script nextScene,LoadSceneMode mode = LoadSceneMode.Single)
    {
        await TransitionManager_Script.OnTransitionFinishedAsync();

        // 終了処理をする
        m_currentScene.Dispose();
        // 次のシーンを設定する
        m_currentScene = nextScene;      
        // 遷移する
        //TransitionManager_Script.StartTransition(m_currentScene.Name, mode);

        // ActiveSceneを切り替える
        if(SceneManager.GetActiveScene().isLoaded)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_currentScene.Name));
        
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
        // ResultScene
        m_result = new SceneResult_Script();
        m_result.Name = "ResultScene";
        // TutorialScene
        m_captureTutorial = new SceneCaptureTutorial_Script();
        m_captureTutorial.Name = "TutorialCaptureScene";
        m_battleTutorial = new SceneBattleTutorial_Script();
        m_battleTutorial.Name = "TutorialBattleScene";
        // EndingScene
        m_ending = new SceneEnding_Script();
        m_ending.Name = "EndingScene";
    }

    private void SetFirstScene()
    {
        if (SceneManager.GetActiveScene().name == m_title.Name)
            m_currentScene = m_title;
        else if (SceneManager.GetActiveScene().name == m_revised.Name)
            m_currentScene = m_revised;
        else if (SceneManager.GetActiveScene().name == m_battle.Name)
            m_currentScene = m_battle;
        else if (SceneManager.GetActiveScene().name == m_result.Name)
            m_currentScene = m_result;
        else if (SceneManager.GetActiveScene().name == m_captureTutorial.Name)
            m_currentScene = m_captureTutorial;
        else if (SceneManager.GetActiveScene().name == m_battleTutorial.Name)
            m_currentScene = m_battleTutorial;
        else if (SceneManager.GetActiveScene().name == m_ending.Name)
            m_currentScene = m_ending;

        // 初期化する
        m_currentScene.Initialize(this);
    }
}
