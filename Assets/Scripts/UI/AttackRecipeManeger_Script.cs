//
//      FileName @ AttackRecipeManeger_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16      
//
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackRecipeManeger_Script : MonoBehaviour
{
    // 列のデータタイプ
    enum Data_Column_Type
    {
        ATK_NAME,
        ATK_ELEMENT,
        ATK_NOTES,
        ATK_RATE,
    }

    // CSVファイル
    TextAsset csvFile;
    // CSVの中身を入れるリスト;
    List<string[]> csvDatas = new List<string[]>(); 

    void Start()
    {
        // Resouces下のCSV読み込み
        csvFile = Resources.Load("Excel/CreatureAttackCSV") as TextAsset; 
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
                Debug.Log(csvDatas[i][j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 前に出現しているクリーチャーがレシピに存在するか検索
    public void SetCreatureNameSearch()
    {

    }

}
