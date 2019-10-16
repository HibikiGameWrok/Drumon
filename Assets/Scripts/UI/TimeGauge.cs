using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGauge : MonoBehaviour
{

    // ゲージの進行を可視化する為のハンドル
    private GameObject m_childHandle;

    // ノーツプレハブ
    private GameObject m_prefabNotes;

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
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("ノーツが出たよ");
            Instantiate(m_prefabNotes, m_childHandle.transform.position, Quaternion.identity);
        } 
    }

}
