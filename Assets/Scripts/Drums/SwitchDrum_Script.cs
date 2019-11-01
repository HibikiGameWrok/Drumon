using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchDrum_Script : Drum_Script
{
    private GameObject m_selectUIC;
    private Transform m_backgroundUI;
    private Transform m_cursorUI;
    private Transform[] m_text;
    private string[] m_creatureName;

    [SerializeField]
    private GameObject m_playerCreature;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    private Vector3 m_cursorUIPos;

    // UIのアクティブフラグ
    private bool m_activeUIFlag;

    // Start is called before the first frame update
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

        m_selectUIC = GameObject.Find("SelectUI Canvas");
        m_backgroundUI = m_selectUIC.transform.Find("BackgroundUI");
        m_cursorUI = m_backgroundUI.transform.Find("CursorUI");
        m_text = new Transform[6];
        m_creatureName = new string[6];

        for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            m_text[i] = m_backgroundUI.transform.Find("Text" + (i + 1));

            if (CreatureList_Script.Get.List.DataList[i] != null)
            {
                m_creatureName[i] = CreatureList_Script.Get.List.DataList[i].name;
            }
        }

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();

        m_cursorUIPos = m_backgroundUI.transform.position + new Vector3(0.0f, 0.3f, 0.0f);

        // UIを非アクティブにする
        m_backgroundUI.gameObject.SetActive(false);

        m_activeUIFlag = false;

       
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
    /// 衝突を検出した時の処理
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    public void OnTriggerEnter(Collider other)
    {
        if (isActive == false)
        {
            // このドラムを現在のドラムにする
            m_manager.ChangeDrum(GetComponent<SwitchDrum_Script>());

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

    // UIの表示
    public void OpenUI()
    {
        // UIの表示フラグが立っていたら
        if (m_leftStick.OpenUIFlag == true && m_activeUIFlag == false)
        {
            // アクティブにする
            m_backgroundUI.gameObject.SetActive(true);
            m_activeUIFlag = true;

            for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
            {
                if (m_creatureName[i] != null)
                {
                    // テキストに名前を入れる
                    Text text = m_text[i].GetComponent<Text>();
                    text.text = m_creatureName[i];
                }
            }
        }
    }

    // UIの非表示
    public void CloseUI()
    {
        // UIの表示フラグが立っていなかったら
        if (m_leftStick.OpenUIFlag == false && m_activeUIFlag == true)
        {
            // 非アクティブにする
            m_backgroundUI.transform.gameObject.SetActive(false);
            m_activeUIFlag = false;
        }
    }

    // カーソルの移動
    public void MoveCursor()
    {
        // 左スティックで外側を叩いたら
        if (m_leftStick.HitDrumFlag.IsFlag((uint)StickLeft_Script.HIT_DRUM.SWITCH) == true)
        {
            // カーソルの移動
            m_cursorUI.transform.position = new Vector3(m_cursorUIPos.x, m_cursorUIPos.y - (m_leftStick.PickCount * 0.1f), m_cursorUIPos.z);

            // 選択ドラムを叩いた判定フラグを伏せる
            m_leftStick.HitDrumFlag.OffFlag((uint)StickLeft_Script.HIT_DRUM.SWITCH);
        }
        // 右スティックで外側を叩いたら
        if (m_rightStick.HitDrumFlag.IsFlag((uint)StickRight_Script.HIT_DRUM.SWITCH) == true)
        {
            // カーソルの移動
            m_cursorUI.transform.position = new Vector3(m_cursorUIPos.x, m_cursorUIPos.y - (m_leftStick.PickCount * 0.1f), m_cursorUIPos.z);

            // 選択ドラムを叩いた判定フラグを伏せる
            m_rightStick.HitDrumFlag.OffFlag((uint)StickRight_Script.HIT_DRUM.SWITCH);
        }
    }

    // モンスターの変更
    public void ChengeCreature()
    {
        // モンスターの変更フラグが立っていたら
        if (m_leftStick.CreatureChengeFlag == true)
        {
            if (CreatureList_Script.Get.List.DataList[m_leftStick.PickCount] != null)
            {
                if (m_playerCreature != null)
                {
                    // モンスターを変更
                    m_playerCreature.GetComponent<PlayerCreature_Script>().ChangeData(CreatureList_Script.Get.List.DataList[m_leftStick.PickCount]);
                }
            }
            // モンスターの変更フラグを伏せる
            m_leftStick.CreatureChengeFlag = false;
        }
    }
}
