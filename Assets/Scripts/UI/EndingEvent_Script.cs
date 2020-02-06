using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingEvent_Script : MonoBehaviour
{
    private string m_textCSVName = "StaffList";

    private CSVDataHolder m_csvHolder = new CSVDataHolder();
    // 設定したCSVの中身を変数に保持
    List<string[]> m_csvDatas = null;

    private bool m_finishFlag = false;
    public bool finishFlag
    {
        get { return m_finishFlag; }
    }

    private GameObject m_fadeObject = null;
    private PanelUI_Fade_Script m_fadeImage = null;

    // Start is called before the first frame update
    void Start()
    {
        CSVSetting(m_textCSVName);
        m_csvDatas = m_csvHolder.CSVDatas;
        StartCoroutine("EventProgress");

        m_fadeObject = GameObject.Find("FadePanel");
        m_fadeImage = m_fadeObject.GetComponent<PanelUI_Fade_Script>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // CSVを設定(主にクリーチャーのデータが変更された時に呼ばれる)
    public void CSVSetting(string textCSVName)
    {
        // CSVの保管クラスに設定
        m_csvHolder.CSVLoadFile(textCSVName);
    }

    private IEnumerator EventProgress()
    {
        for(int i = 1; i < m_csvDatas.Count; i++)
        {
            // 遅延
            yield return new WaitForSeconds(2.0f);

            // CubeプレハブをGameObject型で取得
            GameObject obj = (GameObject)Resources.Load("InsPrefab/StaffRollTextObject");
            // Textコンポーネント
            Text stafftext = obj.transform.GetChild(0).GetComponent<Text>();

            // 文章をcsvの内容に反映
            stafftext.text = m_csvDatas[i][1];
            // フォントサイズをcsvの内容に反映
            stafftext.fontSize = int.Parse(m_csvDatas[i][3]);
            if (m_csvDatas[i][2] == "1")
            {
                // 指定の文字の色を緑にする
                stafftext.color = new Color(0.2f,0.4f,0.2f,1); 
            }
            else
            {
                stafftext.color = Color.black;
            }

            // Cubeプレハブを元に、インスタンスを生成
            Instantiate(obj, this.transform.position - new Vector3(0.0f, 2.0f,0.0f), Quaternion.identity,this.transform);
        }
        yield return new WaitForSeconds(10.0f);
        m_fadeImage.IsFadeOut = true;
        m_finishFlag = true;
    }
}
