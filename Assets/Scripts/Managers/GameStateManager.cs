/*----------------------------------------------------------*/
//  file:      GameStateManager_Script.cs                      |
//				 											                    |
//  brief:    ゲームの状態の管理スクリプト				        |
//                                      				                        |
//															                    |
//  date:	2019.11.12									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

// ゲームステートマネージャークラスの定義
public class GameStateManager : MonoBehaviour
{
    // 定数
    private static readonly string BATTLE_SCENE = "SampleBattle";
    private static readonly string TITLE_SCENE = "SampleTitle";

    [SerializeField]
    private GameDirector m_director;
    // ゲームステート
    private ReactiveProperty<GameState> m_gameState = new ReactiveProperty<GameState>(GameState.Initialize);
    // 現在のゲームステート
    public IReadOnlyReactiveProperty<GameState> CurrentState => m_gameState;


    // Start is called before the first frame update
    void Start()
    {
        // ゲームが開始したら探索パート
        //m_director.ReadyTimer
        //    .FirstOrDefault(x => x == 0)
        //    .Subscribe(_ => m_gameState.SetValueAndForceNotify(GameState.Explore));

        // プレイヤーがドラモンに接触したらバトルパート
        m_director.ReadyTimer
       .FirstOrDefault(x => x == 0)
       .Subscribe(_ =>
       {
           m_gameState.SetValueAndForceNotify(GameState.Battle);
           MoveToBattleScene();
        });

        // バトルパートが終了したら探索パートに戻る

        // 目的を達成したらリザルト

        // リザルト終了でタイトル画面へ


        InitializeAsync().Forget();
    }

    private async UniTaskVoid  InitializeAsync()
    {
        m_gameState.SetValueAndForceNotify(GameState.Initialize);

        // 画面が開くまで待つ
        await TransitionManager_Script.OnTransitionFinishedAsync();
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));

        m_gameState.SetValueAndForceNotify(GameState.Ready);

        // カウントダウン開始
        m_director.CountDownStart();
    }

    public void MoveToTitleScene()
    {
        TransitionManager_Script.StartTransition(TITLE_SCENE, LoadSceneMode.Single);
    }

    public void MoveToBattleScene()
    {
        TransitionManager_Script.StartTransition(BATTLE_SCENE, LoadSceneMode.Additive);
    }

    public void MoveToMainScene()
    {
        TransitionManager_Script.StartTransition("SampleMain", LoadSceneMode.Single);
    }
}
