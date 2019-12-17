//
//      FileName @ AttackRecipeManeger_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16 (Wednesday)     
//
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

public class AttackRecipeManeger_Script : SingletonBase_Script<AttackRecipeManeger_Script>
{
    // 列のデータタイプ
    public enum Data_Column : int
    {
        ATK_NAME = 0,
        ATK_ELEMENT,
        ATK_NOTES,
        ATK_RATE,
        ATK_COST
    }
    

    private CSVDataHolder csvHolder = new CSVDataHolder();
    [SerializeField]
    private AttackRecipeNotesUI_Script m_attackRecipeNotesUI = null;
    [SerializeField]
    private AttackRecipiTextUI_Script m_attackRecipeTextUI = null;
    [SerializeField]
    private AttackRecipiTextUI_Script m_attackRecipeTextCostUI = null;

    // クリーチャーについてのスクリプト保持変数
    private PlayerCreature_Script m_pCreature_Script = null;

    // NotesManagerオブジェクトを取得
    private GameObject m_notesManager = null;

    // TestNotesInstanceスクリプトを取得
    private NotesInstance_Script m_notesInstance = null;

    // 技の名前を表示するオブジェクト
    private GameObject m_abilityNameTextUI = null;
    private AttackAbilityNameUI_Script m_abilityNameUI_Script = null;

    private GameObject m_CostUI = null;
    private CostUI_Script m_costUI_Script = null;

    // 攻撃指示を完了したフラグ
    private bool m_attackCompFlag = false;
    public bool AttackCompFlag
    {
        set { m_attackCompFlag = value; }
        get { return m_attackCompFlag; }
    }


    void Start()
    {
        // ノーツ管理オブジェクトを取得
        m_notesManager = GameObject.Find("NotesManager");

        m_notesInstance = m_notesManager.GetComponent<NotesInstance_Script>();
        m_pCreature_Script = BattleManager_Script.Get.PlayerCreature;

        m_abilityNameTextUI = GameObject.Find("AbilityNameTextUI");
        m_abilityNameUI_Script = m_abilityNameTextUI.GetComponent<AttackAbilityNameUI_Script>();

        m_CostUI = GameObject.Find("RecipeUI");
        m_costUI_Script = m_CostUI.GetComponent<CostUI_Script>();
    }

    // CSVを設定
    public void CSVSetting(string creatureName)
    {
        // CSVの保管クラスに設定
        csvHolder.CSVLoadFile(creatureName);
        m_attackRecipeNotesUI.ChangeRecipe(csvHolder.CSVDatas);
        m_attackRecipeTextUI.ChangeRecipe(csvHolder.CSVDatas);
        m_attackRecipeTextCostUI.ChangeRecipe(csvHolder.CSVDatas);
    }

    // 現在のノーツと攻撃する為のノーツが合っているか見比べる
    public void MatchAttackRecipe(int nowCost)
    {
        // マッチしたか比較する為のノーツレシピを保持する変数
        string mathcAttackNotes = "00";
        // マッチした場合にレートを一時的に保持する変数
        string matchRate = "00";

        // 設定したCSVの中身を変数に保持
        List<string[]> csvDatas = csvHolder.CSVDatas;

        for (int i = 1; i < csvDatas.Count; i++)
        {
            // i = 行,Data_Column.ATK_NOTES = 列
            mathcAttackNotes = csvDatas[i][(int)Data_Column.ATK_NOTES];
            int attackNum = System.Convert.ToInt32(mathcAttackNotes);
            if (attackNum != 0)
            {
                // 生成されたノーツの番号とCSVのデータを比較
                if (m_notesInstance.SearchInstanceNotes() == attackNum)
                {
                    int nowAbility = System.Convert.ToInt32(csvDatas[i][(int)Data_Column.ATK_COST]);
                    if (nowCost >= nowAbility)
                    {
                        // 一致していたレシピが回復でなければ攻撃
                        if (m_notesInstance.SearchInstanceNotes() != 111111)
                        {
                            // 技の名前をクリーチャーに教える
                            m_pCreature_Script.AbiltyName = csvDatas[i][(int)Data_Column.ATK_NAME];

                            // 技のレートをクリーチャーに教える
                            matchRate = csvDatas[i][(int)Data_Column.ATK_RATE];
                            m_pCreature_Script.Rate = System.Convert.ToInt32(matchRate);
                        }
                        else if (m_notesInstance.SearchInstanceNotes() == 111111)
                        {
                            // 回復する
                            m_pCreature_Script.Heal();
                        }
                        // 技の名前を表示する
                        m_abilityNameUI_Script.DrawStringAttackName(csvDatas[i][(int)Data_Column.ATK_NAME]);
                    }
                    else
                    {
                        // 技の名前を表示する
                        m_abilityNameUI_Script.DrawStringAttackName("コストが足りません");
                    }
                    // UI の動作 //
                    // コスト消費
                    m_costUI_Script.CostDawn(System.Convert.ToInt32(csvDatas[i][(int)Data_Column.ATK_COST]));
                    // 攻撃指示が完了したフラグ
                    m_attackCompFlag = true;
                }
            }
        }
    }

}
