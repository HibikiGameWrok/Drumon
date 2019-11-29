using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTimeUI_Script : MonoBehaviour
{
    // 子を保持する変数
    private Transform m_childSlider = null;
    // 子にアタッチしているSliderを保持する変数
    private Slider m_sliderCompnent = null;

    // タイマーオブジェクト（後で消す）
    private GameObject m_timerObject = null;
    // タイマーオブジェクトにアタッチしているスクリプト
    private AccelerationTime_Script m_timeStandardScript = null;

    [SerializeField]
    private float m_minValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.Find("Slider") != null)
        {
            m_childSlider = this.transform.Find("Slider");
            m_sliderCompnent = m_childSlider.GetComponent<Slider>();
        }
        if (GameObject.Find("Timer") != null)
        {
            m_timerObject = GameObject.Find("Timer");
            m_timeStandardScript = m_timerObject.GetComponent<AccelerationTime_Script>();
        }


        m_sliderCompnent.minValue = m_minValue;
        m_sliderCompnent.maxValue = m_timeStandardScript.MaxTimer;
        m_sliderCompnent.value = m_timeStandardScript.NowTimer;
    }

    // Update is called once per frame
    void Update()
    {
        m_sliderCompnent.maxValue = m_timeStandardScript.MaxTimer;
        m_sliderCompnent.value = m_timeStandardScript.NowTimer;
    }

}
