///
///     FileName =
///
///
///
///
///
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackRecipeManeger_Script : MonoBehaviour
{
    enum Data_Type
    {
        NAME,


    }


    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

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
        for (int i = 0; i < csvDatas.Count; i++)
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


}
