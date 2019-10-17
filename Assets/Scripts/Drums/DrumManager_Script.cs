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
public class DrumManager_Script : MonoBehaviour 
{
    // 列挙
    public enum DRUM_TYPE
    {
        ATTACK,
        HEAL,
        SELECT,
        CATCH,

        TOTAL
    }

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

    /// <summary>
    /// Awake関数
    /// </summary>
    void Awake()
    {
        // 攻撃用のドラムを生成する
        m_attackDrum = GetComponentInChildren<AttackDrum_Script>();
        // 初期化する
        m_attackDrum.Initialize(this);
        // 回復用のドラムを生成する
        m_healDrum = GameObject.FindGameObjectWithTag("HealDrum").GetComponent<HealDrum_Script>();
        // 初期化する
        m_healDrum.Initialize(this);
        // 選択用のドラムを生成する
        //  m_selectDrum = GetComponentInChildren<SelectDrum_Script>();

        // 現在のドラムを攻撃用のドラムにする
        m_currentDrum = m_healDrum;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_currentDrum != null)
        {
            // 現在のドラムの処理を実行する
            bool result = m_currentDrum.Execute();

            // 攻撃用のドラムの処理
            if(m_currentDrum == m_attackDrum)
            {
                if(result == true)
                {
                    // 継続する
                }
                else
                {
         
                }
            }
            else if(m_currentDrum == m_healDrum)
            {
                if (result == true)
                {
                    // 継続する
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
