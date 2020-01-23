﻿//
//      FileName @ StatusMenuUI_Script.cs
//
//      Creater  @ Hibiki Yoshiyasu
//
//      Day      @ 2019 / 10 / 16 (Wednesday)    
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusMenuUI_Script : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_drumonDataUI = null;

    CreatureList_Script m_creatureList = null;

    void Awake()
    {
        if (CreatureList_Script.Get != null)
        {
            m_creatureList = CreatureList_Script.Get;
            SetUpUIData();
        }
        BattleManager_Script.Get.EnemyCreature.GetData().hp.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (CreatureList_Script.Get != null)
        {
            m_creatureList = CreatureList_Script.Get;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "BattleScene")
        {
            if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                // 子のUIを非表示
                foreach (Transform child in this.transform)
                {
                    if (child.gameObject.activeSelf == true)
                    {
                        child.gameObject.SetActive(false);
                    }
                    else if (child.gameObject.activeSelf == false)
                    {
                        child.gameObject.SetActive(true);
                        SetUpUIData();
                    }
                }
            }
        }
    }

    void SetUpUIData()
    {
        m_creatureList = CreatureList_Script.Get;

        if (m_creatureList != null)
        {
            for (int i = 0; i < m_drumonDataUI.Length; i++)
            {
                // 子のUIを非表示
                foreach (Transform child in m_drumonDataUI[i].transform)
                {
                    if (i == 3)
                    {
                        switch (child.name)
                        {
                            case "Name":
                                Text drumonName = child.GetComponent<Text>();
                                drumonName.text = BattleManager_Script.Get.EnemyCreature.GetData().drumonName;
                                break;
                            case "Level":
                                Text levelText = child.GetComponent<Text>();
                                levelText.text = "Lv :" + BattleManager_Script.Get.EnemyCreature.GetData().level;
                                break;
                            case "HPGauge":
                                Slider hpSlider = child.GetChild(0).GetComponent<Slider>();
                                hpSlider.maxValue = BattleManager_Script.Get.EnemyCreature.GetData().maxHp;
                                hpSlider.value = BattleManager_Script.Get.EnemyCreature.GetData().hp;
                                break;
                            default:
                                break;
                        }
                    }
                    if (i < 3)
                    {
                        switch (child.name)
                        {
                            case "Name":
                                Text drumonName = child.GetComponent<Text>();
                                drumonName.text = m_creatureList.List.DataList[i].drumonName;
                                break;
                            case "Level":
                                Text levelText = child.GetComponent<Text>();
                                levelText.text = "Lv :" + m_creatureList.List.DataList[i].level;
                                break;
                            case "HPGauge":
                                Slider hpSlider = child.GetChild(0).GetComponent<Slider>();
                                hpSlider.maxValue = m_creatureList.List.DataList[i].maxHp;
                                hpSlider.value = m_creatureList.List.DataList[i].hp;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
