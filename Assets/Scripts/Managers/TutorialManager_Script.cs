using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager_Script : SingletonBase_Script<TutorialManager_Script>
{
    [SerializeField]
    private PlayerCreature_Script m_playerCreature = null;

    public PlayerCreature_Script PlayerCreature
    {
        get { return m_playerCreature; }
    }

    [SerializeField]
    private EnemyCreature_Script m_enemyCreature = null;

    public EnemyCreature_Script EnemyCreature
    {
        get { return m_enemyCreature; }
    }

    private ICreature_Script m_nowMove;
    private ICreature_Script m_nextMove;

    private bool m_isSetting;

    private float m_attackSpan;

    [SerializeField]
    private BoolReactiveProperty m_isFinish = new BoolReactiveProperty(false);

    public IReadOnlyReactiveProperty<bool> IsFinish => m_isFinish;

    [SerializeField]
    private BoolReactiveProperty m_isAllFinish = new BoolReactiveProperty(false);

    public IReadOnlyReactiveProperty<bool> IsAllFinish => m_isAllFinish;

    // チュートリアルを表示中かどうかのフラグ
    private bool m_tutorialModeFlag = true;
    // 実践中かどうかのフラグ
    private bool m_practiceModeFlag = false;
    // プレイヤーのドラモンの行動フラグ
    private bool m_drumonExecuteFlag = false;
    // 敵の行動フラグ
    private bool m_enemyExecuteFlag = false;

    // バトルシーン用
    // チュートリアルキャンバス
    private GameObject m_tutorialCanvas = null;
    // 吹き出し1
    private GameObject m_battleFrame1 = null;
    // テキストの配列
    private GameObject[] m_textArray = null;
    // バトルシーケンステキスト
    private GameObject m_explainBattleSequenceText = null;
    // HP説明テキスト
    private GameObject m_explainHPText = null;
    // 攻撃ドラム説明テキスト
    private GameObject m_explainAttackDrumText = null;
    // 右矢印テキスト
    private GameObject m_rightArrowText = null;
    // バトルシステム説明テキスト
    private GameObject m_explainBattleSystemText = null;
    // バトルシステム説明テキスト2
    private GameObject m_explainBattleSystemText2 = null;
    // 内側を叩いた時のチェック
    private GameObject m_inHitCheckImage = null;
    // 外側を叩いた時のチェック
    private GameObject m_outHitCheckImage = null;
    // 内側を同時に叩いた時のチェック
    private GameObject m_doubleInHitCheckImage = null;
    // 外側を同時に叩いた時のチェック
    private GameObject m_doubleOutHitCheckImage = null;
    // 攻撃成功テキスト
    private GameObject m_succesAttackText = null;
    // 敵撃破テキスト
    private GameObject m_enemyKillText = null;
    // チュートリアルの終了テキスト
    private GameObject m_tutorialEndText = null;
    // チュートリアルの終了テキスト2
    private GameObject m_tutorialEndText2 = null;

    // チュートリアルスイッチキャンバス
    private GameObject m_tutorialSwitchCanvas = null;
    // 吹き出し2
    private GameObject m_battleFrame2 = null;
    // 選択ドラム説明テキスト
    private GameObject m_explainSwitchDrumText = null;
    // 選択ドラム説明テキスト2
    private GameObject m_explainSwitchDrumText2 = null;
    // 左矢印テキスト
    private GameObject m_leftArrowText = null;

    // チュートリアルキャプチャーキャンバス
    private GameObject m_tutorialCaptureCanvas = null;
    // 吹き出し3
    private GameObject m_battleFrame3 = null;
    // 捕獲ドラム説明テキスト
    private GameObject m_explainCaptureDrumText = null;
    // 右矢印テキスト
    private GameObject m_rightArrowText2 = null;

    // チュートリアル技リストのキャンバス
    private GameObject m_tutorialAbilityCanvas = null;
    // 吹き出し4
    private GameObject m_battleFrame4 = null;
    // 技リストの説明テキスト
    private GameObject m_explainAbilityText = null;
    // 技リストの説明テキスト2
    private GameObject m_explainAbilityText2 = null;
    // 技リストの説明テキスト3
    private GameObject m_explainAbilityText3 = null;
    // 技リストの説明テキスト4
    private GameObject m_explainAbilityText4 = null;

    // チュートリアルミュージックスコアキャンバス
    private GameObject m_tutorialMusicScoreCanvas = null;
    // 吹き出し5
    private GameObject m_battleFrame5 = null;
    // タイマーの説明テキスト
    private GameObject m_explainTimerText = null;
    // ノーツリセットの説明テキスト
    private GameObject m_explainNotesResetText = null;

    // キャプチャーシーン用
    // チュートリアル説明キャンバス
    private GameObject m_tutorialExplainCanvas = null;
    // 吹き出し1
    private GameObject m_captureFrame1 = null;
    // 説明テキスト1
    private GameObject m_explainText1 = null;
    // 説明テキスト2
    private GameObject m_explainText2 = null;
    // 説明テキスト3
    private GameObject m_explainText3 = null;
    // 左矢印テキスト3
    private GameObject m_leftArrowText3 = null;
    // 説明テキスト4
    private GameObject m_explainText4 = null;

    // チュートリアルキャプチャーキャンバス2
    private GameObject m_tutorialCaptureCanvas2 = null;
    // 吹き出し2
    private GameObject m_captureFrame2 = null;
    // キャプチャーの説明テキスト1
    private GameObject m_explainCaptureText1 = null;
    // キャプチャーの説明テキスト2
    private GameObject m_explainCaptureText2 = null;
    // キャプチャーの実践テキスト
    private GameObject m_practiceCaptureText = null;

    // 現在のテキスト
    private Text m_text = null;
    // 現在の説明テキスト数
    private int m_curentNum = 0;

    // ドラムマネージャー
    private DrumManager_Script m_drumManager = null;

    private GameObject m_distanceGrabHandLeft = null;
    private GameObject m_distanceGrabHandRight = null;
    private GameObject m_stickLeft = null;
    private GameObject m_stickRight = null;

    // フェードUI
    private PanelUI_Fade_Script m_fadePanel = null;

    private float m_frameCount = 0.0f;

    // マテリアル
    [SerializeField]
    private Material[] m_stickMaterials = null;
    private Renderer m_leftStickRender = null;
    private Renderer m_rightStickRender = null;
    [SerializeField]
    private Mesh[] m_handMesh = null;

    // ヒットパターンキャンバス
    private GameObject m_hitPatternCanvas = null;

    // Start is called before the first frame update
    void Start()
    {
        this.m_nowMove = null;
        this.m_nextMove = null;

        this.m_isSetting = false;

        this.m_attackSpan = 0.0f;
        this.SetTarget();

        m_distanceGrabHandLeft = GameObject.Find("DistanceGrabHandLeft");
        m_distanceGrabHandRight = GameObject.Find("DistanceGrabHandRight");
        m_stickLeft = m_distanceGrabHandLeft.transform.Find("StickLeft").gameObject;
        m_stickRight = m_distanceGrabHandRight.transform.Find("StickRight").gameObject;

        m_leftStickRender = m_stickLeft.GetComponent<Renderer>();
        m_rightStickRender = m_stickRight.GetComponent<Renderer>();

        // モデルをコントローラーに変える
        m_distanceGrabHandLeft.GetComponent<CapsuleCollider>().enabled = false;
        m_distanceGrabHandRight.GetComponent<CapsuleCollider>().enabled = false;
        m_stickLeft.GetComponent<MeshFilter>().sharedMesh = m_handMesh[1];
        m_stickRight.GetComponent<MeshFilter>().sharedMesh = m_handMesh[2];
        m_leftStickRender.sharedMaterial = m_stickMaterials[1];
        m_rightStickRender.sharedMaterial = m_stickMaterials[1];

        if (SceneManager.GetActiveScene().name == "TutorialBattleScene")
        {
            m_tutorialCanvas = GameObject.Find("TutorialCanvas");
            m_battleFrame1 = m_tutorialCanvas.transform.Find("BattleFrame1").gameObject;
            m_explainBattleSequenceText = m_tutorialCanvas.transform.Find("ExplainBattleSequenceText").gameObject;
            m_explainHPText = m_tutorialCanvas.transform.Find("ExplainHPText").gameObject;
            m_explainAttackDrumText = m_tutorialCanvas.transform.Find("ExplainAttackDrumText").gameObject;
            m_rightArrowText = m_tutorialCanvas.transform.Find("RightArrowText").gameObject;
            m_explainBattleSystemText = m_tutorialCanvas.transform.Find("ExplainBattleSystemText").gameObject;
            m_explainBattleSystemText2 = m_tutorialCanvas.transform.Find("ExplainBattleSystemText2").gameObject;
            m_succesAttackText = m_tutorialCanvas.transform.Find("SuccesAttackText").gameObject;
            m_enemyKillText = m_tutorialCanvas.transform.Find("EnemyKillText").gameObject;
            m_tutorialEndText = m_tutorialCanvas.transform.Find("TutorialEndText").gameObject;
            m_tutorialEndText2 = m_tutorialCanvas.transform.Find("TutorialEndText2").gameObject;

            m_inHitCheckImage = m_explainBattleSystemText2.transform.Find("InHitCheckImage").gameObject;
            m_outHitCheckImage = m_explainBattleSystemText2.transform.Find("OutHitCheckImage").gameObject;
            m_doubleInHitCheckImage = m_explainBattleSystemText2.transform.Find("DoubleInHitCheckImage").gameObject;
            m_doubleOutHitCheckImage = m_explainBattleSystemText2.transform.Find("DoubleOutHitCheckImage").gameObject;

            m_tutorialSwitchCanvas = GameObject.Find("TutorialSwitchCanvas");
            m_battleFrame2 = m_tutorialSwitchCanvas.transform.Find("BattleFrame2").gameObject;
            m_explainSwitchDrumText = m_tutorialSwitchCanvas.transform.Find("ExplainSwitchDrumText").gameObject;
            m_explainSwitchDrumText2 = m_tutorialSwitchCanvas.transform.Find("ExplainSwitchDrumText2").gameObject;
            m_leftArrowText = m_tutorialSwitchCanvas.transform.Find("LeftArrowText").gameObject;

            m_tutorialCaptureCanvas = GameObject.Find("TutorialCaptureCanvas");
            m_battleFrame3 = m_tutorialCaptureCanvas.transform.Find("BattleFrame3").gameObject;
            m_explainCaptureDrumText = m_tutorialCaptureCanvas.transform.Find("ExplainCaptureDrumText").gameObject;
            m_rightArrowText2 = m_tutorialCaptureCanvas.transform.Find("RightArrowText2").gameObject;

            m_tutorialAbilityCanvas = GameObject.Find("TutorialAbilityCanvas");
            m_battleFrame4 = m_tutorialAbilityCanvas.transform.Find("BattleFrame4").gameObject;
            m_explainAbilityText = m_tutorialAbilityCanvas.transform.Find("ExplainAbilityText").gameObject;
            m_explainAbilityText2 = m_tutorialAbilityCanvas.transform.Find("ExplainAbilityText2").gameObject;
            m_explainAbilityText3 = m_tutorialAbilityCanvas.transform.Find("ExplainAbilityText3").gameObject;
            m_explainAbilityText4 = m_tutorialAbilityCanvas.transform.Find("ExplainAbilityText4").gameObject;

            m_tutorialMusicScoreCanvas = GameObject.Find("TutorialMusicScoreCanvas");
            m_battleFrame5 = m_tutorialMusicScoreCanvas.transform.Find("BattleFrame5").gameObject;
            m_explainTimerText = m_tutorialMusicScoreCanvas.transform.Find("ExplainTimerText").gameObject;
            m_explainNotesResetText = m_tutorialMusicScoreCanvas.transform.Find("ExplainNotesResetText").gameObject;

            m_textArray = new GameObject[] { m_explainBattleSequenceText, m_explainHPText, m_explainAttackDrumText, m_rightArrowText, m_explainSwitchDrumText, m_explainSwitchDrumText2, m_leftArrowText, m_explainCaptureDrumText, m_rightArrowText2, m_explainBattleSystemText, m_explainBattleSystemText2, m_explainAbilityText, m_explainAbilityText2, m_explainAbilityText3, m_explainAbilityText4, m_explainTimerText, m_explainNotesResetText, m_succesAttackText, m_enemyKillText, m_tutorialEndText, m_tutorialEndText2 };

            m_text = m_textArray[0].GetComponent<Text>();

            m_hitPatternCanvas = GameObject.Find("HitPatternCanvas");
        }
        else if (SceneManager.GetActiveScene().name == "TutorialCaptureScene")
        {
            m_tutorialExplainCanvas = GameObject.Find("TutorialExplainCanvas");
            m_captureFrame1 = m_tutorialExplainCanvas.transform.Find("CaptureFrame1").gameObject;
            m_explainText1 = m_tutorialExplainCanvas.transform.Find("ExplainText1").gameObject;
            m_explainText2 = m_tutorialExplainCanvas.transform.Find("ExplainText2").gameObject;
            m_explainText3 = m_tutorialExplainCanvas.transform.Find("ExplainText3").gameObject;
            m_leftArrowText3 = m_tutorialExplainCanvas.transform.Find("LeftArrowText3").gameObject;
            m_explainText4 = m_tutorialExplainCanvas.transform.Find("ExplainText4").gameObject;

            m_tutorialCaptureCanvas2 = GameObject.Find("TutorialCaptureCanvas2");
            m_captureFrame2 = m_tutorialCaptureCanvas2.transform.Find("CaptureFrame2").gameObject;
            m_explainCaptureText1 = m_tutorialCaptureCanvas2.transform.Find("ExplainCaptureText1").gameObject;
            m_explainCaptureText2 = m_tutorialCaptureCanvas2.transform.Find("ExplainCaptureText2").gameObject;
            m_practiceCaptureText = m_tutorialCaptureCanvas2.transform.Find("PracticeCaptureText").gameObject;

            m_textArray = new GameObject[] { m_explainText1, m_explainText2, m_explainText3, m_leftArrowText3, m_explainCaptureText1, m_explainCaptureText2, m_practiceCaptureText, m_explainText4 };

            m_text = m_textArray[0].GetComponent<Text>();

            CreatureList_Script.Get.List.DataList[0].drumonName = "";
        }

        m_drumManager = GameObject.Find("DrumManager").GetComponent<DrumManager_Script>();
        m_fadePanel = GameObject.Find("FadePanel").GetComponent<PanelUI_Fade_Script>();

        m_fadePanel.IsFadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isSetting)
        {
            this.m_attackSpan -= Time.deltaTime;

            if (SceneManager.GetActiveScene().name != "TutorialCaptureScene")
            {
                if (m_drumonExecuteFlag == true)
                {
                    if (this.JudgeResult()) return;
                    this.m_playerCreature.Execute();
                }
                else
                {
                    this.m_playerCreature.Rate = 0;
                }
            }

            if (m_enemyExecuteFlag == true)
            {
                this.m_enemyCreature.Execute();
            }
            
            if (this.m_playerCreature.AtkFlag) SetActive(this.m_playerCreature);
            if (this.m_enemyCreature.AtkFlag) SetActive(this.m_enemyCreature);
            if (this.m_nowMove != null && this.m_attackSpan <= 0.0f) this.Action();
        }

        if (SceneManager.GetActiveScene().name == "TutorialBattleScene")
        {
            if (m_tutorialModeFlag == false)
            {
                // 文字を小さくする
                m_text.fontSize -= 10;
                foreach (Transform childTransform in m_text.transform)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }

            // ボタンが押されたら
            if (OVRInput.GetDown(OVRInput.RawButton.A) && m_practiceModeFlag == false)
            {
                m_tutorialModeFlag = false;
            }

            // 文字が消えたら
            if (m_text.fontSize <= 0)
            {
                // 次のテキストの表示
                NextText();
            }

            if (m_curentNum == 3)
            {
                m_battleFrame1.gameObject.SetActive(false);
            }
            else if (m_curentNum == 4)
            {
                m_battleFrame2.gameObject.SetActive(true);
            }
            else if (m_curentNum == 6)
            {
                m_battleFrame2.gameObject.SetActive(false);
            }
            else if (m_curentNum == 7)
            {
                m_battleFrame3.gameObject.SetActive(true);
            }
            else if (m_curentNum == 8)
            {
                m_battleFrame3.gameObject.SetActive(false);
            }
            else if (m_curentNum == 9)
            {
                m_battleFrame1.gameObject.SetActive(true);
            }
            else if (m_curentNum == 10)
            {
                m_practiceModeFlag = true;
            }
            else if (m_curentNum == 11)
            {
                m_battleFrame1.gameObject.SetActive(false);
                m_battleFrame4.gameObject.SetActive(true);
            }
            else if (m_curentNum == 15)
            {
                m_battleFrame4.gameObject.SetActive(false);
                m_battleFrame5.gameObject.SetActive(true);
            }
            else if (m_curentNum == 16)
            {
                m_practiceModeFlag = true;

                foreach (Transform childTransform in m_hitPatternCanvas.transform)
                {
                    childTransform.gameObject.SetActive(true);
                }
            }
            else if (m_curentNum == 17 && m_text.gameObject.activeInHierarchy == true)
            {
                m_practiceModeFlag = true;

                m_frameCount++;

                m_battleFrame5.gameObject.SetActive(false);
                m_battleFrame1.gameObject.SetActive(true);

                if (m_frameCount >= 240.0f)
                {
                    m_text.gameObject.SetActive(false);
                    m_frameCount = 0.0f;

                    m_battleFrame1.gameObject.SetActive(false);
                }
            }
            else if (m_curentNum == 18)
            {
                m_practiceModeFlag = true;

                m_frameCount++;

                m_battleFrame1.gameObject.SetActive(true);

                if (m_frameCount >= 300.0f)
                {
                    m_tutorialModeFlag = false;
                    m_frameCount = 0.0f;
                }
            }
            else if (m_curentNum == 19)
            {
                m_practiceModeFlag = true;

                m_frameCount++;

                if (m_frameCount >= 360.0f)
                {
                    m_tutorialModeFlag = false;
                    m_frameCount = 0.0f;
                }
            }
            else if (m_curentNum == 20)
            {
                m_practiceModeFlag = true;
                StartCoroutine(SceneChengeStop());
            }

            // 叩けたらチェックを出す
            if (m_explainBattleSystemText2.activeInHierarchy == true)
            {
                if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.IN_HIT) == true || Input.GetKeyDown(KeyCode.V))
                {
                    m_inHitCheckImage.SetActive(true);
                }
                if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.OUT_HIT) == true || Input.GetKeyDown(KeyCode.B))
                {
                    m_outHitCheckImage.SetActive(true);
                }
                if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.DOUBLE_IN_HIT) == true || Input.GetKeyDown(KeyCode.N))
                {
                    m_doubleInHitCheckImage.SetActive(true);
                }
                if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.DOUBLE_OUT_HIT) == true || Input.GetKeyDown(KeyCode.M))
                {
                    m_doubleOutHitCheckImage.SetActive(true);
                }
                // 全てのチェックが出たら
                if (m_inHitCheckImage.activeInHierarchy == true && m_outHitCheckImage.activeInHierarchy == true && m_doubleInHitCheckImage.activeInHierarchy == true && m_doubleOutHitCheckImage.activeInHierarchy == true)
                {
                    m_tutorialModeFlag = false;
                }
            }
            else
            {
                m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.OffFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.IN_HIT);
                m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.OffFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.OUT_HIT);
                m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.OffFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.DOUBLE_IN_HIT);
                m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.OffFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.DOUBLE_OUT_HIT);
            }

            if (m_explainNotesResetText.activeInHierarchy == true)
            {
                // 攻撃できるようになる
                m_drumonExecuteFlag = true;

                // 敵が攻撃を受けたら
                if (m_enemyCreature.HP < m_enemyCreature.GetData().maxHp)
                {
                    // 次のテキストの表示
                    //NextText();
                    m_tutorialModeFlag = false;

                    // 敵が攻撃するようになる
                    m_enemyExecuteFlag = true;
                }
            }

            if (m_enemyCreature.HP <= 0 && m_curentNum == 17)
            {
                // 次のテキストの表示
                NextText();
            }

            // モデル変更
            ChengeModel();
        }
        else if (SceneManager.GetActiveScene().name == "TutorialCaptureScene")
        {
            if (m_tutorialModeFlag == false)
            {
                // 文字を小さくする
                m_text.fontSize -= 10;
                foreach (Transform childTransform in m_text.transform)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }

            // ボタンが押されたら
            if (OVRInput.GetDown(OVRInput.RawButton.A) && m_practiceModeFlag == false)
            {
                m_tutorialModeFlag = false;
            }

            // 文字が消えたら
            if (m_text.fontSize <= 0)
            {
                // 次のテキストの表示
                NextText();
            }

            if (m_curentNum == 3)
            {
                m_captureFrame1.gameObject.SetActive(false);
            }
            else if (m_curentNum == 4)
            {
                m_captureFrame2.gameObject.SetActive(true);
            }
            else if(m_curentNum == 6)
            {
                m_practiceModeFlag = true;

                m_distanceGrabHandLeft.GetComponent<CapsuleCollider>().enabled = true;
                m_distanceGrabHandRight.GetComponent<CapsuleCollider>().enabled = true;
                m_stickLeft.GetComponent<MeshFilter>().sharedMesh = m_handMesh[0];
                m_stickRight.GetComponent<MeshFilter>().sharedMesh = m_handMesh[0];

                if (CreatureList_Script.Get.List.DataList[0].drumonName != "")
                {
                    // 次のテキストの表示
                    //NextText();
                    m_tutorialModeFlag = false;
                }
            }
            else if (m_curentNum == 7)
            {
                m_captureFrame1.gameObject.SetActive(true);
                m_captureFrame2.gameObject.SetActive(false);
                StartCoroutine(SceneChengeStop());
            }

            // モデル変更
            ChengeModel();
        }
    }

    private void SetTarget()
    {
        if (m_playerCreature && m_enemyCreature)
        {
            this.m_playerCreature.SetTarget(this.m_enemyCreature);
            this.m_enemyCreature.SetTarget(this.m_playerCreature);

            this.m_isSetting = true;
        }
    }

    public void SetActive(ICreature_Script creature)
    {
        if (this.m_nowMove == creature || this.m_nextMove == creature) return;

        if (this.m_nowMove == null) this.m_nowMove = creature;
        else this.m_nextMove = creature;
    }

    private void Action()
    {
        this.m_nowMove.Attack();
        if (m_nextMove != null)
        {
            this.m_nowMove = this.m_nextMove;
            this.m_nextMove = null;
        }
        else
        {
            this.m_nowMove = null;
        }

        this.m_attackSpan = 3.0f;
    }

    private bool JudgeResult()
    {
        if (!this.m_playerCreature)
        {
            this.m_isSetting = false;
            m_isFinish.SetValueAndForceNotify(true);
            return true;
        }
        else if (!this.m_enemyCreature)
        {
            this.m_isSetting = false;
            m_isFinish.SetValueAndForceNotify(true);
            return true;
        }

        return false;
    }

    // 次のテキストの表示
    private void NextText()
    {
        m_tutorialModeFlag = true;

        if (m_textArray.Length > m_curentNum + 1)
        {
            // 現在のテキストを非アクティブ化
            if (m_textArray[m_curentNum])
                m_textArray[m_curentNum].SetActive(false);
        }

        if (m_textArray.Length > m_curentNum + 1)
        {
            // カウントアップ
            m_curentNum++;
        }

        if (m_textArray.Length > m_curentNum)
        {
            if (m_textArray[m_curentNum] != null)
            {
                // 次のテキストをアクティブ化
                m_textArray[m_curentNum].SetActive(true);

                // 次のテキストに変える
                m_text = m_textArray[m_curentNum].GetComponent<Text>();

                m_practiceModeFlag = false;
            }
        }
    }

    private void ResetData(CreatureData data)
    {
        data = new CreatureData();
    }

    private IEnumerator SceneChengeStop()
    {
        yield return new WaitForSeconds(2.0f);
        m_fadePanel.IsFadeOut = true;
        yield return new WaitForSeconds(1.0f);

        if (SceneManager.GetActiveScene().name == "TutorialCaptureScene")
        {
            m_isFinish.SetValueAndForceNotify(true);
        }
        else if (SceneManager.GetActiveScene().name == "TutorialBattleScene")
        {
            m_isAllFinish.SetValueAndForceNotify(true);
        }

        yield return null;
    }

    // モデル変更
    private void ChengeModel()
    {
        if (m_practiceModeFlag == true)
        {
            // モデルをスティックに変える
            m_distanceGrabHandLeft.GetComponent<CapsuleCollider>().enabled = true;
            m_distanceGrabHandRight.GetComponent<CapsuleCollider>().enabled = true;
            m_stickLeft.GetComponent<MeshFilter>().sharedMesh = m_handMesh[0];
            m_stickRight.GetComponent<MeshFilter>().sharedMesh = m_handMesh[0];
            m_leftStickRender.sharedMaterial = m_stickMaterials[0];
            m_rightStickRender.sharedMaterial = m_stickMaterials[0];
        }
        else
        {
            // モデルをコントローラーに変える
            m_distanceGrabHandLeft.GetComponent<CapsuleCollider>().enabled = false;
            m_distanceGrabHandRight.GetComponent<CapsuleCollider>().enabled = false;
            m_stickLeft.GetComponent<MeshFilter>().sharedMesh = m_handMesh[1];
            m_stickRight.GetComponent<MeshFilter>().sharedMesh = m_handMesh[2];
            m_leftStickRender.sharedMaterial = m_stickMaterials[1];
            m_rightStickRender.sharedMaterial = m_stickMaterials[1];
        }
    }
}
