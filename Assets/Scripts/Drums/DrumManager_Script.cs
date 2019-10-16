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
    enum DRUM_TYPE : int
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
    private AttackDrum_Script m_attackDrum;
    // 回復用のドラム
    [SerializeField]
    private HealDrum_Script m_healDrum;
    // メニューセレクト用のドラム
    [SerializeField]
    private SelectDrum_Script m_selectDrum;
    // キャプチャ用のドラム
    //
    //
    [SerializeField]
    private MeshRenderer m_meshRenderer;
    [SerializeField]
    private Material[] m_material;

    // 現在のドラム
    [SerializeField]
    private Drum_Script m_currentDrum;

    /// <summary>
    /// Awake関数
    /// </summary>
    void Awake()
    {
        // 攻撃用のドラムを生成する
        m_attackDrum = new AttackDrum_Script();
        // 回復用のドラムを生成する
        m_healDrum = new HealDrum_Script();

        m_meshRenderer = GetComponentInChildren<MeshRenderer>();

        // 現在のドラムを攻撃用のドラムにする
        m_currentDrum = m_attackDrum;
        // 初期化する
        m_currentDrum.Initialize(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        // test
        m_material = new Material[(int)DRUM_TYPE.TOTAL];
        m_material[(int)DRUM_TYPE.ATTACK] = Resources.Load("Materials/M_Attack_Drum", typeof(Material)) as Material;
        m_material[(int)DRUM_TYPE.HEAL] = Resources.Load<Material>("Materials/M_Heal_Drum");
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
                    // ドラムを変更する
                    ChangeDrum(m_healDrum);
                    // 分かりやすいようにマテリアルを変更する
                    m_meshRenderer.material = m_material[0];
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
                    // ドラムを変更する
                    ChangeDrum(m_attackDrum);
                    // 分かりやすいようにマテリアルを変更する
                    m_meshRenderer.material = m_material[1];
                }
            }
        }

#if UNITY_EDITOR
        Debug.Log("CurrentDrum is " + m_currentDrum);
#endif
    }


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
    public Drum_Script GetAttackDrum
    {
        get
        {
            return m_attackDrum;
        }
    }


    /// <summary>
    /// 回復用のドラムを取得する
    /// </summary>
    public Drum_Script GetHealDrum
    {
        get
        {
            return m_healDrum;
        }
    }

}
