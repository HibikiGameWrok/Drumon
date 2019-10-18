using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackRecipeManeger : MonoBehaviour
{
    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    // Start is called before the first frame update
    void Start()
    {
        csvFile = Resources.Load("Excel/CreatureAttackCSV") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        Debug.Log(csvDatas[0][1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
