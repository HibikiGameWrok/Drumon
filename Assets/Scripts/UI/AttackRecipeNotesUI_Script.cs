﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

public class AttackRecipeNotesUI_Script : MonoBehaviour
{
    // 列のデータタイプ
    public enum Data_Column : int
    {
        ATK_NAME = 0,
        ATK_ELEMENT,
        ATK_NOTES,
        ATK_RATE,
    }

    // プレハブのファイルパス
    private const string FILEPATH_NOTES = "InsPrefab/RecipeNotes/NotePrefab";

    // Textがアタッチされたオブジェクトの生成数
    private const int MAX_COUNT_OBJECT = 5 + 1;

    // 取得するべき物
    // 前にでているクリーチャーの名前
    private string m_nowCreaterName = "none";

    // 前に出ているクリーチャーによって読み込むべきCSV
    // CSVファイル
    private TextAsset m_csvFile = null;
    // CSVの中身を入れるリスト;
    private List<string[]> m_csvDatas = new List<string[]>();

    // レシピのノーツを保管する親オブジェクト
    private Transform []m_AbilitySheetObject = new Transform[5];

    // ノーツプレハブを保持
    private GameObject m_notesPrefab = null;

    // ノーツをずらす高さ
    [SerializeField]
    private float m_notesShiftHeight = 0.0f;

    // ノーツをずらす高さ
    [SerializeField]
    private float m_notesShiftWidth = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        LoadCSVFile(BattleManager_Script.Get.PlayerCreature.Name);
        InstanceRecipeNote();
        this.transform.localRotation = this.transform.parent.rotation;
    }


    // レシピを変更
    public void ChangeRecipe()
    {
        // 元々出ていた子のテキストオブジェクトを削除
        foreach (Transform Child in this.transform)
        {
            Destroy(Child.gameObject);
        }

        // CSVをロード
        LoadCSVFile(BattleManager_Script.Get.PlayerCreature.Name);

        // テキストオブジェクトを表示
        InstanceRecipeNote();
    }

    // CSVを読み込む
    private void LoadCSVFile(string creatureName)
    {
        // 現在出ているクリチャーの名前を戦闘管理オブジェクトから取得
        m_nowCreaterName = Regex.Replace(creatureName, @"[^a-z,A-Z]", "");
        // 取得したクリーチャーと同じ名前のCSVファイルを読み込む
        m_csvFile = Resources.Load("CSV/" + m_nowCreaterName + "CSV") as TextAsset;
        StringReader reader = new StringReader(m_csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            m_csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
    }

    // ノーツを生成
    private void InstanceRecipeNote()
    {
        // listの中身が入っていない場合は生成しない
        if (m_csvDatas != null && m_csvDatas.Count > 0)
        {
            this.transform.localRotation = Quaternion.Euler(Vector3.zero);
            for (int i = 1; i < MAX_COUNT_OBJECT; i++)
            {
                //1文字ずつ列挙する
                for (int j = 0; j < m_csvDatas[i][(int)Data_Column.ATK_NOTES].Length; j++)
                {
                    // 技のノーツ数列を１桁取得
                    int stringNotesNum = System.Convert.ToInt32(m_csvDatas[i][(int)Data_Column.ATK_NOTES].Substring(j, 1));

                    // ノーツの座標を設定(１つのノーツにつきX軸にずらす、１つの技名につきY軸をずらす)
                    Vector3 notePos = new Vector3(this.transform.position.x + m_notesShiftWidth * j, this.transform.position.y - m_notesShiftHeight * (i - 1), this.transform.position.z);

                    //プレハブ生成
                    m_notesPrefab = Instantiate(
                       SetLodeNotesPrefab(stringNotesNum),
                       notePos,
                       Quaternion.identity,
                       this.transform) as GameObject;
                }
            }
            m_csvDatas.Clear();
            this.transform.localRotation = this.transform.parent.rotation * m_notesPrefab.transform.rotation;
        }
    }

    // 生成するノーツプレハブの番号を設定し返す関数
    private GameObject SetLodeNotesPrefab(int num)
    {
        // ResourcesファイルからPrefabデータを設定(末尾の番号によってデータが変わる)
        m_notesPrefab = (GameObject)Resources.Load(FILEPATH_NOTES + num);

        return m_notesPrefab;
    }

}