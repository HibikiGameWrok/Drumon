using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class SampleMain : MonoBehaviour
{
    public GameObject[] gameObjectsTohidden;

    [SerializeField]
    private string m_nextSceneName;
    [SerializeField]
    private UnityEngine.UI.Button m_button;

    // Start is called before the first frame update
    private async void Start()
    {
        // シーン遷移が終わるまで待つ場合はOnTransitionFinishedAsyncをawaitする
        await TransitionManager_Script.OnTransitionFinishedAsync();

        m_button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TransitionManager_Script.StartTransition(
                    m_nextSceneName,
                    LoadSceneMode.Additive
                    );
            });
    }
}
