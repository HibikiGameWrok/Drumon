/*----------------------------------------------------------*/
//  file:      TransitionManager_Scripts.cs                      |
//				 											                    |
//  brief:    トランジションマネージャーのスクリプト        | 
//              Attack Drum class  				                    |
//															                    |
//  date:	2019.11.11									            |
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


// トランジションマネージャークラスの定義
public class TransitionManager_Script
{
    /// <summary>
    /// TransitionContorllerが存在しないなら生成する
    /// </summary>
    private static Lazy<TransitionController_Script> controller = new Lazy<TransitionController_Script>(() =>
    {
        var r = Resources.Load("TransitionController");
        var o = UnityEngine.Object.Instantiate(r) as GameObject;
        UnityEngine.Object.DontDestroyOnLoad(o);

        var canvas = o.GetComponent<Canvas>();
        if(GameObject.Find("CenterEyeAnchor"))
            canvas.worldCamera = GameObject.Find("CenterEyeAnchor")
                                                            .GetComponent<Camera>();
        
        return o.GetComponent<TransitionController_Script>();
    });

    private static TransitionController_Script Controller => controller.Value;

    /// <summary>
    /// 次のシーンへ遷移する
    /// </summary>
    /// <param name="nextSceneName">次のシーンの名前</param>
    public static void StartTransition(string nextSceneName)
    {
        Controller.TransitionStart(nextSceneName);
    }


    /// <summary>
    /// 次のシーンへ遷移する
    /// </summary>
    /// <param name="nextSceneName">次のシーンの名前</param>
    /// <param name="mode">遷移前のシーンを残すか</param>
    public static void StartTransition(string nextSceneName, LoadSceneMode mode)
    {
        Controller.TransitionStart(nextSceneName, mode);
    }


    public static void StartTransition_UnloadScene(string unloadSceneName)
    {
        Controller.UnloadStart(unloadSceneName);
    }

    /// <summary>
    /// シーン遷移完了を通知する
    /// </summary>
    /// <returns></returns>
    public static IObservable<Unit> OnTransitionFinishedAsync()
    {
        return Controller.OnTransitionFinished;
    }
}
