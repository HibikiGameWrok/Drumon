using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMoveTimeUI_Script : MonoBehaviour
{
    // 子を保持する変数
    private Transform m_childSlider = null;
    // 子にアタッチしているSliderを保持する変数
    private Slider m_sliderCompnent = null;

    // タイマーオブジェクト（後で消す）
    private GameObject m_timerObject = null;
    // タイマーオブジェクトにアタッチしているスクリプト
    private TimeStandard_Script m_timeStandardScript = null;

    [SerializeField]
    private float m_maxValue = 10.0f;
    [SerializeField]
    private float m_minValue = 0.0f;
    [SerializeField]
    private float m_nowValue = 0.0f;

    [SerializeField]
    private float m_addtractValue;

    // Start is called before the first frame update
    void Start()
    {
        m_childSlider = this.transform.Find("Slider");
        m_sliderCompnent = m_childSlider.GetComponent<Slider>();

        /*  後で消す                                                                     */
        m_timerObject = GameObject.Find("Timer");
        m_timeStandardScript = m_timerObject.GetComponent<TimeStandard_Script>();
        /*                                                                      */

        m_sliderCompnent.minValue = m_minValue;
        m_sliderCompnent.maxValue = m_maxValue;
        m_sliderCompnent.value = m_nowValue;
    }

    // Update is called once per frame
    void Update()
    {
        /*   後で消す                                       */
        m_sliderCompnent.value = m_timeStandardScript.NowTimer;
        /*                                                  */
    }

}
