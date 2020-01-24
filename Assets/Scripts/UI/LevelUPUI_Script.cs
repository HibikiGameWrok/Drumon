using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUPUI_Script : MonoBehaviour
{
    [SerializeField]
    private Text[] m_drumonNameText = null;

    [SerializeField]
    private Text[] m_beforLevelUIText = null;

    [SerializeField]
    private Text[] m_afterLevelUIText = null;

    [SerializeField]
    private GameObject[] m_activeObject = null;

    [SerializeField]
    private GameObject[] m_point = null;

    CreatureList_Script m_creatureList = null;

    private int m_noActiveNum = 0;
    public int noActiveNum
    {
        get { return m_noActiveNum; }
    }

    private bool[] m_levelFlag = { false };

    private int[] m_startDrumonLv = new int[3];
    public int[] startDrumonLv
    {
        set { m_startDrumonLv = value; }
    }

    private string[] m_startDrumonName = new string[3];
    public string[] startDrumonName
    {
        set { m_startDrumonName = value; }
    }

    private bool m_onewayFlag = false;
    public bool onewayFlag
    {
        get { return m_onewayFlag; }
    }

    private int m_blockID = 0;
    public int blockID
    {
        set { m_blockID = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (CreatureList_Script.Get != null)
        {
            m_creatureList = CreatureList_Script.Get;
        }
        m_levelFlag = new bool[CreatureList_Script.Get.List.DataList.Length];
    }

    // 出力するテキスト
    public void out_putText()
    {
        for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            if (m_levelFlag[i] == true)
            {
                if (i < CreatureList_Script.Get.List.DataList.Length)
                {
                    // 出力するドラモンの名前
                    m_drumonNameText[i].text = CreatureList_Script.Get.List.DataList[i].drumonName;
                    // 出力するレベルアップ前
                    m_beforLevelUIText[i].text = m_startDrumonLv[i].ToString();
                    // 出力するレベルアップ後
                    m_afterLevelUIText[i].text = CreatureList_Script.Get.List.DataList[i].level.ToString();
                }
            }
        }
        m_onewayFlag = true;
    }

    public void CheckLevelUP()
    {
        for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            if (m_blockID != i)
            {
                // レベルが上がっているかどうか
                if (CreatureList_Script.Get.List.DataList[i].level != m_startDrumonLv[i])
                {
                    m_levelFlag[i] = true;
                    m_activeObject[i].SetActive(true);
                }
                else
                {
                    m_levelFlag[i] = false;
                    m_activeObject[i].SetActive(false);
                    m_noActiveNum += 1;
                }
            }
            else
            {
                m_levelFlag[i] = false;
                m_activeObject[i].SetActive(false);
                m_noActiveNum += 1;
            }
        }
    }

    public void SetPoint()
    {
        int num = 0;
        if (m_noActiveNum < 3)
        {
            for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
            {
                // レベルが上がっているかどうか
                if (CreatureList_Script.Get.List.DataList[i].level != m_startDrumonLv[i] || m_blockID != i)
                {
                    if (m_noActiveNum == 2)
                    {
                        m_activeObject[i].transform.position = m_point[num].transform.position;
                    }
                    else if (m_noActiveNum == 1)
                    {
                        num = 1;
                        m_activeObject[i].transform.position = m_point[num].transform.position;
                        num++;
                    }
                }
            }
        }
        else
        {
            m_activeObject[0].transform.position = m_point[3].transform.position;
            m_activeObject[1].transform.position = m_point[4].transform.position;
            m_activeObject[2].transform.position = m_point[5].transform.position;
        }
    }
}
