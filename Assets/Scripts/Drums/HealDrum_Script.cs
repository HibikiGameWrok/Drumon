/*----------------------------------------------------------*/
//  file:      HealDrum_Scripts.cs						            |
//				 											                    |
//  brief:    回復用のドラムクラスのスクリプト		            |
//              Heal Drum class  				                        |
//															                    |
//  date:	2019.10.9										            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 回復用のドラムクラスの定義
public class HealDrum_Script : Drum_Script
{

    // メンバ変数

    // 左スティック
    StickLeft_Script m_leftStick;
    // 右スティック
    StickRight_Script m_rightStick;
    // HPUI
    private HealProsperityUI_Script m_healProsperityUI;

    /// <summary>
    /// デフォルト関数
    /// </summary>
    void Start()
    {
        // 何もしない
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize(DrumManager_Script manager)
    {
        // 親オブジェクトを入れる
        m_manager = manager;

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();
        m_healProsperityUI = FindObjectOfType<HealProsperityUI_Script>();
    }


    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=ドラムを変更する</returns>
    public override bool Execute()
    {
        // アクティブでないなら
        if(isActive == false)
        {
            // 変更する
            return false;
        }

        // 継続する
        return true;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    public override void Dispose()
    {        
        // 非アクティブにする
        isActive = false;
    }


    /// <summary>
    /// アクティブフラグのプロパティ
    /// </summary>
    public override bool isActive
    {
        // 取得する
        get { return m_isActive; }
        // 設定する
        set { m_isActive = value; }
    }

    /// <summary>
    /// 当たり判定の検出をする
    /// </summary>
    /// <param name="col">衝突した相手</param>
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Stick")
        {
            if (isActive == false)
            {
                // 回復用のドラムに変更する
                m_manager.ChangeDrum(/*m_manager.HealDrum*/GetComponent<HealDrum_Script>());
            }

            // アクティブにする
            isActive = true;
        }
    }


    /// <summary>
    /// 当たり判定から外れた時
    /// </summary>
    /// <param name="col">衝突した相手</param>
    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }

    // 回復処理
    public void Heal()
    {
        // 回復ドラムが叩かれたら
        if (m_leftStick.HealHitFlag == true || m_rightStick.HealHitFlag == true)
        {
            // HPを回復
            m_healProsperityUI.NowPoint = m_healProsperityUI.NowPoint + 1.0f;

            m_leftStick.HealHitFlag = false;
            m_rightStick.HealHitFlag = false;

            Debug.Log("回復");
        }
    }
}
