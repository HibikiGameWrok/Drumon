using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 618

public class CretureHpTextUI_Script : MonoBehaviour
{
    // テンプレート
    [SerializeField]
    private string TEMP_TEXT = "/";
    
    // テキストコンポーネント
    private Text m_HPtext = null;

    // HP値をする為のオブジェクト
    [SerializeField]
    private GameObject m_HPSlider = null;
    // 値
    private HealProsperityUI_Script m_healProsperityUI_script= null;

    // Start is called before the first frame update
    void Start()
    {
        m_HPtext = this.GetComponent<Text>();
        m_healProsperityUI_script = m_HPSlider.GetComponent<HealProsperityUI_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        m_HPtext.text = m_healProsperityUI_script.NowPoint.ToString() + TEMP_TEXT + m_healProsperityUI_script.MaxPoint.ToString();
    }
}
