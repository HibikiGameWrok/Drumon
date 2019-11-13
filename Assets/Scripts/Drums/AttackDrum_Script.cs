/*----------------------------------------------------------*/
//  file:      AttackDrum_Scripts.cs					    |
//				 											|
//  brief:    攻撃用のドラムクラスのスクリプト		        | 
//              Attack Drum class  				            |
//															|
//  date:	2019.10.9										|
//															|
//  author: Renya Fukuyama									|
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

    private GameObject m_musicalScore;
    private Transform m_notesManager;
    private TestNotesInstance m_notesInsRec;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;
    // スティックマネージャー
    private GameObject m_stickManager;
    private StickManager_Script m_stickManagerScript;

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

        m_musicalScore = GameObject.Find("MusicScore");
        m_notesManager = m_musicalScore.transform.Find("NotesManager");
        m_notesInsRec = m_notesManager.GetComponent<TestNotesInstance>();

        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
        m_stickManager = GameObject.Find("StickManeger");
        m_stickManagerScript = m_stickManager.GetComponent<StickManager_Script>();
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
        if (isActive == false)
        {
            // このドラムを現在のドラムにする
            m_manager.ChangeDrum(GetComponent<AttackDrum_Script>());

            // アクティブにする
            isActive = true;
        }
    }

    /// <summary>
    /// 衝突したオブジェクトが離れた時の処理
    /// </summary>
    /// <param name="other">当たっていたオブジェクト</param>
    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }

    // ノーツの生成処理
    public void GenerateNotes()
    {
        // 内側を同時に叩いていたら
        if (m_stickManagerScript.DoubleInHitFlag == true)
        {
            // ノーツ生成
            m_notesInsRec.InstanceNotes((int)TestNotesInstance.NOTES_TYPE.DON_NOTE);
            // 内側を同時に叩いた判定フラグを伏せる
            m_stickManagerScript.DoubleInHitFlag = false;

            // 時間を初期化
            m_stickManagerScript.DoubleHitTime = 0;

            m_leftStick.HitDrumFlag.OffFlag(((uint)Stick_Script.HIT_DRUM.ATTACK));
            m_rightStick.HitDrumFlag.OffFlag(((uint)Stick_Script.HIT_DRUM.ATTACK));

            // 内側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            // 内側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
        }
        // 外側を同時に叩いていたら
        else if (m_stickManagerScript.DoubleOutHitFlag == true)
        {
            // ノーツ生成
            m_notesInsRec.InstanceNotes((int)TestNotesInstance.NOTES_TYPE.KAN_NOTE);
            // 外側を同時に叩いた判定フラグを伏せる
            m_stickManagerScript.DoubleOutHitFlag = false;

            // 時間を初期化
            m_stickManagerScript.DoubleHitTime = 0;

            m_leftStick.HitDrumFlag.OffFlag(((uint)Stick_Script.HIT_DRUM.ATTACK));
            m_rightStick.HitDrumFlag.OffFlag(((uint)Stick_Script.HIT_DRUM.ATTACK));

            // 内側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            // 内側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
        }

        // 時間が0になったら
        if (m_stickManagerScript.DoubleHitTime < 0)
        {
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)TestNotesInstance.NOTES_TYPE.DO_NOTE);
            }
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)TestNotesInstance.NOTES_TYPE.KA_NOTE);
            }

            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)TestNotesInstance.NOTES_TYPE.DO_NOTE);
            }
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)TestNotesInstance.NOTES_TYPE.KA_NOTE);
            }

            // 時間を初期化
            m_stickManagerScript.DoubleHitTime = 0;

            // 内側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            // 内側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);

            m_leftStick.HitDrumFlag.OffFlag(((uint)Stick_Script.HIT_DRUM.ATTACK));
            m_rightStick.HitDrumFlag.OffFlag(((uint)Stick_Script.HIT_DRUM.ATTACK));
        }
    }
}
