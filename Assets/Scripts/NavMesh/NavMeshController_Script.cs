/*----------------------------------------------------------*/
//  file:      NavMeshController_Scripts.cs		                |
//				 											                    |
//  brief:    NavMeshを制御するスクリプト		                | 
//															                    |
//  date:	2019.11.27									            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;


// NavMeshControllerクラスの定義
[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshController_Script : MonoBehaviour
{
    // 巡回する座標
    [SerializeField]
    private Transform[] m_targets = null;
    // 巡回する座標スクリプト
    [SerializeField]
    private SetPatrolPosition_Script m_patrolPos = null;

    // 追跡用
    [SerializeField]
    private ChaseCharacter_Script m_chaseCharater;

    // エージェント
    private NavMeshAgent m_navAgent = null;

    // オーディオマネージャー
    AudioManager_Script m_audio;

    private int m_targetIndex = 0;
    // 目的地に到着したかどうか
    private bool m_isArrived = false;
    // 経過時間
    [SerializeField]
    private float m_elapsedTime;
    // アニメーターコンポーネント
    private Animator m_animator;
    // 現在の状態遷移
    [SerializeField]
    private WorldCreatureState_Script m_currentState;
    // Idle状態
    private WorldCreatureIdle_Script m_idleState;
    // Walk状態
    private WorldCreatureWalk_Script m_walkState;
    // Chase状態
    private WorldCreatureChace_Script m_chaseState;

    // ChaseTargetの座標
    private Transform m_chaseTargetPosition;


    /// <summary>
    /// ターゲット座標のプロパティ
    /// </summary>
    public Transform ChaseTargetPosition
    {
        set { m_chaseTargetPosition = value; }
        get { return m_chaseTargetPosition; }
    }


    /// <summary>
    /// アニメーターのプロパティ
    /// </summary>
    public Animator Animator => m_animator;


    /// <summary>
    /// NavMeshAgentのプロパティ
    /// </summary>
    public NavMeshAgent Agent => m_navAgent;

    /// <summary>
    /// Chase
    /// </summary>
    public ChaseCharacter_Script ChaseCharacter => m_chaseCharater;

    /// <summary>
    /// 経過時間のプロパティ
    /// </summary>
    public float ElapsedTime
    {
        get { return m_elapsedTime; }
        set { m_elapsedTime = value; }
    }


    /// <summary>
    /// 経過時間の初期化
    /// </summary>
    public void ResetElapsedTime()
    {
        m_elapsedTime = 0f;
    }


    /// <summary>
    /// 到着フラグのプロパティ
    /// </summary>
    public bool IsArrived
    {
        get { return m_isArrived; }
        set { m_isArrived = value; }
    }


    /// <summary>
    /// 状態プロパティ
    /// </summary>
    public WorldCreatureState_Script Idle => m_idleState;
    public WorldCreatureState_Script Walk => m_walkState;
    public WorldCreatureChace_Script Chase => m_chaseState;

    /// <summary>
    /// ターゲット座標のプロパティ
    /// </summary>
    private Vector3 CurrentTargetPosition
    {
        get
        {
            if(m_targets == null || m_targets.Length <= m_targetIndex)
            {
                return Vector3.zero;
            }
            return m_targets[m_targetIndex].position;
        }
    }


    private void Awake()
    {
        // コンポーネントを取得する
        m_navAgent = GetComponent<NavMeshAgent>();
        m_patrolPos = GetComponent<SetPatrolPosition_Script>();
        m_animator = GetComponent<Animator>();
        m_chaseCharater = GetComponentInChildren<ChaseCharacter_Script>();
        // オーディオマネージャーを取得する
        m_audio = AudioManager_Script.Get;

        // 初期化
        ResetElapsedTime();

        // 状態遷移を生成する
        CreateState();
        // 初期状態をWalkにする
        ChangeState(Walk);
    }

    // Start is called before the first frame update
    void Start()
    {
        // ターゲットを設定する
        m_targets = m_patrolPos.GetPatrolPosition();
      
        // 座標をランダムにする
        m_targets = ShufflePosition(m_targets);

        // 目的座標を設定する
        m_navAgent.destination = CurrentTargetPosition;

        // 更新処理　Destroyされるときに通知を切る
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                StateID result = m_currentState.Execute();

                switch (result)
                {
                    case StateID.CONTINUE:
                        break;
                    case StateID.STATE_WALK:
                        ChangeState(Walk);
                        break;
                    case StateID.STATE_IDLE:
                        ChangeState(Idle);
                        break;
                    case StateID.STATE_CHASE:
                        ChangeState(Chase);
                        break;
                }
            }).AddTo(gameObject);
    }


    /// <summary>
    /// 次の座標を設定する
    /// </summary>
    public void NextPosition()
    {
        m_targetIndex = (m_targetIndex + 1) % m_targets.Length;
        
        m_navAgent.destination = CurrentTargetPosition;
    }


    /// <summary>
    /// 座標をランダムにする
    /// </summary>
    /// <param name="targets">ランダムにする座標</param>
    /// <returns>ランダムにした座標</returns>
    private Transform[] ShufflePosition(Transform[] targets)
    {
        // ローカル変数に入れる
        Transform[] mainArray = targets;
        // シャッフル
        Transform[] tempArray = mainArray;
        mainArray = tempArray.OrderBy(i => Guid.NewGuid()).ToArray();

        return mainArray;
    }


    /// <summary>
    /// 状態遷移を変更する
    /// </summary>
    /// <param name="nextState">次の状態遷移</param>
    public void ChangeState(WorldCreatureState_Script nextState)
    {
        // 次の状態遷移を設定する
        m_currentState = nextState;
        // 初期化する
        m_currentState.Initialize(this);
    }


    /// <summary>
    /// オブジェクトが破棄されたら実行する
    /// </summary>
    private void OnDestory()
    {
        // 終了処理
        m_idleState.Dispose();
        m_walkState.Dispose();
        m_chaseState.Dispose();
    }


    /// <summary>
    /// 状態遷移を生成する
    /// </summary>
    private void CreateState()
    {
        // Idle状態を生成する
        m_idleState = new WorldCreatureIdle_Script();
        // Walk状態を生成する
        m_walkState = new WorldCreatureWalk_Script();
        // Chase状態を生成する
        m_chaseState = new WorldCreatureChace_Script();
    }
}
