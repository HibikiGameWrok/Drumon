using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GotoButton : MonoBehaviour
{
    [SerializeField]
    private string m_nextScene = null;
    [SerializeField]
    private UnityEngine.UI.Button m_button = null;

    private async void Start()
    {
        // シーン遷移が終わるまで待つ場合はOnTransitionFinishedAsyncをawaitする
        await TransitionManager_Script.OnTransitionFinishedAsync();
        
        m_button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TransitionManager_Script.StartTransition(m_nextScene);
            });
    }
}
