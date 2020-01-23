using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleDrum_Script : MonoBehaviour
{
    // 左スティック
    private StickLeft_Script m_leftStick;
    // 右スティック
    private StickRight_Script m_rightStick;

    // 選択カウント
    private int m_selectCount = 0;
    // 選択カウントのプロパティ
    public int SelectCount
    {
        get { return m_selectCount; }
    }

    // タイトルUIC
    private GameObject m_titleUIC = null;
    // ゲームプレイボタン
    private GameObject m_gamePlayButton = null;
    // チュートリアルボタン
    private GameObject m_tutorialButton = null;

    // 決定フラグ
    private bool m_decision = false;
    // 決定フラグのプロパティ
    public bool Decision
    {
        get { return m_decision; }
        set { m_decision = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_leftStick = GameObject.FindGameObjectWithTag("StickLeft").GetComponent<StickLeft_Script>();
        m_rightStick = GameObject.FindGameObjectWithTag("StickRight").GetComponent<StickRight_Script>();

        m_titleUIC = GameObject.Find("TitleCanvas");
        m_gamePlayButton = m_titleUIC.transform.Find("Button1").gameObject;
        m_tutorialButton = m_titleUIC.transform.Find("Button2").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 色変更
        ChengeColor();

        if (m_leftStick.TitleDrumHitFlag == true)
        {
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // 決定
                m_decision = true;

                // 内側を叩いた判定フラグを伏せる
                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }
            if (m_leftStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                if (m_selectCount > 0)
                {
                    m_selectCount--;
                }

                m_leftStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }

            m_leftStick.TitleDrumHitFlag = false;
        }
        else if (m_rightStick.TitleDrumHitFlag == true)
        {
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT) == true)
            {
                // 決定
                m_decision = true;

                // 内側を叩いた判定フラグを伏せる
                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.IN_HIT);
            }
            if (m_rightStick.HitPatternFlag.IsFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT) == true)
            {
                if (m_selectCount < 1)
                {
                    m_selectCount++;
                }

                m_rightStick.HitPatternFlag.OffFlag((uint)Stick_Script.HIT_PATTERN.OUT_HIT);
            }

            m_rightStick.TitleDrumHitFlag = false;
        }
    }

    // 色変更
    void ChengeColor()
    {
        if (m_selectCount == 0)
        {
            // 黄色
            m_gamePlayButton.GetComponent<Image>().color = new Color(250.0f, 255.0f, 80.0f, 255.0f);
            // 白色
            m_tutorialButton.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        }
        else if (m_selectCount == 1)
        {
            // 白色
            m_gamePlayButton.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            // 黄色
            m_tutorialButton.GetComponent<Image>().color = new Color(250.0f, 255.0f, 80.0f, 255.0f);
        }
    }
}
