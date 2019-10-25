using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMoveTimeUI_Script : MonoBehaviour
{
    private Slider m_sliderCompnent;

    [SerializeField]
    private float m_maxValue;
    [SerializeField]
    private float m_minValue;
    [SerializeField]
    private float m_nowValue;

    [SerializeField]
    private float m_addtractValue;

    int timeflame = 0;
    int timeminutes = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_sliderCompnent = this.GetComponent<Slider>();
        m_sliderCompnent.minValue = m_minValue;
        m_sliderCompnent.maxValue = m_maxValue;
        m_sliderCompnent.value = m_nowValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeflame < 60)
        {
            timeflame++;
        }
        else
        {
            timeflame = 0;
        }

        if (m_nowValue < m_maxValue)
        {
            AddValueNow();
        }

        //else
        //{
        //    m_nowValue = m_minValue;
        //}
    }

    private void AddValueNow()
    {
        m_nowValue = m_nowValue + m_addtractValue;
        m_sliderCompnent.value = m_nowValue;
    }
}
