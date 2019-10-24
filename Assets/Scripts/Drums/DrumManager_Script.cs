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


// ドラムマネージャーの定義
public class DrumManager_Script : SingletonBase_Script<DrumManager_Script> 
{
    // メンバ変数 
    // 攻撃用のドラム 
    [SerializeField]
    private Drum_Script m_attackDrum;
    // 回復用のドラム
    [SerializeField]
    private Drum_Script m_healDrum;
    // メニューセレクト用のドラム
    [SerializeField]
    private Drum_Script m_selectDrum;
    // キャプチャ用のドラム
    //
    //

    // 現在のドラム
    [SerializeField]
    private Drum_Script m_currentDrum;

    // HPUI
    private GameObject m_healProsperityUI;
    private HealProsperityUI_Script m_healProsperityUIScript;

    private PlayerCreature_Script m_playerCreature;

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
        // 回復用のドラムを生成する
        m_healDrum = GameObject.FindGameObjectWithTag("HealDrum").GetComponent<HealDrum_Script>();
        // 初期化する
        m_healDrum.Initialize(this);

        // 選択用のドラムを生成する
        // m_selectDrum = GameObject.FindGameObjectWithTag("SelectDrum").GetComponent<SelectDrum_Script>();
        // m_selectDrum.Initialize(this);

        // 現在のドラムを攻撃用のドラムにする
        m_currentDrum = m_attackDrum;
        // 現在のドラムをアクティブにする
        m_currentDrum.isActive = true;

        m_healProsperityUI = GameObject.Find("PSlider");
        m_healProsperityUIScript = m_healProsperityUI.GetComponent<HealProsperityUI_Script>();

        m_playerCreature = BattleManager_Script.Get.PlayerCreature;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        // 現状は何もしない
    }

    // Update is called once per frame
    void Update()
    {
        m_healProsperityUIScript.NowPoint = m_playerCreature.HP;
        if (m_currentDrum != null)
        {
            Debug.Log(m_currentDrum);
            // 現在のドラムの処理を実行する
            bool result = m_currentDrum.Execute();

            // 攻撃用のドラムの処理
            if(m_currentDrum == m_attackDrum)
            {
                if(result == true)
                {
                    // 継続する

                    // ノーツの生成処理
                    m_attackDrum.GetComponent<AttackDrum_Script>().GenerateNotes();
                }
                else
                {
                    
                }
            }
            // 回復用のドラムの処理
            else if(m_currentDrum == m_healDrum)
            {
                if (result == true)
                {
                    // 継続する

                    // 回復処理
                    m_healDrum.GetComponent<HealDrum_Script>().Heal();
                    // HPを回復
                    m_playerCreature.Heal();
                }
                else
                {

                }
            }
        }

#if UNITY_EDITOR
        Debug.Log("CurrentDrum is " + m_currentDrum);
#endif
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
        if (m_healDrum != null)
        {
            // 回復用のドラムを解放する
            m_healDrum.Dispose();
            m_healDrum = null;
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
    /// 回復用のドラムを取得する
    /// </summary>
    public Drum_Script HealDrum
    {
        get
        {
            return m_healDrum;
        }
    }
}
