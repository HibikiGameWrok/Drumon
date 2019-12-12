using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Script
{
    // 現在の処理
    private IScene_Script m_currentScene;

    private IScene_Script m_title;
    private IScene_Script m_resived;

    private AudioManager_Script m_audio;

    public AudioManager_Script Audio => m_audio;


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(AudioManager_Script audio)
    {
        // Audio
        m_audio = audio;

        // Scene
        m_title = new SceneTitle_Script();
        m_title.Name = "TitleScene";
        m_resived = new SceneRevised_Script();
        m_resived.Name = "Revised";

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
        bool result = m_currentScene.Execute();

        if(m_currentScene == m_title)
        {
            if(result == true)
            {

            }
            else
            {
                ChangeScene(m_resived);
            }
        }

        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public void Dispose()
    {

    }

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
}
