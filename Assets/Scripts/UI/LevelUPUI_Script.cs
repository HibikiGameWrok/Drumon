using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUPUI_Script : MonoBehaviour
{
    [SerializeField]
    private Text m_drumonNameText = null;

    [SerializeField]
    private Text m_beforLevelUIText = null;

    [SerializeField]
    private Text m_afterLevelUIText = null;

    CreatureList_Script m_creatureList = null;

    private int m_num = 0;

    void Awake()
    {
        if (CreatureList_Script.Get != null)
        {
            m_creatureList = CreatureList_Script.Get;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (CreatureList_Script.Get != null)
        {
            m_creatureList = CreatureList_Script.Get;
        }
    }

    public void out_putText()
    {
        // 出力するドラモンの名前
        m_drumonNameText.text = m_creatureList.List.DataList[m_num].name;
        // 出力するレベルアップ前
        m_beforLevelUIText.text = m_creatureList.List.DataList[m_num].level.ToString();
        // 
    }
}
