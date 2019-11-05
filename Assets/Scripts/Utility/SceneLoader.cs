/*----------------------------------------------------------*/
//  file:      SceneLoader_Scripts.cs				                |
//				 											                    |
//  brief:    ゲーム側から呼び出すためのstaticクラス         | 
//				 											                    |
//															                    |
//  date:	2019.11.5										            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using UniRx;
using UnityEngine;


public static class SceneLoader
{
    public static SceneDataPack PreviousSceneData;

    private static TransitionManager m_transitionManager;

    private static TransitionManager TransitionManager
    {
        get
        {
            if (m_transitionManager != null)
                return m_transitionManager;

            Initialize();
            return m_transitionManager;
        }
    }


    /// <summary>
    /// トランジションマネージャーが存在しない場合に初期化する
    /// </summary>
    public static void Initialize()
    {
        if(TransitionManager.Get == null)
        {
            var resource = Resources.Load("");
            Object.Instantiate(resource);
        }
        m_transitionManager = TransitionManager.Get;
    }


    /// <summary>
    /// シーン遷移のトランジションが完了したことを通知する
    /// </summary>
    public static System.IObservable<Unit> OnTransitionFinished
    {
        get { return TransitionManager.OnTransitionAnimationFinished; }
    }


    /// <summary>
    /// シーンのロードがすべて完了したことを通知する
    /// </summary>
    public static System.IObservable<Unit> OnScenesLoaded
    {
        get { return TransitionManager.OnSceneLoaded.FirstOrDefault(); }
    }


    /// <summary>
    /// トランジションアニメーションを終了させてゲームシーンを変更する
    /// </summary>
    public static void Open()
    {
        TransitionManager.Open();
    }


    /// <summary>
    /// シーン遷移処理中か
    /// </summary>
    public static bool IsTransitionRunning
    {
        get { return TransitionManager.IsRunning; }
    }


    /// <summary>
    /// シーン遷移を行う
    /// </summary>
    /// <param name="scene">次のシーン</param>
    /// <param name="data">次のシーンへ引き継ぐデータ</param>
    /// <param name="additiveLoadScenes">追加でロードするシーン</param>
    /// <param name="autoMove">トランジションアニメーションを自動で完了させるか</param>
    public static void LoadScene(GameScenes scene, SceneDataPack data = null, GameScenes[] additiveLoadScenes = null, bool autoMove = true)
    {
        if(data == null)
        {
            data = new DefalutSceneDataPack(TransitionManager.CurrentGameScene, additiveLoadScenes);
        }
        TransitionManager.StartTranscation(scene, data, additiveLoadScenes, autoMove);
    }
}