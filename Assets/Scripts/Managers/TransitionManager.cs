/*----------------------------------------------------------*/
//  file:      TransitionManager_Scripts.cs				        |
//				 											                    |
//  brief:    シーン遷移管理をするクラス		                    | 
//              Scene Tranition Class                                 |
//															                    |
//  date:	2019.11.1										            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// シーン遷移を管理する
public class TransitionManager : SingletonBase_Script<TransitionManager>
{
    
    // 蓋絵のイメージ
    private RawImage m_image;
    // シーン遷移処理を実行中か
    private bool m_isRunning = false;
    // トランジションのアニメーションの終了通知
    private Subject<Unit> m_onTransactionFinishedInternal = new Subject<Unit>();
    // トランジションが終了し、シーンが介したことを通知する
    private Subject<Unit> m_onTransitionAnimationFinishedSubject = new Subject<Unit>();

    private Subject<Unit> m_onAllSceneLoaded = new Subject<Unit>();
    // トランジションアニメーションを終了させてよいか
    private ReactiveProperty<bool> m_canEndTransition = new ReactiveProperty<bool>(false);

    private GameScenes m_currentGameScenes;


    public bool IsRunning
    {
        get { return m_isRunning; }
    }


    /// <summary>
    /// 現在のシーン情報
    /// </summary>
    public GameScenes CurrentGameScene
    {
        get { return m_currentGameScenes; }
    }


    /// <summary>
    /// 全シーンのロードが完了したことを通知する
    /// </summary>
    public IObservable<Unit> OnSceneLoaded
    {
        get { return m_onAllSceneLoaded; }
    }


    /// <summary>
    /// トランジションが終了し、シーンが開始したことを通知する
    /// (OnCompletedも発行する)
    /// </summary>
    public IObservable<Unit> OnTransitionAnimationFinished
    {
        get
        {
            if(m_isRunning)
            {
                return m_onTransitionAnimationFinishedSubject.FirstOrDefault();
            }
            else
            {
                // シーン遷移を実行していないなら即値を返す
                return Observable.Return(Unit.Default);
            }
        }
    }


    public void Open()
    {
        m_canEndTransition.Value = true;
    }


    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);

        try
        {
            m_currentGameScenes = (GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
        }
        catch
        {
            Debug.Log("シーンの取得に失敗した");
            // 適当なシーンで埋めておく
            m_currentGameScenes = GameScenes.Title;
        }
    }


    private void Initialize()
    {
        //if(m_transitionComponent == null)
        //{
        //    m_transitionComponent = GetComponent<>();

        m_image = GetComponent<RawImage>();
        m_image.raycastTarget = false;

        //    // m_transitionComponent.filpAfterAnimation = true;
        //    m_transitionComponent.onTransitionComplete.AddListerner(
        //        () => m_onTransactionFinishedInternal.OnNext(Unit.Default));
        //}
    }


    /// <summary>
    /// シーン遷移を実行する
    /// </summary>
    /// <param name="nextScene">次のシーン</param>
    /// <param name="data">次のシーンへ引き継ぐデータ</param>
    /// <param name="additiveLoadScenes">追加ロードするシーン</param>
    /// <param name="autoMove">トランジションの自動遷移を行うか</param>
    public void StartTranscation(GameScenes nextScene, SceneDataPack data, GameScenes[] additiveLoadScenes, bool autoMove)
    {
        if (m_isRunning)
            return;

        StartCoroutine(TransitionCoroutine(nextScene,data,additiveLoadScenes,autoMove));
    }


    /// <summary>
    /// シーン遷移処理の本体
    /// </summary>
    /// <param name="nextScene">次のシーン</param>
    /// <param name="data">次のシーンへ引き継ぐデータ</param>
    /// <param name="additiveLoadScenes">追加ロードするシーン</param>
    /// <param name="autoMove">トランジションの自動遷移を行うか</param>
    private IEnumerator TransitionCoroutine(GameScenes nextScene, SceneDataPack data, GameScenes[] additiveLoadScenes, bool autoMove)
    {
        // 処理開始フラグを設定する
        m_isRunning = true;

        // トランジションの自動遷移を設定する
        m_canEndTransition.Value = autoMove;

        //if(m_transitionComponent == null)
        {
            // 初期化が出来なかったらここでする
            Initialize();
            yield return null;
        }

        // 蓋絵でuGUIのタッチイベントをブロックする
        m_image.raycastTarget = true;

        // トランジション開始
        // m_transitionComponent.flip = false;
        // m_transitionComponent.ignoreTimeScale = true;
        // m_transitionComponent.Play();

        // トランジションアニメーションが終了するのを待つ
        yield return m_onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();

        // 前のシーンから受け取った情報を登録
        SceneLoader.PreviousSceneData = data;

        // メインとなるシーンをSingleで読み込む
        yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

        // 追加シーンがある場合は一緒に読み込む
        if(additiveLoadScenes != null)
        {
            yield return additiveLoadScenes.Select(scene =>
            SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive).AsObservable()
            ).WhenAll().ToYieldInstruction();
        }
        yield return null;

        // 使っていないリソースの解放とGCを実行する
        Resources.UnloadUnusedAssets();
        GC.Collect();

        yield return null;

        // 現在シーンを設定する
        m_currentGameScenes = nextScene;

        // シーンロードの完了通知を発行する
        m_onAllSceneLoaded.OnNext(Unit.Default);

        if(!autoMove)
        {
            // 自動遷移しない設定の場合はフラグがtrueに変化するまで待つ
            yield return m_canEndTransition.FirstOrDefault(x => x).ToYieldInstruction();
        }
        m_canEndTransition.Value = false;

        // 蓋絵を開く方のアニメーション開始
        //m_transitionComponent

        // 蓋絵が開ききるのを待つ
        yield return m_onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();

        // 蓋絵のイベントブロックを解除する
        m_image.raycastTarget = false;

        // トランジションがすべて完了したことを通知する
        m_onTransitionAnimationFinishedSubject.OnNext(Unit.Default);

        // 終了
        m_isRunning = false;
    }
}
