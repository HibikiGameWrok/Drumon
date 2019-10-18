/*----------------------------------------------------------*/
//  file:      AttackDrum_Scripts.cs					            |
//				 											                    |
//  brief:    攻撃用のドラムクラスのスクリプト		            | 
//              Attack Drum class  				                    |
//															                    |
//  date:	2019.10.9										            |
//															                    |
//  author: Renya Fukuyama									    |
/*----------------------------------------------------------*/

// using
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 攻撃用のドラムクラスの定義
public class AttackDrum_Script : Drum_Script
{
    // メンバ変数
    GameObject m_musicalScore;
    NotesActionGauge_Script m_notesActionGauge;

    StickLeft_Script m_leftStick;
    StickRight_Script m_rightStick;

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
    /// <param name="manager">ドラムマネージャー</param>
    public override void Initialize(DrumManager_Script manager)
    {
        // 親オブジェクトを入れる
        m_manager = manager;

        m_musicalScore = GameObject.Find("MusicalScore");
        m_notesActionGauge = m_musicalScore.GetComponent<NotesActionGauge_Script>();

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();
    }

    /// <summary>
    /// 実行する
    /// </summary>
    /// <returns>true=継続する false=ドラムを変更する</returns>
    public override bool Execute()
    {
        // アクティブでないなら
        if (isActive == false)
        {
            // 変更する
            return false;
        }

        // 内側を同時に叩いていたら
        if (m_rightStick.DoubleInHitFlag == true)
        {
            // ノーツ生成
            m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.DOUBLE_IN_HIT);

            m_rightStick.DoubleInHitFlag = false;
        }
        // 外側を同時に叩いていたら
        else if (m_rightStick.DoubleOutHitFlag == true)
        {
            // ノーツ生成
            m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.DOUBLE_OUT_HIT);

            m_rightStick.DoubleOutHitFlag = false;
        }

        // 内側を同時に叩いていたら
        if (m_leftStick.DoubleInHitFlag == true)
        {
            // ノーツ生成
            m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.DOUBLE_IN_HIT);

            m_leftStick.DoubleInHitFlag = false;
        }
        // 外側を同時に叩いていたら
        else if (m_leftStick.DoubleOutHitFlag == true)
        {
            // ノーツ生成
            m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.DOUBLE_OUT_HIT);

            m_leftStick.DoubleOutHitFlag = false;
        }

        // 時間が0になったら
        if (m_leftStick.DoubleHitTime < 0)
        {
            if (m_leftStick.InHitNotesFlag == true)
            {
                // ノーツ生成
                m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.ONE_IN_HIT);
            }
            else if (m_leftStick.OutHitNotesFlag == true)
            {
                // ノーツ生成
                m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.ONE_OUT_HIT);
            }

            if (m_rightStick.InHitNotesFlag == true)
            {
                // ノーツ生成
                m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.ONE_IN_HIT);
            }
            else if (m_rightStick.OutHitNotesFlag == true)
            {
                // ノーツ生成
                m_notesActionGauge.InstantiateNotes(NotesActionGauge_Script.NOTES_TYPE.ONE_OUT_HIT);
            }

            // 左スティックの状態を元に戻す
            m_leftStick.LeftStickState = 0;
            // 右スティックの状態を元に戻す
            m_rightStick.RightStickState = 0;
            // 時間を初期化
            m_leftStick.DoubleHitTime = 0;

            m_leftStick.InHitNotesFlag = false;
            m_leftStick.OutHitNotesFlag = false;
            m_rightStick.InHitNotesFlag = false;
            m_rightStick.OutHitNotesFlag = false;
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
    /// 衝突を検出した時の処理
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    public void OnTriggerEnter(Collider other)
    {
        // スティックに当たったら処理をする
        if (other.tag == "Stick")
        { 
            // アクティブにする
            isActive = true;
            // このドラムを現在のドラムにする
            m_manager.ChangeDrum(GetComponent<AttackDrum_Script>());
        }
    }

    /// <summary>
    /// 衝突したオブジェクトが離れた時の処理
    /// </summary>
    /// <param name="other">当たっていたオブジェクト</param>
    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }
}
