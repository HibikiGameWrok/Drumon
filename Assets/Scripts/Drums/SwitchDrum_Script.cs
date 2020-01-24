using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SwitchDrum_Script : Drum_Script
{
    // プレイヤーのモンスター
    [SerializeField]
    private GameObject m_playerCreature = null;

    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;
    // スティックマネージャー
    private GameObject m_stickManager;
    private StickManager_Script m_stickManagerScript;

    // UIのアクティブフラグ
    private bool m_activeUIFlag = false;

    // スイッチドラムのUIキャンバス
    private GameObject m_switchUIC;
    // アイコン
    private Transform m_icon;

    // モンスターの変更フラグ
    private bool m_creatureChengeFlag = false;

    // 攻撃レシピのオブジェクト
    private GameObject m_abilitySheet_Wood = null;

    private Transform m_recipeNote = null;

    private AttackRecipeNotesUI_Script m_recipeNotesUI_Script = null;
    private AttackRecipiTextUI_Script m_recipeTextUI_Script = null;

    // チュートリアル用のモンスター変更フラグ
    private bool m_tutorialChengeFlag = false;
    // チュートリアル用のモンスター変更フラグのプロパティ
    public bool TutorialChengeFlag
    {
        get { return m_tutorialChengeFlag; }
        set { m_tutorialChengeFlag = value; }
    }

    // コスト
    private CostUI_Script m_costUIScript = null;

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

        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();
        m_stickManager = GameObject.Find("StickManeger");
        m_stickManagerScript = m_stickManager.GetComponent<StickManager_Script>();
        m_abilitySheet_Wood = GameObject.Find("AbilitySheet_Wood");
        m_recipeNote = m_abilitySheet_Wood.transform.Find("AttackRecipeCanvas");
        m_recipeTextUI_Script = m_recipeNote.GetComponent<AttackRecipiTextUI_Script>();
        m_recipeNote = m_abilitySheet_Wood.transform.Find("Notes");
        m_recipeNotesUI_Script = m_recipeNote.GetComponent<AttackRecipeNotesUI_Script>();
        

        m_switchUIC = GameObject.Find("SwitchUI Canvas");
        m_icon = m_switchUIC.transform.Find("SwitchUI");

        m_costUIScript = GameObject.Find("Slider").GetComponent<CostUI_Script>();
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

    // UIの表示
    public void OpenUI()
    {
        // UIの表示フラグが立っていたら
        if (m_stickManagerScript.OpenUIFlag == true && m_activeUIFlag == false)
        {
            // アクティブにする
            m_icon.gameObject.SetActive(true);
            m_activeUIFlag = true;

            // カウントの初期化
            m_stickManagerScript.PickCount = 0;

            if (m_playerCreature != null)
            {
                // 場に出ているモンスターと名前が一緒だったら
                if (m_playerCreature.GetComponent<PlayerCreature_Script>().GetData() == CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount])
                {
                    // カウントアップ
                    m_stickManagerScript.PickCount++;
                }
            }

            if (m_stickManagerScript.PickCount < CreatureList_Script.Get.List.DataList.Length)
            {
                if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                {
                    Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + Regex.Replace(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName, @"[^a-z,A-Z]", "") + " Icon");
                    Image image = m_icon.GetComponent<Image>();
                    image.sprite = sprite;
                }
                // Boxにモンスターが1体だったら
                else
                {
                    Sprite sprite = Resources.Load<Sprite>("");
                    Image image = m_icon.GetComponent<Image>();
                    image.sprite = sprite;
                }
            }    

            // 選択ドラムを叩いた判定フラグを伏せる
            m_leftStick.HitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.SWITCH);
            // 内側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            // 選択ドラムを叩いた判定フラグを伏せる
            m_rightStick.HitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.SWITCH);
            // 内側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            // 外側を叩いた判定フラグを伏せる
            m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
        }
    }

    // UIの非表示
    public void CloseUI()
    {
        // UIの表示フラグが立っていなかったら
        if (m_stickManagerScript.OpenUIFlag == false && m_activeUIFlag == true)
        {
            // 内側を叩かれたら
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true || m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // 非アクティブにする
                m_icon.gameObject.SetActive(false);
                m_activeUIFlag = false;

                // モンスターの変更フラグを立てる
                m_creatureChengeFlag = true;

                // 選択ドラムを叩いた判定フラグを伏せる
                m_leftStick.HitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.SWITCH);
                // 内側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
                // 選択ドラムを叩いた判定フラグを伏せる
                m_rightStick.HitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.SWITCH);
                // 内側を叩いた判定フラグを伏せる
                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }

        }
    }

    // アイコンの変更
    public void ChangeIcon()
    {
        // モンスターの変更フラグが立っていなかったら
        if (m_creatureChengeFlag == false)
        {
            // UIのアクティブフラグが立っていたら
            if (m_activeUIFlag == true)
            {
                // 左スティックで外側を叩いたら
                if (m_leftStick.HitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.SWITCH) == true && m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
                {
                    // カウントダウン
                    m_stickManagerScript.PickCount--;

                    if (m_stickManagerScript.PickCount >= 0)
                    {
                        if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                        {
                            if (m_playerCreature != null)
                            {
                                // 場に出ているモンスターと名前が一緒だったら
                                if (m_playerCreature.GetComponent<PlayerCreature_Script>().GetData() == CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount])
                                {
                                    // カウントアップ
                                    m_stickManagerScript.PickCount--;
                                }
                            }
                        }
                    }

                    if (m_stickManagerScript.PickCount >= 0)
                    {
                        if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                        {
                            Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + Regex.Replace(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName, @"[^a-z,A-Z]", "") + " Icon");
                            Image image = m_icon.GetComponent<Image>();
                            image.sprite = sprite;
                        }
                    }
                    else
                    {
                        for (int i = CreatureList_Script.Get.List.DataList.Length - 1; i >= 0; i--)
                        {
                            m_stickManagerScript.PickCount = i;
                            if (CreatureList_Script.Get.List.DataList[i].drumonName != "")
                            {
                                break;
                            }
                        }

                        if (m_playerCreature != null)
                        {
                            // 場に出ているモンスターと名前が一緒だったら
                            if (m_playerCreature.GetComponent<PlayerCreature_Script>().GetData() == CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount])
                            {
                                // カウントアップ
                                m_stickManagerScript.PickCount--;
                            }
                        }

                        if (m_stickManagerScript.PickCount >= 0)
                        {
                            if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                            {
                                Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + Regex.Replace(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName, @"[^a-z,A-Z]", "") + " Icon");
                                Image image = m_icon.GetComponent<Image>();
                                image.sprite = sprite;
                            }
                        }
                        // Boxにモンスターが1体だったら
                        else
                        {
                            Sprite sprite = Resources.Load<Sprite>("");
                            Image image = m_icon.GetComponent<Image>();
                            image.sprite = sprite;
                        }
                    }

                    // 選択ドラムを叩いた判定フラグを伏せる
                    m_leftStick.HitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.SWITCH);
                    // 外側を叩いた判定フラグを伏せる
                    m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
                }
                // 右スティックで外側を叩いたら
                if (m_rightStick.HitDrumFlag.IsFlag((uint)Stick_Script.HIT_DRUM.SWITCH) == true && m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
                {
                    // カウントアップ
                    m_stickManagerScript.PickCount++;

                    if (m_stickManagerScript.PickCount < CreatureList_Script.Get.List.DataList.Length)
                    {
                        if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                        {
                            if (m_playerCreature != null)
                            {
                                // 場に出ているモンスターと名前が一緒だったら
                                if (m_playerCreature.GetComponent<PlayerCreature_Script>().GetData() == CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount])
                                {
                                    // カウントアップ
                                    m_stickManagerScript.PickCount++;
                                }
                            }
                        }
                    }

                    if (m_stickManagerScript.PickCount < CreatureList_Script.Get.List.DataList.Length)
                    {
                        if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                        {
                            Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + Regex.Replace(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName, @"[^a-z,A-Z]", "") + " Icon");
                            Image image = m_icon.GetComponent<Image>();
                            image.sprite = sprite;
                        }
                        else
                        {
                            // カウントを初期化
                            m_stickManagerScript.PickCount = 0;

                            if (m_playerCreature != null)
                            {
                                // 場に出ているモンスターと名前が一緒だったら
                                if (m_playerCreature.GetComponent<PlayerCreature_Script>().GetData() == CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount])
                                {
                                    // カウントアップ
                                    m_stickManagerScript.PickCount++;
                                }
                            }

                            if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                            {
                                Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + Regex.Replace(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName, @"[^a-z,A-Z]", "") + " Icon");
                                Image image = m_icon.GetComponent<Image>();
                                image.sprite = sprite;
                            }
                        }
                    }
                    else
                    {
                        // カウントを初期化
                        m_stickManagerScript.PickCount = 0;

                        if (m_playerCreature != null)
                        {
                            // 場に出ているモンスターと名前が一緒だったら
                            if (m_playerCreature.GetComponent<PlayerCreature_Script>().GetData() == CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount])
                            {
                                // カウントアップ
                                m_stickManagerScript.PickCount++;
                            }
                        }

                        if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
                        {
                            Sprite sprite = Resources.Load<Sprite>("UI/Icon/" + Regex.Replace(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName, @"[^a-z,A-Z]", "") + " Icon");
                            Image image = m_icon.GetComponent<Image>();
                            image.sprite = sprite;
                        }
                    }

                    // 選択ドラムを叩いた判定フラグを伏せる
                    m_rightStick.HitDrumFlag.OffFlag((uint)Stick_Script.HIT_DRUM.SWITCH);
                    // 外側を叩いた判定フラグを伏せる
                    m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
                }
            }
        }
    }

    // モンスターの変更
    public void ChengeCreature()
    {
        // モンスターの変更フラグが立っていたら
        if (m_creatureChengeFlag == true)
        {
            if (CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount].drumonName != "")
            {
                if (m_playerCreature != null)
                {
                    if (m_costUIScript.GageEnd() == false)
                    {
                        // モンスターを変更
                        m_playerCreature.GetComponent<PlayerCreature_Script>().ChangeData(CreatureList_Script.Get.List.DataList[m_stickManagerScript.PickCount]);

                        // コストダウン
                        m_costUIScript.CostDawn(1);

                        m_tutorialChengeFlag = true;
                    }
                }
            }
            // モンスターの変更フラグを伏せる
            m_creatureChengeFlag = false;
        }
    }
}
