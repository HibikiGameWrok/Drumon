using UnityEngine;
using UnityEditor;

public class CreateData_Script : SingletonBase_Script<CreateData_Script>
{
    [SerializeField]
    private DrumonParameters m_paramList = null;

    public CreatureData CreateData(string name)
    {
        CreatureData data = ScriptableObject.CreateInstance<CreatureData>();

        int i = 0;

        for (i = 0; i < m_paramList.Sheet1.Count; i++)
        {
            if(name.Equals(m_paramList.Sheet1[i].name))
            {
                data.data = m_paramList.Sheet1[i];
                break;
            }
        }

        string fileName = data.data.name;
        i = 1;
        while (true)
        {
            if(!(bool)Resources.Load("Datas/CreatureData/" + fileName + i.ToString()))
            {
                fileName += i.ToString();
                break;
            }
            i++;
        }

        AssetDatabase.CreateAsset(data, "Assets/Resources/Datas/CreatureData/" + fileName + ".asset");

        return data;
    }
}
