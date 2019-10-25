//
//      FileName @ NotesManeger_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 18 (Friday)   
//
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class NotesManager_Script : MonoBehaviour
{
    // 子のノーツプレハブから数字を保持する変数
    private string m_childNumSequence;

    // 生成された子の名前の数字列だけを取得する関数
    public int SearchInstanceNotes()
    {
        m_childNumSequence = "00";
        // 子を全て検索
        foreach (Transform child in transform)
        {
            // 生成された子の順で名前の数字の部分だけを文字列に代入
            m_childNumSequence += Regex.Replace(child.ToString(), @"[^0-9]", "");
        }
        int attackNum = System.Convert.ToInt32(m_childNumSequence);
        // 生成された子の順で並べられた数字の文字列を返す(1~4で最大8桁)
        return attackNum;
    }
}
