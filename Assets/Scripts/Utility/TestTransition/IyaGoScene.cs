using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class IyaGoScene : MonoBehaviour
{
    [SerializeField]
    private BattleManager_Script m_battleManager = null;

    [SerializeField]
    private string m_nextScene;

    // Start is called before the first frame update
    private async void Start()
    {
        // バトルマネージャーを取得する
        m_battleManager = BattleManager_Script.Get;

        await TransitionManager_Script.OnTransitionFinishedAsync();

        // バトルが終了したらトランジション
        m_battleManager.IsFinish
            .Where(_ => m_battleManager.IsFinish.Value == true)
            .Subscribe(_ =>
            {
                TransitionManager_Script.StartTransition(m_nextScene, LoadSceneMode.Single);
            }).AddTo(gameObject);
    }
}
