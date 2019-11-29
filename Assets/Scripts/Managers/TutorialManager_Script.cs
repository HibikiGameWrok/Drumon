using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class TutorialManager_Script : SingletonBase_Script<TutorialManager_Script>
{
    [SerializeField]
    private PlayerCreature_Script m_playerCreature;

    public PlayerCreature_Script PlayerCreature
    {
        get { return m_playerCreature; }
    }

    [SerializeField]
    private EnemyCreature_Script m_enemyCreature;

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

    // タイムオブジェクトを保持
    private GameObject m_timer = null;
    // タイムScriptを取得
    private AccelerationTime_Script m_timeStandard_Script = null;
    
    // チュートリアルを表示中かどうかのフラグ
    private bool m_tutorialModeFlag = true;
    // 実践中かどうかのフラグ
    private bool m_practiceModeFlag = false;

    // チュートリアルキャンバス
    private GameObject m_tutorialCanvas;
    // テキストの配列
    private GameObject[] m_textArray;
    // バトルシーケンステキスト
    private GameObject m_explainBattleSequenceText;
    // ドラモン説明テキスト
    private GameObject m_explainDrumonText;
    // HP説明テキスト
    private GameObject m_explainHPText;
    // 攻撃ドラム説明テキスト
    private GameObject m_explainAttackDrumText;
    // 右矢印テキスト
    private GameObject m_rightArrowText;
    // バトルシステム説明テキスト
    private GameObject m_explainBattleSystemText;
    // バトルシステム説明テキスト2
    private GameObject m_explainBattleSystemText2;
    // 内側を叩いた時のチェック
    private GameObject m_inHitCheckImage;
    // 外側を叩いた時のチェック
    private GameObject m_outHitCheckImage;
    // 内側を同時に叩いた時のチェック
    private GameObject m_doubleInHitCheckImage;
    // 外側を同時に叩いた時のチェック
    private GameObject m_doubleOutHitCheckImage;

    // チュートリアルスイッチキャンバス
    private GameObject m_tutorialSwitchCanvas;
    // 選択ドラム説明テキスト
    private GameObject m_explainSwitchDrumText;
    // 左矢印テキスト
    private GameObject m_leftArrowText;

    // チュートリアルキャプチャーキャンバス
    private GameObject m_tutorialCaptureCanvas;
    // 捕獲ドラム説明テキスト
    private GameObject m_explainCaptureDrumText;
    // 右矢印テキスト
    private GameObject m_rightArrowText2;

    // チュートリアル技リストのキャンバス
    private GameObject m_tutorialAbilityCanvas;
    // 技リストの説明テキスト
    private GameObject m_explainAbilityText;
    // 技リストの説明テキスト2
    private GameObject m_explainAbilityText2;

    // チュートリアルミュージックスコアキャンバス
    private GameObject m_tutorialMusicScoreCanvas;
    // ノーツリセットの説明テキスト
    private GameObject m_explainNotesResetText;
    // タイマーの説明テキスト
    private GameObject m_explainTimerText;

    // 現在のテキスト
    private Text m_text;
    // 現在の説明テキスト数
    private int m_curentNum = 0;

    // ドラムマネージャー
    private DrumManager_Script m_drumManager = null;

    // Start is called before the first frame update
    void Start()
    {
        //this.m_playerCreature = null;
        //this.m_enemyCreature = null;
        this.m_nowMove = null;
        this.m_nextMove = null;

        this.m_isSetting = false;

        this.m_attackSpan = 0.0f;
        this.SetTarget();

        m_timer = GameObject.Find("Timer");
        m_timeStandard_Script = m_timer.GetComponent<AccelerationTime_Script>();

        m_tutorialCanvas = GameObject.Find("TutorialCanvas");
        m_explainBattleSequenceText = m_tutorialCanvas.transform.Find("ExplainBattleSequenceText").gameObject;
        m_explainDrumonText = m_tutorialCanvas.transform.Find("ExplainDrumonText").gameObject;
        m_explainHPText = m_tutorialCanvas.transform.Find("ExplainHPText").gameObject;
        m_explainAttackDrumText = m_tutorialCanvas.transform.Find("ExplainAttackDrumText").gameObject;
        m_rightArrowText = m_tutorialCanvas.transform.Find("RightArrowText").gameObject;
        m_explainBattleSystemText = m_tutorialCanvas.transform.Find("ExplainBattleSystemText").gameObject;
        m_explainBattleSystemText2 = m_tutorialCanvas.transform.Find("ExplainBattleSystemText2").gameObject;

        m_inHitCheckImage = m_explainBattleSystemText2.transform.Find("InHitCheckImage").gameObject;
        m_outHitCheckImage = m_explainBattleSystemText2.transform.Find("OutHitCheckImage").gameObject;
        m_doubleInHitCheckImage = m_explainBattleSystemText2.transform.Find("DoubleInHitCheckImage").gameObject;
        m_doubleOutHitCheckImage = m_explainBattleSystemText2.transform.Find("DoubleOutHitCheckImage").gameObject;

        m_tutorialSwitchCanvas = GameObject.Find("TutorialSwitchCanvas");
        m_explainSwitchDrumText = m_tutorialSwitchCanvas.transform.Find("ExplainSwitchDrumText").gameObject;
        m_leftArrowText = m_tutorialSwitchCanvas.transform.Find("LeftArrowText").gameObject;

        m_tutorialCaptureCanvas = GameObject.Find("TutorialCaptureCanvas");
        m_explainCaptureDrumText = m_tutorialCaptureCanvas.transform.Find("ExplainCaptureDrumText").gameObject;
        m_rightArrowText2 = m_tutorialCaptureCanvas.transform.Find("RightArrowText2").gameObject;

        m_tutorialAbilityCanvas = GameObject.Find("TutorialAbilityCanvas");
        m_explainAbilityText = m_tutorialAbilityCanvas.transform.Find("ExplainAbilityText").gameObject;
        m_explainAbilityText2 = m_tutorialAbilityCanvas.transform.Find("ExplainAbilityText2").gameObject;

        m_tutorialMusicScoreCanvas = GameObject.Find("TutorialMusicScoreCanvas");
        m_explainNotesResetText = m_tutorialMusicScoreCanvas.transform.Find("ExplainNotesResetText").gameObject;
        m_explainTimerText = m_tutorialMusicScoreCanvas.transform.Find("ExplainTimerText").gameObject;

        m_textArray = new GameObject[] { m_explainBattleSequenceText, m_explainDrumonText, m_explainHPText, m_explainAttackDrumText, m_rightArrowText, m_explainSwitchDrumText, m_leftArrowText, m_explainCaptureDrumText, m_rightArrowText2, m_explainBattleSystemText, m_explainBattleSystemText2, m_explainAbilityText, m_explainAbilityText2, m_explainNotesResetText, m_explainTimerText };

        m_text = m_textArray[0].GetComponent<Text>();

        m_drumManager = GameObject.Find("DrumManager").GetComponent<DrumManager_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isSetting)
        {
            this.m_attackSpan -= Time.deltaTime;

            if (this.JudgeResult()) return;
            this.m_playerCreature.Execute();
            this.m_enemyCreature.Execute();
            if (this.m_playerCreature.AtkFlag) SetActive(this.m_playerCreature);
            if (this.m_enemyCreature.AtkFlag) SetActive(this.m_enemyCreature);
            if (this.m_nowMove != null && this.m_attackSpan <= 0.0f) this.Action();
        }

        if (m_tutorialModeFlag == true)
        {
            m_timeStandard_Script.StopFlag = true;
        }
        else
        {
            // 文字を小さくする
            m_text.fontSize -= 10;
        }

        // ボタンが押されたら
        if (Input.GetKeyDown(KeyCode.L) && m_practiceModeFlag == false)
        {
            m_tutorialModeFlag = false;
        }

        // 文字が消えたら
        if (m_text.fontSize <= 0)
        {
            // 次のテキストの表示
            NextText();
        }

        if (m_curentNum == 10)
        {
            m_practiceModeFlag = true;
        }
        else if (m_curentNum == 14)
        {
            m_practiceModeFlag = true;
            m_timeStandard_Script.StopFlag = false;
        }

        // 叩けたらチェックを出す
        if (m_explainBattleSystemText2.activeInHierarchy == true)
        {
            if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.IN_HIT) == true || Input.GetKeyDown(KeyCode.V))
            {
                m_inHitCheckImage.SetActive(true);
            }
            else if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.OUT_HIT) == true || Input.GetKeyDown(KeyCode.B))
            {
                m_outHitCheckImage.SetActive(true);
            }
            else if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.DOUBLE_IN_HIT) == true || Input.GetKeyDown(KeyCode.N))
            {
                m_doubleInHitCheckImage.SetActive(true);
            }
            else if (m_drumManager.AttackDrum.GetComponent<AttackDrum_Script>().TutorialFlag.IsFlag((uint)AttackDrum_Script.TUTORIAL_HIT_PATTERN.DOUBLE_OUT_HIT) == true || Input.GetKeyDown(KeyCode.M))
            {
                m_doubleOutHitCheckImage.SetActive(true);
            }
            // 全てのチェックが出たら
            if (m_inHitCheckImage.activeInHierarchy == true && m_outHitCheckImage.activeInHierarchy == true && m_doubleInHitCheckImage.activeInHierarchy == true && m_doubleOutHitCheckImage.activeInHierarchy == true)
            {
                m_tutorialModeFlag = false;
            }
        }

        // 敵が攻撃を受けたら
        if (m_enemyCreature.HP < 100)
        {
            m_timeStandard_Script.StopFlag = true;

            // 次のテキストの表示
            NextText();
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    m_timeStandard_Script.StopFlag = false;
        //}
    }

    public void SetPlayerCreature(PlayerCreature_Script creature)
    {
        this.m_playerCreature = creature;
        if (this.m_enemyCreature) this.SetTarget();
    }

    public void SetEnemyCreature(EnemyCreature_Script creature)
    {
        this.m_enemyCreature = creature;
        if (this.m_playerCreature) this.SetTarget();
    }

    private void SetTarget()
    {
        this.m_playerCreature.SetTarget(this.m_enemyCreature);
        this.m_enemyCreature.SetTarget(this.m_playerCreature);

        this.m_isSetting = true;
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

        // 現在のテキストを非アクティブ化
        m_textArray[m_curentNum].SetActive(false);
        // カウントアップ
        m_curentNum++;

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
