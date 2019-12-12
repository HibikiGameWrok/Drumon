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
    public enum TUTORIAL_HIT_PATTERN
    {
        IN_HIT = (1 << 0),          // 内側を叩いた判定(0001)
        OUT_HIT = (1 << 1),         // 外側を叩いた判定(0010)
        DOUBLE_IN_HIT = (1 << 2),   // 内側を同時に叩いた判定(0100)
        DOUBLE_OUT_HIT = (1 << 3)   // 外側を同時に叩いた判定(1000)
    }

    // メンバ変数

    private GameObject m_musicalScore;
    private Transform m_notesManager;
    private NotesInstance_Script m_notesInsRec;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;
    // スティックマネージャー
    private GameObject m_stickManager;
    private StickManager_Script m_stickManagerScript;

    // 内側のドラム
    private Transform m_inDrum;
    // 外側のドラム
    private Transform m_outDrum;

    // チュートリアル用の叩いた判定フラグ管理
    private Flag_Script m_tutorialFlag;
    // チュートリアル用の叩いた判定フラグ管理のプロパティ
    public Flag_Script TutorialFlag
    {
        get { return m_tutorialFlag; }
    }

    // マテリアル
    [SerializeField]
    private Material[] m_drumMaterials;
    private Renderer m_inDrumRender;
    private Renderer m_outDrumRender;

    private bool m_changeMaterialFlag = false;
    private int m_changeMaterialCount = 0;

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
        m_notesInsRec = m_notesManager.GetComponent<NotesInstance_Script>();

        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
        m_stickManager = GameObject.Find("StickManeger");
        m_stickManagerScript = m_stickManager.GetComponent<StickManager_Script>();

        m_inDrum = this.gameObject.transform.Find("InDrum");
        m_outDrum = this.gameObject.transform.Find("OutDrum");

        m_inDrumRender = m_inDrum.GetComponent<Renderer>();
        m_outDrumRender = m_outDrum.GetComponent<Renderer>();

        m_tutorialFlag = new Flag_Script();
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

        if (m_changeMaterialFlag == true)
        {
            m_changeMaterialCount++;
        }

        if (m_changeMaterialCount >= 3)
        {
            // マテリアル変更
            m_inDrumRender.sharedMaterial = m_drumMaterials[0];
            // マテリアル変更
            m_outDrumRender.sharedMaterial = m_drumMaterials[1];

            m_changeMaterialCount = 0;
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

    // 内側に当たった処理
    public void InHit()
    {
        // マテリアル変更
        m_inDrumRender.sharedMaterial = m_drumMaterials[2];

        m_changeMaterialFlag = true;
    }

    // 外側に当たった処理
    public void OutHit()
    {
        // マテリアル変更
        m_outDrumRender.sharedMaterial = m_drumMaterials[3];

        m_changeMaterialFlag = true;
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
            m_notesInsRec.InstanceNotes((int)NotesInstance_Script.NOTES_TYPE.DON_NOTE);
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

            m_tutorialFlag.OnFlag((uint)TUTORIAL_HIT_PATTERN.DOUBLE_IN_HIT);
        }
        // 外側を同時に叩いていたら
        else if (m_stickManagerScript.DoubleOutHitFlag == true)
        {
            // ノーツ生成
            m_notesInsRec.InstanceNotes((int)NotesInstance_Script.NOTES_TYPE.KAN_NOTE);
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

            m_tutorialFlag.OnFlag((uint)TUTORIAL_HIT_PATTERN.DOUBLE_OUT_HIT);
        }

        // 時間が0になったら
        if (m_stickManagerScript.DoubleHitTime < 0)
        {
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)NotesInstance_Script.NOTES_TYPE.DO_NOTE);

                m_tutorialFlag.OnFlag((uint)TUTORIAL_HIT_PATTERN.IN_HIT);
            }
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)NotesInstance_Script.NOTES_TYPE.KA_NOTE);

                m_tutorialFlag.OnFlag((uint)TUTORIAL_HIT_PATTERN.OUT_HIT);
            }

            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)NotesInstance_Script.NOTES_TYPE.DO_NOTE);

                m_tutorialFlag.OnFlag((uint)TUTORIAL_HIT_PATTERN.IN_HIT);
            }
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                // ノーツ生成
                m_notesInsRec.InstanceNotes((int)NotesInstance_Script.NOTES_TYPE.KA_NOTE);

                m_tutorialFlag.OnFlag((uint)TUTORIAL_HIT_PATTERN.OUT_HIT);
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
