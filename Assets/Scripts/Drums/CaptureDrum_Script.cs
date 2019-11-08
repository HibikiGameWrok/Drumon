using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureDrum_Script : Drum_Script
{
    // メンバ変数

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    // 捕獲ドラムを叩いた回数
    private int m_captureCount;

    public int CaptureCount
    {
        get { return m_captureCount; }
        set { m_captureCount = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 何もしない
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize(DrumManager_Script manager)
    {
        // 親オブジェクトを入れる
        m_manager = manager;

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();

        m_captureCount = 0;
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
    /// 当たり判定の検出をする
    /// </summary>
    /// <param name="col">衝突した相手</param>
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Stick")
        {
            if (isActive == false)
            {
                // 捕獲用のドラムに変更する
                m_manager.ChangeDrum(GetComponent<CaptureDrum_Script>());
            }

            // アクティブにする
            isActive = true;
        }
    }

    // 捕獲処理
    public void Capture()
    {
        // 回復ドラムが叩かれたら
        if (m_leftStick.HitDrumFlag.IsFlag((uint)StickLeft_Script.HIT_DRUM.CAPTURE) == true || m_rightStick.HitDrumFlag2.IsFlag((uint)StickRight_Script.HIT_DRUM2.CAPTURE) == true)
        {
            // カウントアップ
            m_captureCount++;

            // 回復ドラムを叩いた判定フラグを伏せる
            m_leftStick.HitDrumFlag.OffFlag((uint)StickLeft_Script.HIT_DRUM.CAPTURE);
            m_rightStick.HitDrumFlag2.OffFlag((uint)StickRight_Script.HIT_DRUM2.CAPTURE);
            //m_leftStick.CaptureHit = false;
        }
    }

    /// <summary>
    /// 当たり判定から外れた時
    /// </summary>
    /// <param name="col">衝突した相手</param>
    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Stick")
        {
            Debug.Log("nonononononono");
        }
    }
}
