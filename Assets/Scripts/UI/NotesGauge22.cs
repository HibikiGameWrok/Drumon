using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesGage22 : MonoBehaviour
{
 　 private Slider m_notesSlider;

    // 譜面スピード
    private int m_gaugeTime = 0;

    // TimeStop
    private bool m_gaugeStopFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        m_notesSlider = GetComponent<Slider>();

        float maxPoint = 200.0f;
        float minPoint = 0.0f;
        float nowPoint = 0.0f;

        m_notesSlider.maxValue = maxPoint;
        m_notesSlider.minValue = minPoint;
        m_notesSlider.value = nowPoint;
    }

    // Update is called once per frame
    void Update()
    {
        // タイムが止まっているか
        if (m_gaugeStopFlag == false)
        {
            // タイムがMAXでない限り
            if (m_gaugeTime < m_notesSlider.maxValue)
            {
                // タイムを加算させる
                m_gaugeTime++;
            }
            else
            {
                // MAXになったら止める
                Debug.Log("MAXになったよ");
                m_gaugeStopFlag = true;
            }

            // UIに反映
            m_notesSlider.value = m_gaugeTime;
        }

        // デバッグ用ノーツを出す関数
        ProvisionalIncetansNote();

        // デバッグ用関数
        ProvisionalResetKey();
        //Debug.Log(m_gaugeTime);
    }


    private void InstanceNotes()
    {

    }

    // 仮のデバッグ用関数
    private void ProvisionalResetKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("押された");
            if (m_gaugeStopFlag == true)
            {
                // リセット
                m_notesSlider.value = m_gaugeTime = 0;
                m_gaugeStopFlag = false;
            }
        }
    }

    // 仮のデバッグ用関数　キーボードでノーツを出す
    private void ProvisionalIncetansNote()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("ノーツを出す");
            // 下記にノーツを出す処理

        }
    }
}
