using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUPUI_Script : MonoBehaviour
{
    private string TEMP_TEXT = "がレベルアップした";

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

    private int m_drumonnum = 0;
    public int DrumonNum
    {
        get { return m_drumonnum; }
        set { m_drumonnum = value; }
    }

    private bool[] m_levelFlag = { false };


    // Start is called before the first frame update
    void Start()
    {
        if (CreatureList_Script.Get == null)
        {
            m_creatureList = CreatureList_Script.Get;
        }
        m_levelFlag = new bool[CreatureList_Script.Get.List.DataList.Length];
        m_drumonnum = 0;
    }

    // 出力するテキスト
    public void out_putText()
    {
        CheckLevelUP();
        for (m_drumonnum = 0; m_drumonnum < CreatureList_Script.Get.List.DataList.Length; m_drumonnum++)
        {
            if (m_levelFlag[m_drumonnum] == true)
            {
                if (m_drumonnum < CreatureList_Script.Get.List.DataList.Length)
                {
                    // 出力するドラモンの名前
                    m_drumonNameText[m_drumonnum].text = CreatureList_Script.Get.List.DataList[m_drumonnum].drumonName + TEMP_TEXT;
                    // 出力するレベルアップ前
                    m_beforLevelUIText[m_drumonnum].text = m_creatureList.List.DataList[m_drumonnum].level.ToString();
                    // 出力するレベルアップ後
                    m_afterLevelUIText[m_drumonnum].text = CreatureList_Script.Get.List.DataList[m_drumonnum].level.ToString();
                }
            }
            else if(m_levelFlag[m_drumonnum] == false)
            {
                m_activeObject[m_drumonnum].SetActive(false);
            }
        }
    }

    private void CheckLevelUP()
    {
        for(int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            // レベルが上がっているかどうか
            if(CreatureList_Script.Get.List.DataList[i].level != m_creatureList.List.DataList[i].level)
            {
                m_levelFlag[i] = true;
            }
        }
    }
}
