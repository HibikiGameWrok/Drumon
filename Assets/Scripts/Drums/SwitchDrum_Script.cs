using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchDrum_Script : Drum_Script
{
    // プレイヤーのモンスター
    [SerializeField]
    private GameObject m_playerCreature = null;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    // UIのアクティブフラグ
    private bool m_activeUIFlag;

    // スイッチドラムのUIキャンバス
    private GameObject m_switchUIC;
    // アイコン
    private Transform m_icon;

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

        m_leftStick = FindObjectOfType<StickLeft_Script>();
        m_rightStick = FindObjectOfType<StickRight_Script>();

        m_switchUIC = GameObject.Find("SwitchUI Canvas");
        m_icon = m_switchUIC.transform.Find("SwitchUI");
        // UIを非アクティブにする
        m_icon.gameObject.SetActive(false);

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
            m_icon.gameObject.SetActive(true);
            m_activeUIFlag = true;
        }
    }

    // UIの非表示
    public void CloseUI()
    {
        // UIの表示フラグが立っていなかったら
        if (m_leftStick.OpenUIFlag == false && m_activeUIFlag == true)
        {
            // 非アクティブにする
            m_icon.gameObject.SetActive(false);
            m_activeUIFlag = false;
        }
    }

    // アイコンの変更
    public void ChangeIcon()
    {
        // モンスターの変更フラグが立っていなかったら
        if (m_leftStick.CreatureChengeFlag == false)
        {
            // 左スティックで外側を叩いたら
            if (m_leftStick.HitDrumFlag.IsFlag((uint)StickLeft_Script.HIT_DRUM.SWITCH) == true)
            {
                // カウントダウン
                m_leftStick.PickCount--;

                if (m_leftStick.PickCount >= 0)
                {
                    if (CreatureList_Script.Get.List.DataList[m_leftStick.PickCount] != null)
                    {
                        Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + CreatureList_Script.Get.List.DataList[m_leftStick.PickCount].name + " Icon");
                        Image image = m_icon.GetComponent<Image>();
                        image.sprite = sprite;
                    }
                }
                else
                {
                    for (int i = CreatureList_Script.Get.List.DataList.Length - 1; i > 0; i--)
                    {
                        m_leftStick.PickCount = i;
                        if (CreatureList_Script.Get.List.DataList[i] != null)
                        {
                            break;
                        }
                    }

                    Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + CreatureList_Script.Get.List.DataList[m_leftStick.PickCount].name + " Icon");
                    Image image = m_icon.GetComponent<Image>();
                    image.sprite = sprite;
                }

                // 選択ドラムを叩いた判定フラグを伏せる
                m_leftStick.HitDrumFlag.OffFlag((uint)StickLeft_Script.HIT_DRUM.SWITCH);
            }
            // 右スティックで外側を叩いたら
            if (m_rightStick.HitDrumFlag.IsFlag((uint)StickRight_Script.HIT_DRUM2.SWITCH) == true)
            {
                m_leftStick.PickCount++;

                if (CreatureList_Script.Get.List.DataList[m_leftStick.PickCount] != null)
                {
                    if (m_leftStick.PickCount <= CreatureList_Script.Get.List.DataList.Length)
                    {
                        Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + CreatureList_Script.Get.List.DataList[m_leftStick.PickCount].name + " Icon");
                        Image image = m_icon.GetComponent<Image>();
                        image.sprite = sprite;
                    }
                }
                else
                {
                    m_leftStick.PickCount = 0;

                    Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + CreatureList_Script.Get.List.DataList[m_leftStick.PickCount].name + " Icon");
                    Image image = m_icon.GetComponent<Image>();
                    image.sprite = sprite;
                }

                // 選択ドラムを叩いた判定フラグを伏せる
                m_rightStick.HitDrumFlag.OffFlag((uint)StickRight_Script.HIT_DRUM2.SWITCH);
            }
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
