/*----------------------------------------------------------*/
//  file:      DrumManger_Script.cs		                    |
//				 											|
//  brief:    ドラムマネージャーのスクリプト				|
//              Drums 	Manager Class		                |
//															|
//  date:	2019.10.11										|
//															|
//  author: Renya Fukuyama									|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

// ドラムマネージャーの定義
public class DrumManager_Script : SingletonBase_Script<DrumManager_Script> 
{
    // メンバ変数 
    // 攻撃用のドラム 
    [SerializeField]
    private Drum_Script m_attackDrum;
    // 選択用のドラム
    [SerializeField]
    private Drum_Script m_switchDrum;
    // 捕獲用のドラム
    [SerializeField]
    private Drum_Script m_captureDrum;

    // 現在のドラム
    [SerializeField]
    private Drum_Script m_currentDrum;

    // HPUI
    private GameObject m_healProsperityUI;
    private HealProsperityUI_Script m_healProsperityUIScript;
    // プレイヤーモンスター
    private PlayerCreature_Script m_playerCreature;
    private EnemyCreature_Script m_enemyCreature;

    // タイマーオブジェクト
    private GameObject m_timerObject;

    // チュートリアルのモンスターを捕獲したフラグ
    //private bool m_tutorialGetFlag = false;
    //// チュートリアルのモンスターを捕獲したフラグのプロパティ
    //public bool TutorialGetFlag
    //{
    //    get { return m_tutorialGetFlag; }
    //    set { m_tutorialGetFlag = value; }
    //}

    /// <summary>
    /// Awake関数
    /// </summary>
    protected override void Awake() 
    {
        // 既にこのオブジェクトがあるか確認する
        CheckInstance();

        // 攻撃用のドラムを生成する
        m_attackDrum = GameObject.FindGameObjectWithTag("AttackDrum").GetComponent<AttackDrum_Script>();
        // 初期化する
        m_attackDrum.Initialize(this);
        // 選択用のドラムを生成する
        m_switchDrum = GameObject.FindGameObjectWithTag("SwitchDrum").GetComponent<SwitchDrum_Script>();
        // 初期化する
        m_switchDrum.Initialize(this);
        // 捕獲用のドラムを生成する
        m_captureDrum = GameObject.FindGameObjectWithTag("CaptureDrum").GetComponent<CaptureDrum_Script>();
        // 初期化する
        m_captureDrum.Initialize(this);

        // 現在のドラムを攻撃用のドラムにする
        m_currentDrum = m_attackDrum;
        // 現在のドラムをアクティブにする
        m_currentDrum.isActive = true;

        if (SceneManager.GetActiveScene().name != "TutorialCaptureScene")
        {
            m_healProsperityUI = GameObject.Find("PSlider");
            m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();
        }

        m_playerCreature = BattleManager_Script.Get.PlayerCreature;
        m_enemyCreature = BattleManager_Script.Get.EnemyCreature;

        m_timerObject = GameObject.Find("Timer");
    }

    
    // Start is called before the first frame update
    void Start()
    {
        // 現状は何もしない
    }


    // Update is called once per frame
    void Update()
    {
        // プレイヤーモンスターのHPをUIに適用
        //m_healProsperityUIScript.NowPoint = m_playerCreature.HP;

        if (m_currentDrum != null)
        {
            //Debug.Log(m_currentDrum);
            // 現在のドラムの処理を実行する
            bool result = m_currentDrum.Execute();

            // 攻撃用のドラムの処理
            if(m_currentDrum == m_attackDrum)
            {
                if(result == true)
                {
                    if (XRDevice.isPresent)
                    {
                        // 継続する

                        // ノーツの生成処理
                        m_attackDrum.GetComponent<AttackDrum_Script>().GenerateNotes();
                    }
                }
                else
                {
                    
                }
            }
            // 選択用のドラムの処理
            else if (m_currentDrum == m_switchDrum)
            {
                if (result == true)
                {
                    // 継続する

                    // UIの表示
                    m_switchDrum.GetComponent<SwitchDrum_Script>().OpenUI();
                    // UIの非表示
                    m_switchDrum.GetComponent<SwitchDrum_Script>().CloseUI();
                    // アイコンの変更
                    m_switchDrum.GetComponent<SwitchDrum_Script>().ChangeIcon();
                    // モンスターの変更
                    m_switchDrum.GetComponent<SwitchDrum_Script>().ChengeCreature();
                }
                else
                {

                }
            }
            // 捕獲用のドラムの処理
            else if (m_currentDrum == m_captureDrum)
            {
                if (result == true)
                {
                    // 継続する
                    m_captureDrum.GetComponent<CaptureDrum_Script>().Capture();
                }
            }
        }

        // キャプチャーの時にコストが0になったら
        if (m_captureDrum.GetComponent<CaptureDrum_Script>().CostZeroFlag == true)
        {
            if (m_captureDrum.GetComponent<CaptureDrum_Script>().CaptureCount != 0)
            {
                if (m_captureDrum.GetComponent<CaptureDrum_Script>().TutorialCaptureFlag == true)
                {
                    m_enemyCreature.Capture(CaptureDrum_Script.CAPTURE_CONFIRM);
                    //m_tutorialGetFlag = true;
                }
                else if (m_captureDrum.GetComponent<CaptureDrum_Script>().TutorialBattleFlag == true)
                {
                    m_enemyCreature.Capture(0);
                }
                else
                {
                    m_enemyCreature.Capture(m_captureDrum.GetComponent<CaptureDrum_Script>().CaptureCount);
                }

                m_captureDrum.GetComponent<CaptureDrum_Script>().CaptureCount = 0;
            }
            m_captureDrum.GetComponent<CaptureDrum_Script>().CostZeroFlag = false;
        }
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    private void OnDestroy()
    {
        if (m_attackDrum != null)
        {
            // 攻撃用のドラムを解放する
            m_attackDrum.Dispose();
            m_attackDrum = null;
        }
        if (m_switchDrum != null)
        {
            // 選択用のドラムを解放する
            m_switchDrum.Dispose();
            m_switchDrum = null;
        }
        if (m_captureDrum != null)
        {
            // 捕獲用のドラムを解放する
            m_captureDrum.Dispose();
            m_captureDrum = null;
        }
    }


    /// <summary>
    /// ドラムを変更する処理
    /// </summary>
    /// <param name="nextDrum">次に変えるドラム</param>
    /// <returns>true=成功 false=失敗</returns>
    public bool ChangeDrum(Drum_Script nextDrum)
    {
        if(m_currentDrum != null)
        {
            // 現在のドラムの終了処理
            m_currentDrum.Dispose();

            // 次のドラムに変更する
            m_currentDrum = nextDrum;

            // 次のドラムを初期化する
            m_currentDrum.Initialize(this);

            // 成功
            return true;
        }

        // 失敗
        return false;
    }


    /// <summary>
    /// 攻撃用のドラムを取得する
    /// </summary>
    public Drum_Script AttackDrum
    {
        get
        {
            return m_attackDrum;
        }
    }

    /// <summary>
    /// 選択用のドラムを取得する
    /// </summary>
    public Drum_Script SwitchDrum
    {
        get
        {
            return m_switchDrum;
        }
    }

    /// <summary>
    /// 選択用のドラムを取得する
    /// </summary>
    public Drum_Script CaptureDrum
    {
        get
        {
            return m_captureDrum;
        }
    }
}
