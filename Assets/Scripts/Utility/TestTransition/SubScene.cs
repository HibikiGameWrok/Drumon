using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class SubScene : MonoBehaviour
{
    [SerializeField]
    private string m_nextScene = null;
    [SerializeField]
    private double m_timeSpan = 3;

    // Start is called before the first frame update
    private async void Start()
    {
        // シーン遷移が終わるまで待つ場合はOnTransitionFinishedAsyncをawaitする
        await TransitionManager_Script.OnTransitionFinishedAsync();

        Observable.Timer(TimeSpan.FromSeconds(m_timeSpan))
            .Subscribe(_ =>
            {
                TransitionManager_Script.StartTransition(m_nextScene);
            }).AddTo(this);
    }
}
