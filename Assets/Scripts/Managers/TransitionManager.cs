using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TransitionManager
{
    /// <summary>
    /// TransitionContorllerが存在しないなら生成する
    /// </summary>
    private static Lazy<TransitionController> controller = new Lazy<TransitionController>(() =>
    {
        var r = Resources.Load("TransitionController");
        var o = UnityEngine.Object.Instantiate(r) as GameObject;
        UnityEngine.Object.DontDestroyOnLoad(o);
        return o.GetComponent<TransitionController>();
    });

    private static TransitionController Controller => controller.Value;

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
