using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesActionGauge_Script : MonoBehaviour
{
    // ノーツの生成最大値
    public const int MAX_INS_NOTES = 8;

    public enum NOTES_TYPE : int
    {
        OneInHit,           // １スティックで内側を叩いた時
        OneOutHit,          // １スティックで外側を叩いた時
        DoubleInHit,        // ２スティックで内側を叩いた時
        DoubleOutHit,       // ２スティックで外側を叩いた時
    }
    // 出すノーツの種類
    NOTES_TYPE m_notesType;

    // ノーツの生成数を保持する変数
    private int m_notesCount = 0;

    // ゲージの進行を可視化する為のハンドル
    private GameObject m_childHandle = null;
    // ノーツプレハブ
    private GameObject m_prefabNotes = null;

    // 行動ゲージが終わったか
    private bool m_finishFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        // 子のハンドルを取得
        m_childHandle = GameObject.Find("Handle");
        // プレハブを取得
        m_prefabNotes = (GameObject)Resources.Load("InsPrefab/NotesPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();

        KeyNotesinstace();
    }

    // ハンドルの動作処理
    private void HandleMove()
    {
        if (m_finishFlag == false)
        {
            if (m_childHandle.transform.position.x < (this.transform.position.x + 4))
            {
                m_childHandle.transform.position += new Vector3(0.05f, 0, 0);
            }
            else
            {
                m_finishFlag = true;
            }
        }
    }

    // ノーツを生成する動作処理
    private void InstantiateNotes(NOTES_TYPE type)
    {　
        // ノーツのプレハブタイプを設定しロードする
        SetNotesPrefabPath(type);
        // ノーツの現在の数が設定値に達成していなければ
        if (m_notesCount < MAX_INS_NOTES)
        {
            // ハンドルの位置にプレハブを生成
            Instantiate(m_prefabNotes, m_childHandle.transform.position, Quaternion.identity);
            // ノーツの数を増加
            m_notesCount++;
        }
    }

    // ノーツの出す種類を設定
    private void SetNotesPrefabPath(NOTES_TYPE type)
    {
        // プレハブを取得
        m_prefabNotes = (GameObject)Resources.Load("InsPrefab/NotesPrefab" + (int)type);
    }

    // ノーツとゲージをリセットする
    public void NotesReset()
    {
        // ノーツの数を０にする
        m_notesCount = 0;
        // 子を初期位置に戻す
        m_childHandle.transform.position = new Vector3(this.transform.position.x - 4, 0, 0);
        // 終了フラグを初期化
        m_finishFlag = false;
    }

    // デバッグ用キーによってノーツを出す
    private void KeyNotesinstace()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            InstantiateNotes(NOTES_TYPE.OneInHit);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InstantiateNotes(NOTES_TYPE.OneOutHit);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InstantiateNotes(NOTES_TYPE.DoubleInHit);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            InstantiateNotes(NOTES_TYPE.DoubleOutHit);
        }
    }
}
