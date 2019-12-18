using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

public class CostUI_Script : MonoBehaviour
{
    // 最大値
    [SerializeField]
    private float m_maxValue = 10.0f;
    // 現在値
    [SerializeField]
    private float m_nowValue = 0.0f;
    public float NowCostValue
    {
        get { return m_nowValue; }
        set { m_nowValue = value; }
    }
    // 最小値
    private float m_minValue = 0.0f;
   
    // 回復可能フラグ
    private bool m_recoveryFlag = false;
    public bool RecoveryFlag
    {
        set { m_recoveryFlag = value; }
        get { return m_recoveryFlag; }
    }


    // 子を保持する変数
    private Transform m_childSlider = null;
    // 子にアタッチしているSliderを保持する変数
    private Slider m_sliderCompnent = null;

    // コストテキストオブジェクト
    private Transform m_costTextUI = null;
    private CostTextUI_Script m_costText = null;

    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.Find("Slider") != null)
        {
            m_childSlider = this.transform.Find("Slider");
            m_sliderCompnent = m_childSlider.GetComponent<Slider>();
        }
        if (this.transform.FindChild("CostTextUI") != null)
        {
            m_costTextUI = this.transform.FindChild("CostTextUI");
            m_costText = m_costTextUI.GetComponent<CostTextUI_Script>();
        }
        m_sliderCompnent.minValue = m_minValue;
        m_sliderCompnent.maxValue = m_maxValue;
        m_sliderCompnent.value = m_nowValue;
    }

    // Update is called once per frame
    void Update()
    {
        m_sliderCompnent.value = m_nowValue;
        m_costText.NowCost = m_nowValue;
        GageEnd();
        if (GageEnd() == true)
        {
            GageRecovery(10.0f);
        }
    }

    // ゲージを増やす処理
    private void GageRecovery(float waitTime)
    {
        if (m_nowValue < m_sliderCompnent.maxValue)
        {
            m_nowValue += Time.deltaTime / m_maxValue * waitTime;
        }
    }

    // コスト消費（毎フレーム処理は不可）
    public void CostDawn(float cost)
    {
        if (m_recoveryFlag != true)
        {
            if ((int)m_nowValue > 0)
            {
                // 絶対値で取得し計算する
                m_nowValue -= Mathf.Abs(cost);
            }
        }
    }

    private bool GageEnd()
    {
        if((int)m_nowValue <= 0)
        {
            m_recoveryFlag = true;
        }
        else
        if ((int)m_nowValue >= m_sliderCompnent.maxValue)
        {
            m_recoveryFlag = false;
        }

        return m_recoveryFlag;
    }
}
