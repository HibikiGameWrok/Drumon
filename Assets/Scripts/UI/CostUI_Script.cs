///
///     CostUI_Script.cs
///
///     コストゲージUIの制御スクリプト
///
///     ヨシヤス　ヒビキ
///
///     2020 / 01 / 07 火曜日
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

public class CostUI_Script : MonoBehaviour
{
    // 最大値
    [SerializeField]
    private float m_maxValue = 0.0f;
    // 現在値
    [SerializeField]
    private float m_nowValue = 0.0f;

    [SerializeField]
    private AttackRecipeManeger_Script m_attackRecipeManegerScrit = null;

    [SerializeField]
    private AttackAbilityNameUI_Script m_attackAbilityNameUIScrit = null;

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

    // クールタイム
    private float m_waitTime = 0.0f;
    public float WaitTime
    {
        set { m_waitTime = value; }
        get { return m_waitTime; }
    }

    // クールタイム
    private float m_waitSpeedUp = 0.0f;
    public float waitSpeedUp
    {
        set { m_waitSpeedUp = value; }
        get { return m_waitSpeedUp; }
    }

    // 子にアタッチしているSliderを保持する変数
    private Slider m_sliderCompnent = null;

    // コストテキストオブジェクト
    private GameObject m_costTextUI = null;
    private CostTextUI_Script m_costText = null;

    // Start is called before the first frame update
    void Start()
    {

        m_sliderCompnent = this.GetComponent<Slider>();

        m_costTextUI = GameObject.Find("CostTextUI");
        m_costText = m_costTextUI.GetComponent<CostTextUI_Script>();

        m_sliderCompnent.minValue = m_minValue;
        m_sliderCompnent.maxValue = m_maxValue;
        m_sliderCompnent.value = m_nowValue;
    }

    // Update is called once per frame
    void Update()
    {
        m_sliderCompnent.value = m_nowValue;
        m_costText.NowCost = m_nowValue;
        int minValue = 10;

        if (m_recoveryFlag != true)
        {
            if (m_attackRecipeManegerScrit.csvDatas != null)
            {
                for (int i = 1; i < m_attackRecipeManegerScrit.csvDatas.Count; i++)
                {
                    if (minValue > int.Parse(m_attackRecipeManegerScrit.csvDatas[i][4]))
                    {
                        minValue = int.Parse(m_attackRecipeManegerScrit.csvDatas[i][4]);
                    }
                }
                if (m_nowValue < minValue) m_recoveryFlag = true;
            }
            else
            {
                if (m_nowValue <= 0) m_recoveryFlag = true;
            }
        }

        if (GageEnd() == true)
        {
            m_attackAbilityNameUIScrit.DrawStringAttackName("コスト回復中");
            GageRecovery(m_waitTime);
        }
    }

    // ゲージを増やす処理
    private void GageRecovery(float waitTime)
    {
        if (m_nowValue < m_sliderCompnent.maxValue)
        {
            m_nowValue += (Time.deltaTime / m_maxValue * waitTime) + m_waitSpeedUp;
            m_waitSpeedUp = 0;
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

    // ゲージが最終値に達した時
    public bool GageEnd()
    {
        // 現在が最低値に達したとき
        if((int)m_sliderCompnent.value <= 0)
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
