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


    private bool[] m_levelFlag = { false };

    private int[] m_drumonLv = new int[3];
    public int[] drumonLv
    {
        set { m_drumonLv = value; }
    }

    private bool m_onewayFlag = false;
    public bool onewayFlag
    {
        get { return m_onewayFlag; }
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
        CheckLevelUP();
        for (int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            if (m_levelFlag[i] == true)
            {
                if (i < CreatureList_Script.Get.List.DataList.Length)
                {
                    // 出力するドラモンの名前
                    m_drumonNameText[i].text = CreatureList_Script.Get.List.DataList[i].drumonName;
                    // 出力するレベルアップ前
                    m_beforLevelUIText[i].text = m_drumonLv[i].ToString();
                    // 出力するレベルアップ後
                    m_afterLevelUIText[i].text = CreatureList_Script.Get.List.DataList[i].level.ToString();
                }
            }
        }
        m_onewayFlag = true;
    }

    private void CheckLevelUP()
    {
        for(int i = 0; i < CreatureList_Script.Get.List.DataList.Length; i++)
        {
            // レベルが上がっているかどうか
            if (CreatureList_Script.Get.List.DataList[i].level != m_drumonLv[i])
            {
                m_levelFlag[i] = true;
                m_activeObject[i].SetActive(true);
            }
            else
            {
                m_levelFlag[i] = false;
                m_activeObject[i].SetActive(false);
            }
        }
    }
}
