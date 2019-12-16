using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostTextUI_Script : MonoBehaviour
{
    private Text m_textUI;

    private const string TEMP_TEXTNAME = "Cost : ";
    private const string TEMP_TEXTCOSTMAX = " / 10";

    private float m_nowCost = 0;
    public float NowCost
    {
        set { m_nowCost = value; }
        get { return m_nowCost; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Text>() != null)
        {
            m_textUI = this.GetComponent<Text>();
        }
        m_textUI.text = TEMP_TEXTNAME + Mathf.Floor(m_nowCost) + TEMP_TEXTCOSTMAX;
    }

    // Update is called once per frame
    void Update()
    {
        m_textUI.text = TEMP_TEXTNAME + Mathf.Floor(m_nowCost) + TEMP_TEXTCOSTMAX;
    }
}
