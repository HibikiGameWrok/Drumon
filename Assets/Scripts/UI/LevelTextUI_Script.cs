//
//      FileName @ LevelTextUI_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2020 / 01 / 14 (Wednesday)    
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextUI_Script : MonoBehaviour
{
    private int m_nowLevel;
    public int NowLevel
    {
        set { m_nowLevel = value; }
        get { return m_nowLevel; }
    }

    public const string TEMP_NAME = "Lv :";

    private Text m_outputText = null;

    // 開始関数
    void Start()
    {
        m_outputText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_outputText.text = TEMP_NAME + m_nowLevel.ToString();
    }
}
