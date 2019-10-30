using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchDrum_Script : Drum_Script
{
    private GameObject m_selectUI;
    private GameObject[] m_text;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    private Vector3 m_selectUIPos;

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

        m_selectUI = GameObject.Find("SelectUI");

        m_text = new GameObject[7];
        for (int i = 0; i < 7; i++)
        {
            m_text[i] = GameObject.Find("Text" + (i + 1));
        }

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();

        // UIをアクティブにする
        m_selectUI.transform.parent.gameObject.SetActive(true);
        m_selectUIPos = m_selectUI.transform.position;
        // UIを非アクティブにする
        m_selectUI.transform.parent.gameObject.SetActive(false);

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

        //--------------------------------------
        Debug.Log(m_selectUI.transform.position);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_leftStick.OpenUIFlag = true;
            m_leftStick.SelectCount++;
            m_selectUI.transform.position = new Vector3(m_selectUIPos.x, m_selectUIPos.y - (m_leftStick.SelectCount * 0.1f), m_selectUIPos.z);
        }
        //-----------------------------------------

        // 左スティックで外側を叩いたら
        if (m_leftStick.HitDrumFlag.IsFlag((uint)StickLeft_Script.HIT_DRUM.SWITCH) == true)
        {
            //m_selectUI.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0); // left, bottom
            //m_selectUI.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0); // right, top

            m_selectUI.transform.position = new Vector3(m_selectUIPos.x, m_selectUIPos.y - (m_leftStick.SelectCount * 0.1f), m_selectUIPos.z);

            // 選択ドラムを叩いた判定フラグを伏せる
            m_leftStick.HitDrumFlag.OffFlag((uint)StickLeft_Script.HIT_DRUM.SWITCH);
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
            m_selectUI.transform.parent.gameObject.SetActive(true);
            m_activeUIFlag = true;

            for (int i = 0; i < 7; i++)
            {
                if (CreatureList_Script.Get.List.DataList[i].name != null)
                {
                    // テキストに名前を入れる
                    Text text = m_text[i].GetComponent<Text>();
                    text.text = CreatureList_Script.Get.List.DataList[i].name;
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
            m_selectUI.transform.parent.gameObject.SetActive(false);
            m_activeUIFlag = false;
        }
    }
}
