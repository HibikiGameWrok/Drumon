using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class CSVDataHolder 
{
    // CSVの中身を入れるリスト;
    private List<string[]> m_csvDatas = new List<string[]>();
    public List<string[]> CSVDatas
    {
        get { return m_csvDatas; }
    }


    // CSVデータを読み込み変数に保管する
    // 引数 : string型 クリーチャーの名前
    public void CSVLoadFile(string creatureName)
    {
        if(creatureName == null)
        {
            return;
        }

        // Resouces下のCSV読み込み
        TextAsset csvFile = Resources.Load("CSV/" + Regex.Replace(creatureName, @"[^a-z,A-Z]", "") + "CSV") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            m_csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
    }
}
