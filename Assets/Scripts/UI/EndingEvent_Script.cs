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



    // Start is called before the first frame update
    void Start()
    {
        CSVSetting(m_textCSVName);
        m_csvDatas = m_csvHolder.CSVDatas;
        StartCoroutine("EventProgress");
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
            yield return new WaitForSeconds(2.0f);
            // CubeプレハブをGameObject型で取得
            GameObject obj = (GameObject)Resources.Load("InsPrefab/StaffRollTextObject");

            Text stafftext = obj.transform.GetChild(0).GetComponent<Text>();
            stafftext.text = m_csvDatas[i][1];
            if(m_csvDatas[i][2] == "1")
            {
                stafftext.color = new Color(0.2f,0.4f,0.2f,1); 
            }
            else
            {
                stafftext.color = Color.black;
            }

            // Cubeプレハブを元に、インスタンスを生成
            Instantiate(obj, this.transform.position - new Vector3(0.0f, 2.0f,0.0f), Quaternion.identity,this.transform);


        }
    }
}
