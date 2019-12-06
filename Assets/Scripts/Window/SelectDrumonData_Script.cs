using UnityEngine;
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
                data.data = m_paramList.Sheet1[i];
                break;
            }
        }

        string fileName = data.data.name;
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
}
