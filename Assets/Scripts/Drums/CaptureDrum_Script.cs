using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureDrum_Script : Drum_Script
{
    // 定数
    // 確定捕獲の値
    public const int CAPTURE_CONFIRM = 100;
    
    // メンバ変数

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;
    //private Stick_Script m_stick;

    // 捕獲ドラムを叩いた回数
    private int m_captureCount;

    public int CaptureCount
    {
        get { return m_captureCount; }
        set { m_captureCount = value; }
    }

    // チュートリアル用のモンスター捕獲フラグ
    private bool m_tutorialCaptureFlag = false;
    // チュートリアル用のモンスター捕獲フラグのプロパティ
    public bool TutorialCaptureFlag
    {
        get { return m_tutorialCaptureFlag; }
        set { m_tutorialCaptureFlag = value; }
    }

    // キャプチャードラムのUIキャンバス
    private GameObject m_captureUIC;
    // キャプチャーモードテキスト
    private Transform m_captureModeText;

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

        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
        //m_stick = FindObjectOfType<Stick_Script>();

        m_captureCount = 0;

        m_captureUIC = GameObject.Find("CaptureCanvas");
        m_captureModeText = m_captureUIC.transform.Find("CaptureModeText");
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
        if (col.gameObject.tag == "StickLeft" || col.gameObject.tag == "StickRight")
        {
            if (isActive == false)
            {
                // 捕獲用のドラムに変更する
                m_manager.ChangeDrum(GetComponent<CaptureDrum_Script>());
            }

            // アクティブにする
            isActive = true;

            //m_stick = GameObject.FindGameObjectWithTag(col.gameObject.tag).GetComponent<Stick_Script>();
        }
    }

    // 捕獲処理
    public void Capture()
    {
        if (m_leftStick.HitDrumFlag.IsFlag((uint)StickLeft_Script.HIT_DRUM.CAPTURE) == true || m_rightStick.HitDrumFlag.IsFlag((uint)StickRight_Script.HIT_DRUM.CAPTURE) == true)
        {
            // カウントアップ
            m_captureCount++;

            // 捕獲ドラムを叩いた判定フラグを伏せる
            m_leftStick.HitDrumFlag.OffFlag((uint)StickLeft_Script.HIT_DRUM.CAPTURE);
            m_rightStick.HitDrumFlag.OffFlag((uint)StickRight_Script.HIT_DRUM.CAPTURE);
        }

        // 捕獲ドラムが叩かれたら
        //if (m_stick.LeftHitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.CAPTURE) == true || m_stick.RightHitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.CAPTURE) == true)
        //{
        //    // カウントアップ
        //    m_captureCount++;

        //    // 捕獲ドラムを叩いた判定フラグを伏せる
        //    m_stick.LeftHitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.CAPTURE);
        //    m_stick.RightHitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.CAPTURE);
        //    //m_leftStick.CaptureHit = false;
        //}
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
