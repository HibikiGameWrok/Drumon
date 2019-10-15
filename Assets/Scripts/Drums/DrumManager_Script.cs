/*----------------------------------------------------------*/
//  file:      DrumManger_Script.cs		                        |
//				 															    |
//  brief:    ドラムマネージャーのスクリプト				    |
//              Drums 	Manager Class		                        |
//																				|
//  date:	2019.10.11											    |
//																				|
//  author: Renya Fukuyama										|
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ドラムマネージャーの定義
public class DrumManager_Script : MonoBehaviour 
{
    // メンバ変数 

    // 攻撃用のドラム 
    [SerializeField]
    private AttackDrum_Script m_attackDrum;
    // 回復用のドラム
    [SerializeField]
    private HeelDrum_Script m_heelDrum;
    // メニューセレクト用のドラム
    [SerializeField]
    private SelectDrum_Script m_selectDrum;
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
        m_attackDrum = gameObject.GetComponentInChildren<AttackDrum_Script>();
        m_heelDrum = gameObject.GetComponentInChildren<HeelDrum_Script>();

        m_currentDrum = m_attackDrum;
    }


    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        // テスト
        if(Input.GetKeyDown(KeyCode.A))
        {
            ChangeDrum(m_attackDrum);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            ChangeDrum(m_heelDrum);
        }

        Debug.Log(m_currentDrum);
    }



    /// <summary>
    /// ドラムを変更する処理
    /// </summary>
    /// <param name="nextDrum">次に変えるドラム</param>
    /// <returns>true=成功 false=失敗</returns>
    public bool ChangeDrum(Drum_Script nextDrum)
    {
        if(m_currentDrum)
        {
            // 現在のドラムの終了処理


            // 次のドラムに変更する
            m_currentDrum = nextDrum;

            // 次のドラムを初期化する
            

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

}
