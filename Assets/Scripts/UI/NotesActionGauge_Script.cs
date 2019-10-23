//
//      FileName @ NoteActionGauge_Scrip.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16 (Wednesday)    
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesActionGauge_Script : MonoBehaviour
{
    // ゲージの最小値
    public const int MIN_GAUGE = 4;
    // ゲージの最大値
    public const int MAX_GAUGE = 4;

    // ノーツの速度
    [SerializeField]
    private float m_notesVel = 0.01f;

    // 行動ゲージが終わったか
    private bool m_finishFlag = false;

    // ゲージの進行を可視化する為のハンドル
    private Transform m_childHandle = null;
    

    // Start is called before the first frame update
    void Start()
    {
        // 子のオブジェクトを取得
        m_childHandle = this.transform.Find("Handle");
    }


    // Update is called once per frame
    void Update()
    {
        // ハンドルの動作処理
        HandleMove();
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
                m_childHandle.transform.position += new Vector3(m_notesVel, 0, 0);
            }
            else
            {
                // 到達したならば動作終了フラグを立てる
                m_finishFlag = true;
            }
        }
    }
}
