using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class AttackRecipeUI_Script : MonoBehaviour
{
    // 設定するフォントのファイルパス
    private const string FILEPATH_FONT = "Fonts/DrumonFont-Regular";
    // 設定するフォントのサイズ
    private const int FONT_SIZE = 29;

    // 生成するオブジェクト名
    private const string INS_OBJECT_NAME = "AttackNameText";
    // Textがアタッチされたオブジェクトの生成数
    private const int MAX_COUNT_OBJECT = 5 + 1;

    // 取得するべき物
    // 前にでているクリーチャーの名前
    private string m_nowCreaterName = "none";

    // 前に出ているクリーチャーによって読み込むべきCSV
    // CSVファイル
    private TextAsset csvFile = null;
    // CSVの中身を入れるリスト;
    private List<string[]> csvDatas = new List<string[]>();

    // 子のオブジェクト
    private Transform m_canvas = null;

    // テキストを取得
    private GameObject []m_textObject = new GameObject[MAX_COUNT_OBJECT];

    // 攻撃の名前を保持
    string m_attackName = "none";

    // 攻撃のノーツ列を保持
    int m_attackNotesNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 子のCanvasを取得
        m_canvas = this.transform.GetChild(0);

        SetChildTextObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetChildTextObject()
    {
        // 定数の数分オブジェクトをTextをアタッチして生成する
        for (int i = 1; i < MAX_COUNT_OBJECT; i++)
        {
            m_textObject[i] = new GameObject(INS_OBJECT_NAME + i);
            m_textObject[i].AddComponent<Text>();
            m_textObject[i].transform.SetParent(m_canvas.transform);
            SettingTextProperty(i);
        }


        //テストでテキストに文字を入力
        for (int i = 1; i < MAX_COUNT_OBJECT; i++)
        {
            m_textObject[i].GetComponent<Text>().text = "ここに攻撃の名前が入る";
        }
    }

    private void SettingTextProperty(int num)
    {
        m_textObject[num].GetComponent<Text>().font = Resources.Load<Font>(FILEPATH_FONT);
        m_textObject[num].GetComponent<Text>().fontSize = FONT_SIZE;
        m_textObject[num].GetComponent<Text>().rectTransform.sizeDelta = new Vector2(330.0f, 30.0f);
        m_textObject[num].transform.localPosition = new Vector3(0.0f,250.0f - 75.0f * num,0.0f);
        m_textObject[num].transform.rotation = m_canvas.transform.rotation;
        m_textObject[num].transform.localScale = Vector3.one;
    }

    public void LoadCSVFile()
    {
        // 現在出ているクリチャーの名前を戦闘管理オブジェクトから取得
        m_nowCreaterName = Regex.Replace(BattleManager_Script.Get.PlayerCreature.Name, @"[^a-z,A-Z]", "");
        // 取得したクリーチャーと同じ名前のCSVファイルを読み込む
        csvFile = Resources.Load("CSV/" + m_nowCreaterName + "CSV") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
    }

    private void UIReflect()
    {

    }
}
