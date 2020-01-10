using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingEvent_Script : MonoBehaviour
{
    private GameObject m_canvasObject = null;

    private string m_textCSVName = "StaffRollText";


    private CSVDataHolder csvHolder = new CSVDataHolder();

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("StaffRollCanvas") != null)
        {
            m_canvasObject = GameObject.Find("StaffRollCanvas");
        }

        //CSVSetting(m_textCSVName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // CSVを設定(主にクリーチャーのデータが変更された時に呼ばれる)
    public void CSVSetting(string textCSVName)
    {
        // CSVの保管クラスに設定
        csvHolder.CSVLoadFile(textCSVName);
    }

    private IEnumerator EventProgress()
    {


        yield break;
    }
}
