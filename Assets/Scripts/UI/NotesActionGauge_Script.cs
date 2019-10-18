using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesActionGauge_Script : MonoBehaviour
{
    // ゲージの最小値
    public const int MIN_GAUGE = 4;
    // ゲージの最大値
    public const int MAX_GAUGE = 4;

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
    private Transform m_childHandle = null;
    // ノーツをまとめる親オブジェクト
    private Transform m_notesManager = null;
    // ノーツプレハブ
    private GameObject m_prefabNotes = null;


    // 行動ゲージが終わったか
    private bool m_finishFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        // 子のオブジェクトを取得
        m_childHandle = this.transform.Find("Handle");
        m_notesManager = this.transform.Find("NotesManager");

        // プレハブを取得
        m_prefabNotes = (GameObject)Resources.Load("InsPrefab/NotesPrefab");
    }


    // Update is called once per frame
    void Update()
    {
        // ハンドルの動作処理
        HandleMove();

        /// デバッグ用関数 ///
        KeyNotesinstace(); 
    }


    // ハンドルの動作処理
    private void HandleMove()
    {
        // 動作終了フラグが立ってないか
        if (m_finishFlag == false)
        {
            // 背景画像の端までハンドルが到達しているか
            if (m_childHandle.transform.position.x < (this.transform.position.x + MAX_GAUGE))
            {
                // x座標に加算、よって右方向へ移動
                m_childHandle.transform.position += new Vector3(0.05f, 0, 0);
            }
            else
            {
                // 到達したならば動作終了フラグを立てる
                m_finishFlag = true;
            }
        }
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
            GameObject InsNotes = (GameObject)Instantiate(m_prefabNotes, m_childHandle.transform.position, Quaternion.identity);
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
        foreach(Transform n in m_notesManager.transform)
        {
            GameObject.Destroy(n.gameObject);
        }

        // 子を初期位置に戻す
        m_childHandle.transform.position = new Vector3(this.transform.position.x - MIN_GAUGE, this.transform.position.y, this.transform.position.z);
        // 終了フラグを初期化
        m_finishFlag = false;
    }


    // デバッグ用キーによってノーツを出す
    private void KeyNotesinstace()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NotesReset();
        }
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

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            NotesReset();
        }
    }
}
