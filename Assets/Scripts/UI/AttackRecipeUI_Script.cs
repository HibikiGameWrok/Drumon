using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

public class AttackRecipeUI_Script : MonoBehaviour
{
    // 列のデータタイプ
    public enum Data_Column : int
    {
        ATK_NAME = 0,
        ATK_ELEMENT,
        ATK_NOTES,
        ATK_RATE,
    }

    // 設定するフォントのファイルパス
    private const string FILEPATH_FONT = "Fonts/DrumonFont-Regular";
    // 設定するフォントのサイズ
    private const int FONT_SIZE = 24;

    // 生成するオブジェクト名
    private const string INS_OBJECT_NAME = "AttackNameText";
    // Textがアタッチされたオブジェクトの生成数
    private const int MAX_COUNT_OBJECT = 5 + 1;

    // プレハブのファイルパス
    private const string FILEPATH_NOTES = "InsPrefab/RecipeNotes/NotePrefab";

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

    // レシピのノーツを保管する親オブジェクト
    private Transform []m_AbilitySheetObject = new Transform[5];

    // テキストを取得
    private GameObject []m_textObject = new GameObject[MAX_COUNT_OBJECT];

    // ノーツプレハブを保持
    private GameObject m_notesPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        haneiUI();
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
            if (m_textObject[i] == null)
            {
                m_textObject[i] = new GameObject(INS_OBJECT_NAME + i);
                m_textObject[i].AddComponent<Text>();
                m_textObject[i].transform.SetParent(m_canvas.transform);
            }
            SettingTextProperty(i);
            m_textObject[i].GetComponent<Text>().text = csvDatas[i][(int)Data_Column.ATK_NAME];
        }
    }

    private void SettingTextProperty(int num)
    {
        m_textObject[num].GetComponent<Text>().font = Resources.Load<Font>(FILEPATH_FONT);
        m_textObject[num].GetComponent<Text>().fontSize = FONT_SIZE;
        m_textObject[num].GetComponent<Text>().rectTransform.sizeDelta = new Vector2(330.0f, 30.0f);
        m_textObject[num].GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        m_textObject[num].transform.localPosition = new Vector3(0.0f,120.0f - 39.0f * num,0.0f);
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
        for (int i = 1; i < MAX_COUNT_OBJECT; i++)
        {
            //1文字ずつ列挙する
            for (int j = 0; j < csvDatas[i][(int)Data_Column.ATK_NOTES].Length; j++)
            {
                Vector3 notePos = new Vector3(m_AbilitySheetObject[i - 1].transform.position.x + 0.05f * j, m_AbilitySheetObject[i - 1].transform.position.y, m_AbilitySheetObject[i - 1].transform.position.z);

                int stringNotesNum = System.Convert.ToInt32(csvDatas[i][(int)Data_Column.ATK_NOTES].Substring(j, 1));

                //プレハブ生成
                m_notesPrefab = Instantiate(
                   SetLodeNotesPrefab(stringNotesNum),
                   notePos,
                   new Quaternion(0.0f, 0.0f, 0.0f, 0.0f),
                   m_AbilitySheetObject[i - 1].transform) as GameObject;
            }
        }
    }

    // 生成するノーツプレハブの番号を設定し返す関数
    private GameObject SetLodeNotesPrefab(int num)
    {
        // ResourcesファイルからPrefabデータを設定(末尾の番号によってデータが変わる)
        m_notesPrefab = (GameObject)Resources.Load(FILEPATH_NOTES + num);

        return m_notesPrefab;
    }

    public void haneiUI()
    {
        for (int i = 0; i < 5; i++)
        {
            if (m_AbilitySheetObject[i] != null)
            {
                m_notesPrefab = null;
                // 子のオブジェクトが生成されていないのであれば削除
                foreach (Transform n in m_AbilitySheetObject[i].transform)
                {
                    GameObject.Destroy(n.gameObject);
                }
            }
        }
        // アビリティシートのノーツ保持オブジェクトを取得
        for (int i = 0; i < 5; i++)
        {
            if (m_AbilitySheetObject[i] == null)
            {
                m_AbilitySheetObject[i] = this.transform.Find("Ability" + (i + 1));
            }
        }
        // 子のCanvasを取得
        if (m_canvas == null)
        {
            m_canvas = this.transform.GetChild(0);
        }

        // CSVファイルを前に出ているクリーチャーによって取得
        LoadCSVFile();
        SetChildTextObject();
        UIReflect();
    }
}
