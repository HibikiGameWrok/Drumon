using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesInstanceReceive_Script : MonoBehaviour
{
    // ノーツの生成最大値
    public const int MAX_INS_NOTES = 8;

    // スティックの叩き方によって出すノーツの種類
    public enum NOTES_TYPE : int
    {
        NONE,               // 何もない
        ONE_IN_HIT,           // １スティックで内側を叩いた時
        DOUBLE_IN_HIT,        // ２スティックで内側を叩いた時
        ONE_OUT_HIT,          // １スティックで外側を叩いた時
        DOUBLE_OUT_HIT,       // ２スティックで外側を叩いた時
    }
    NOTES_TYPE m_notesType;

    // ノーツの生成数を保持する変数
    private int m_notesCount = 0;

    // ActionGaugeUIオブジェクトの取得用変数
    private GameObject m_actionGauge = null;
    // ActionGaugeScriptを取得用変数
    private ActionGauge_Script m_actionGaugeScript = null;

    // ノーツをまとめる親オブジェクト
    private GameObject m_notesManager = null;
    // ノーツをまとめる親オブジェクトにアタッチしているスクリプトを取得
    private NotesManager_Script m_notesmanager_script = null;
    // ノーツプレハブ
    private GameObject m_prefabNotes = null;

    // Start is called before the first frame update
    void Start()
    {
        // ActionGaugeUIオブジェクトを取得
        m_actionGauge = GameObject.Find("ActionGauge");
        // オブジェクト内のアタッチされたScriptを取得
        m_actionGaugeScript = m_actionGauge.GetComponent<ActionGauge_Script>();


        // 子のノーツを管理している親オブジェクトを取得
        m_notesManager = GameObject.Find("NotesManager");
        // 子のオブジェクトにアタッチしているScriptを取得
        m_notesmanager_script = m_notesManager.GetComponent<NotesManager_Script>();
        // プレハブを取得
        m_prefabNotes = (GameObject)Resources.Load("InsPrefab/NotesPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        /// デバッグ用関数 ///
        KeyNotesinstace();
    }

    // ドラムとスティックが当たると呼ばれる関数
    // 引数 : NOTES_TYPE type -- 叩き方
    public void InstantiateNotes(NOTES_TYPE type)
    {
        // ノーツのプレハブタイプを設定しロードする
        SetNotesPrefabPath(type);
        // ノーツの現在の数が設定値に達成していなければ
        if (m_notesCount < MAX_INS_NOTES)
        {
            // ハンドルの位置にプレハブを生成
            GameObject InsNotes = (GameObject)Instantiate(m_prefabNotes, m_actionGaugeScript.HandlePos, Quaternion.identity);
            InsNotes.transform.parent = m_notesManager.transform;
            // ノーツの数を増加
            m_notesCount++;
        }
    }


    // ドラムの叩き方によって、出すノールのプレハブをロードする関数
    // InstantiateNotes関数で呼ばれる
    // 引数 : NOTES_TYPE type -- 叩き方
    private void SetNotesPrefabPath(NOTES_TYPE type)
    {
        // プレハブを取得
        m_prefabNotes = (GameObject)Resources.Load("InsPrefab/NotesPrefab" + (int)type);
    }

    // ノーツとゲージをリセットする
    // どこでも呼べるようにpublic化
    public void NotesReset()
    {
        // ノーツの数を０にする
        m_notesCount = 0;
        // ノーツ管理オブジェクトの子として生成されたノーツオブジェクトを全て消す
        foreach (Transform n in m_notesManager.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        m_actionGaugeScript.ResetGauge();
    }

    // デバッグ用キーによってノーツを出す
    private void KeyNotesinstace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NotesReset();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InstantiateNotes(NOTES_TYPE.ONE_IN_HIT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InstantiateNotes(NOTES_TYPE.DOUBLE_IN_HIT);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InstantiateNotes(NOTES_TYPE.ONE_OUT_HIT);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            InstantiateNotes(NOTES_TYPE.DOUBLE_OUT_HIT);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            NotesReset();
        }
    }
}
