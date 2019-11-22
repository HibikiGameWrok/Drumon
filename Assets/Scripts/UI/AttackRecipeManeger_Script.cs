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
    }

    // CSVファイル
    private TextAsset csvFile = null;
    // CSVの中身を入れるリスト;
    private List<string[]> csvDatas = new List<string[]>();
    

    // クリーチャーについてのスクリプト保持変数
    private PlayerCreature_Script m_pCreature_Script = null;

    // シートを出す為の前に出ているクリーチャーの名前を保持する変数
    private string m_sheetCreatureName = null;

    // NotesManagerオブジェクトを取得
    private GameObject m_notesManager = null;

    // TestNotesInstanceスクリプトを取得
    private NotesInstance_Script m_notesInstance = null;

    void Start()
    {
        // ノーツ管理オブジェクトを取得
        m_notesManager = GameObject.Find("NotesManager");

        m_notesInstance = m_notesManager.GetComponent<NotesInstance_Script>();
        m_pCreature_Script = BattleManager_Script.Get.PlayerCreature;
    }

    public void CSVLoadFile(PlayerCreature_Script pCreature)
    {
        // 変数に保持
        m_sheetCreatureName = pCreature.Name;

        // Resouces下のCSV読み込み
        csvFile = Resources.Load("CSV/"+ Regex.Replace(m_sheetCreatureName, @"[^a-z,A-Z]", "") + "CSV") as TextAsset; 
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // デバッグ用中身を確認する処理
        for (int i = 1; i < csvDatas.Count; i++)
        {
            for (int j = 0; j < csvDatas[i].Length; j++)
            {
                //Debug.Log(csvDatas[i][j].ToString());
            }
        }
    }

    // 現在のノーツと攻撃する為のノーツが合っているか見比べる
    public void MatchAttackRecipe()
    {
        // マッチしたか比較する為のノーツレシピを保持する変数
        string mathcAttackNotes = "00";
        // マッチした場合にレートを一時的に保持する変数
        string matchRate = "00";

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
                    // 一致していたレシピが回復でなければ攻撃
                    if (csvDatas[i][(int)Data_Column.ATK_NAME] != "HEAL")
                    {
                        matchRate = csvDatas[i][(int)Data_Column.ATK_RATE];
                        m_pCreature_Script.Rate = System.Convert.ToInt32(matchRate);
                    }
                    else
                    {
                        m_pCreature_Script.Heal();
                    }
                }
            }
        }
    }
}
