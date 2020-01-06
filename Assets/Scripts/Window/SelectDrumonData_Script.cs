﻿using UnityEngine;
using UnityEditor;

public class SelectDrumonData_Script : MonoBehaviour
{
    [MenuItem("DrumonData/Garfish", false, 1)]
    static void CreateGarfish()
    {
        CreateData("Garfish");
    }

    [MenuItem("DrumonData/Lantern", false, 1)]
    static void CreateLantern()
    {
        CreateData("Lantern");
    }

    [MenuItem("DrumonData/Kitsune", false, 1)]
    static void CreateKitsune()
    {
        CreateData("Kitsune");
    }

    [MenuItem("DrumonData/ChurchGrim", false, 1)]
    static void CreateChurchGrim()
    {
        CreateData("ChurchGrim");
    }

    [MenuItem("DrumonData/Merlion", false, 1)]
    static void CreateMerlion()
    {
        CreateData("Merlion");
    }

    [MenuItem("DrumonData/Otso", false, 1)]
    static void CreateOtso()
    {
        CreateData("Otso");
    }

    private static void CreateData(string name)
    {
        CreatureData data = ScriptableObject.CreateInstance<CreatureData>();
        DrumonParameters m_paramList = (DrumonParameters)Resources.Load("Datas/ExcelSheets/DrumonParameters");

        int i = 0;

        for (i = 0; i < m_paramList.Sheet1.Count; i++)
        {
            if (name.Equals(m_paramList.Sheet1[i].name))
            {
                data.name = m_paramList.Sheet1[i].name;
                data.hp = RandomString2Int(m_paramList.Sheet1[i].hp);
                data.maxHp = data.hp;
                data.atk = RandomString2Int(m_paramList.Sheet1[i].atk);
                data.def = RandomString2Int(m_paramList.Sheet1[i].def);
                data.waitTime = m_paramList.Sheet1[i].waitTime;
                data.elem = m_paramList.Sheet1[i].elem;
                break;
            }
        }

        string fileName = data.name;
        i = 1;
        while (true)
        {
            if (!(bool)Resources.Load("Datas/CreatureData/" + fileName + i.ToString()))
            {
                fileName += i.ToString();
                break;
            }
            i++;
        }

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(data, "Assets/Resources/Datas/CreatureData/" + fileName + ".asset");
#endif
    }

    static public int RandomString2Int(string value)
    {
        int num = 0;
        string[] arr = value.Split('-');
        int rand = arr.Length;
        num = System.Convert.ToInt32(arr[Random.Range(0, rand)]);
        return num;
    }
}
