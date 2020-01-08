///
///     TextUI_Change_Script.cs
///
///     テキストコンポーネントの文章を変更するScript
///
///     ヨシヤス　ヒビキ
///
///     2020 / 01 / 08 水曜日
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI_Change_Script : MonoBehaviour
{
    // 読み込むCSVファイル名
    [SerializeField]
    private string m_csvFileName = "ResultEventText";

    // 描画する文章
    private string m_drawtext = null;

    // 文章の行数
    private int m_rowCount = 1;

    // 文章の列
    private int m_columCount = 1;

    // テキストコンポーネントを保持する変数
    private Text m_text = null;

    // CSVを保持する変数
    private CSVDataHolder csvHolder = new CSVDataHolder();
    
    // 設定したCSVの中身を変数に保持
    private List<string[]> csvDatas;


    // Start is called before the first frame update
    void Start()
    {
        // Textコンポーネントを取得
        if(this.GetComponent<Text>() != null)
        {
            m_text = this.GetComponent<Text>();
        }
        // CSVを設定
        CSVSetting(m_csvFileName);
    }

    // Update is called once per frame
    void Update()
    {
        // CSVの指定された行の文章を反映
        m_text.text = csvDatas[m_rowCount][m_columCount];
    }

    // 次の行の文章にする 
    public void NextRowText()
    {
        m_rowCount++;
    }

    // 行数を指定
    public void AssignRowText(int row)
    {
        if (row > 0)
        {
            m_rowCount = row;
        }
        else if(m_rowCount > csvDatas.Count)
        {
            m_rowCount = 1;
        }
    }

    // CSVを設定(主にクリーチャーのデータが変更された時に呼ばれる)
    private void CSVSetting(string filename)
    {
        // CSVの保管クラスに設定
        csvHolder.CSVLoadFile(filename);
        csvDatas = csvHolder.CSVDatas;
    }
}
