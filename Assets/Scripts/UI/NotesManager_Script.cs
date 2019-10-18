using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class NotesManager_Script : MonoBehaviour
{
    // 子のノーツプレハブから数字を保持する変数
    private string m_childNumSequence;

    public string SearchInstanceNotes()
    {
        foreach (Transform child in transform)
        {
            m_childNumSequence += Regex.Replace(child.ToString(), @"[^0-9]", "");
        }
        return m_childNumSequence;
    }
}
