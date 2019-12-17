///
///    AttaciRecipiUI_Text_Script.cs
///
///    味方ドラモンの技名を表示するTextオブジェクトを生成するファイル 
///
///     2019 / 11 / 26  (火曜日)
///
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AttackRecipiTextUI_Script : MonoBehaviour
{
    // 列のデータタイプ
    public enum Data_Column : int
    {
        ATK_NAME = 0,
        ATK_ELEMENT,
        ATK_NOTES,
        ATK_RATE,
        ATK_COST
    }
    [SerializeField]
    private Data_Column m_dataColumn;

    private string m_costText = null;

    // 設定するフォントのファイルパス
    private const string FILEPATH_FONT = "Fonts/DrumonFont-Regular";
    // 設定するフォントのサイズ
    private const int FONT_SIZE = 24;
    // 生成するオブジェクト名
    private const string INS_OBJECT_NAME = "AttackRecipeText";
    // Textがアタッチされたオブジェクトの生成数
    private const int MAX_COUNT_OBJECT = 5 + 1;

    // テキスト座標初期化定数
    private const float ZERO_TEXT_POS = 0.0f;

    // 生成するTextオブジェクト
    private GameObject[] m_textObject = new GameObject[MAX_COUNT_OBJECT];

    // テキスト表示範囲
    [SerializeField]
    private Vector2 m_textRect = new Vector2(330.0f, 30.0f);

    // テキストをずらす高さ
    [SerializeField]
    private  float m_textShiftHeight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }


    // レシピを変更
    public void ChangeRecipe(List<string[]> csvDatas)
    {

        // 元々出ていた子のテキストオブジェクトを削除
        foreach (Transform Child in this.transform)
        {
            Destroy(Child.gameObject);
        }

        // テキストオブジェクトを表示
        InstanceTextChildObject(csvDatas);
    }

    // 子に設定
    private void InstanceTextChildObject(List<string[]> csvDatas)
    {
        // 定数の数分オブジェクトをTextコンポーネントをアタッチして生成する
        for (int i = 1; i < MAX_COUNT_OBJECT; i++)
        {
            // 空のオブジェクトを生成
            m_textObject[i] = new GameObject(INS_OBJECT_NAME + i);
            // Textコンポーネントをアタッチ
            m_textObject[i].AddComponent<Text>();
            // 親子関係を自身に結ぶ
            m_textObject[i].transform.SetParent(this.transform);
            // Textオブジェクトのコンポーネント設定
            SettingTextProperty(i);
            // 表示するTextの内容をcsvのデータを参照
            m_textObject[i].GetComponent<Text>().text = SetText(i, m_dataColumn, csvDatas);
        }
    }

    private string SetText(int num, Data_Column dataColumn, List<string[]> csvDatas)
    {
        switch (dataColumn)
        {
            case Data_Column.ATK_NAME:
                return m_costText = csvDatas[num][(int)Data_Column.ATK_NAME];
            case Data_Column.ATK_ELEMENT:
                return m_costText = csvDatas[num][(int)Data_Column.ATK_ELEMENT]; 
            case Data_Column.ATK_NOTES:
                return "ノーツプレハブが生成するオブジェクトを使用してください";
            case Data_Column.ATK_RATE:
                return m_costText = csvDatas[num][(int)Data_Column.ATK_RATE];
            case Data_Column.ATK_COST:
                return m_costText = "Cost : " + csvDatas[num][(int)Data_Column.ATK_COST];
            default:
                break;
        }
        return "";
    }

    // Textオブジェクトのコンポーネント設定
    private void SettingTextProperty(int num)
    {
        // フォント設定
        m_textObject[num].GetComponent<Text>().font = Resources.Load<Font>(FILEPATH_FONT);
        // フォントサイズ
        m_textObject[num].GetComponent<Text>().fontSize = FONT_SIZE;
        // テキストの表示範囲
        m_textObject[num].GetComponent<Text>().rectTransform.sizeDelta = m_textRect;
        // テキストの揃え
        m_textObject[num].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        // テキストの表示座標(数によって-Y軸方向へずらす)
        m_textObject[num].transform.localPosition = new Vector3(ZERO_TEXT_POS, this.transform.localPosition.y - m_textShiftHeight * (num - 1), ZERO_TEXT_POS);
        // テキストの表示角度
        m_textObject[num].transform.rotation = this.transform.rotation;
        // テキストオブジェクトの大きさ
        m_textObject[num].transform.localScale = Vector3.one;
    }


}
