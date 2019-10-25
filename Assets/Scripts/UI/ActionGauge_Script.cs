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

public class ActionGauge_Script: MonoBehaviour
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
    public bool FinishFlag
    {
        get { return m_finishFlag; }
    }

    // ゲージの進行を可視化する為のハンドル
    private Transform m_childHandle = null;

    // ハンドル座標を保持するVector
    private Vector3 m_handlePos;
    public Vector3 HandlePos
    {
        get { return m_handlePos; }
    }

    private GameObject m_notesInstanceReceive = null;
    private NotesInstanceReceive_Script m_notesInstanceReceiveScript = null;
    private GameObject m_attackRecipe = null;
    private AttackRecipeManeger_Script m_attackRecipeScript = null;

    // Start is called before the first frame update
    void Start()
    {
        // 子のオブジェクトを取得
        m_childHandle = this.transform.Find("Handle");
        m_handlePos = this.transform.position;

        m_notesInstanceReceive = GameObject.Find("NotesInsetance");
        m_notesInstanceReceiveScript = m_notesInstanceReceive.GetComponent<NotesInstanceReceive_Script>();
        m_attackRecipe = GameObject.Find("AttackRecipeManeger");
        m_attackRecipeScript = m_attackRecipe.GetComponent<AttackRecipeManeger_Script>();
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
        // 動作終了フラグが立っていたら
        if (m_finishFlag)
        {
            // ノーツとゲージをリセットする
            m_notesInstanceReceiveScript.NotesReset();
        }

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

                // 現在のノーツと攻撃する為のノーツが合っているか見比べる
                m_attackRecipeScript.MatchAttackRecipe();
            }
        }
    }

    // UI初期化
    public void ResetGauge()
    {
        // 子を初期位置に戻す
        m_childHandle.transform.position = new Vector3(this.transform.position.x - MIN_GAUGE, this.transform.position.y, this.transform.position.z);
        // 終了フラグを初期化
        m_finishFlag = false;
    }
}
