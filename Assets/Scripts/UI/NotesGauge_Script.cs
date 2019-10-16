//
//      FileName @ TimeGauge.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16      
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesGage_Script : MonoBehaviour
{
    // ノーツの生成最大値
    public const int MAX_INS_NOTES = 8;

    // ノーツの生成数を保持する変数
    private int m_notesCount = 0;

    // ゲージの進行を可視化する為のハンドル
    private GameObject m_childHandle = null;

    // ノーツプレハブ
    private GameObject m_prefabNotes = null;

    // ノーツを出す際の指示フラグ
    private bool m_instanceFlag = false;



    // Awake is called before the Start Function after object becomes active
    void Awake()
    {
        // 子のハンドルを取得
        m_childHandle = GameObject.Find("Handle");

        // プレハブを取得
        m_prefabNotes = (GameObject)Resources.Load("InsPrefab/NotesPrefab");
    }

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
        InstantiateNotes();
    }


    // ハンドルの動作処理
    private void HandleMove()
    {
        if (m_childHandle.transform.position.x < (this.transform.position.x + 4))
        {
            m_childHandle.transform.position += new Vector3(0.05f, 0, 0);
        }
    }

    // ノーツを出す動作処理
    private void InstantiateNotes()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("ノーツが出たよ");
            if (m_notesCount < MAX_INS_NOTES)
            {
                Instantiate(m_prefabNotes, m_childHandle.transform.position, Quaternion.identity);
                m_notesCount++;
            }
        } 
    }

}
