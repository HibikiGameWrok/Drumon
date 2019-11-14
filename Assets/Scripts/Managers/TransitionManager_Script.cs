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
        return o.GetComponent<TransitionController_Script>();
    });

    private static TransitionController_Script Controller => controller.Value;

    /// <summary>
    /// 次のシーンへ遷移する
    /// </summary>
    /// <param name="nextSceneName"></param>
    public static void StartTransition(string nextSceneName)
    {
        Controller.TransitionStart(nextSceneName);
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
