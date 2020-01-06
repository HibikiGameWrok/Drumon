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
            if(!(bool)Resources.Load("Datas/CreatureData/" + fileName + i.ToString()))
            {
                fileName += i.ToString();
                break;
            }
            i++;
        }

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(data, "Assets/Resources/Datas/CreatureData/" + fileName + ".asset");
#endif

        return data;
    }

    public int RandomString2Int(string value)
    {
        int num = 0;
        string[] arr = value.Split('-');
        int rand = arr.Length;
        num = System.Convert.ToInt32(arr[Random.Range(0,rand)]);
        return num;
    }
}
