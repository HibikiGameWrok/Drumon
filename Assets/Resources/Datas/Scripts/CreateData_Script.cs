using UnityEngine;

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
                data.drumonName = m_paramList.Sheet1[i].name;
                int num = ArrayRandom(m_paramList.Sheet1[i].hp);
                data.level = 1;
                data.hp = String2Int(m_paramList.Sheet1[i].hp, num);
                data.maxHp = data.hp;
                data.atk = String2Int(m_paramList.Sheet1[i].atk, num);
                data.def = String2Int(m_paramList.Sheet1[i].def, num);
                data.waitTime = m_paramList.Sheet1[i].waitTime;
                data.elem = m_paramList.Sheet1[i].elem;
                data.exp = 20;
                int lostPoint = (data.hp + data.atk + data.def) - m_paramList.Sheet1[i].basePoint;
                data.hp -= lostPoint;

                break;
            }
        }

        return data;
    }

    public int ArrayRandom(string array)
    {
        string[] arr = array.Split('-');
        return Random.Range(0, arr.Length);
    }

    public int String2Int(string value, int num)
    {
        string[] arr = value.Split('-');
        return System.Convert.ToInt32(arr[num]);
    }
}
